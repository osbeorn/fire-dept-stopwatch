using odm.core;
using onvif.utils;
using utils;
using FireDeptStopwatch.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using video.player;
using onvif.services;
using System.IO;
using Accord.Video.FFMPEG;
using System.Net;
using FireDeptStopwatch.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Deployment.Application;
using System.Windows.Forms;

namespace FireDeptStopwatch.Helpers
{
    public class VideoRecorder : IPlaybackController
    {
        private Guid instanceId;

        private NetworkCredential credentials;

        private CameraInfo cameraInfo;

        private HostedPlayer player;
        private VideoBuffer buffer;
        private IPlaybackSession playbackSession;

        private Size videoSize = new Size(720, 576); // default size
        private int frameRate = 25; // default frame rate

        private Dispatcher dispatcher;

        private CompositeDisposable disposables;

        private bool IsConnected { get; set; }
        private bool IsRecording { get; set; }
        private bool IsReset { get; set; }

        private TimerResult result;

        private Queue<Bitmap> bitmaps;

        public EventHandler OnConnect;
        public EventHandler OnStart;
        public EventHandler OnConvertStart;
        public EventHandler OnConvertEnd;

        public VideoRecorder(CameraInfo cameraInfo)
        {
            this.cameraInfo = cameraInfo;

            instanceId = Guid.NewGuid();

            bitmaps = new Queue<Bitmap>();
            disposables = new CompositeDisposable();

            GetVideoSize(cameraInfo);
        }

        public bool Initialized(IPlaybackSession playbackSession)
        {
            this.playbackSession = playbackSession;
            return true;   
        }

        public void Shutdown()
        {
            
        }

        public void StartRecording()
        {
            bitmaps = new Queue<Bitmap>();        
            disposables = new CompositeDisposable();

            IsRecording = true;
            IsReset = false;

            OnStart(this, null);

            Connect(cameraInfo);

            ThreadPool.QueueUserWorkItem(SaveBitmaps, null);
        }

        public void StopRecording(TimerResult result)
        {
            this.result = result;

            IsConnected = false;
            IsRecording = false;
        }

        public void StopRecording()
        {
            IsConnected = false;
            IsRecording = false;
            IsReset = true;
        }

        private void GetVideoSize(CameraInfo cameraInfo)
        {
            if (!string.IsNullOrEmpty(cameraInfo.User))
            {
                credentials = new NetworkCredential(cameraInfo.User, cameraInfo.Password);
            }

            var cameraUrl = CameraHelper.BuildCameraUrl(cameraInfo.Url);

            var factory = new NvtSessionFactory(credentials);
            disposables.Add(
                factory
                .CreateSession(new Uri[] { new Uri(cameraUrl) })
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    session => {
                        GetVideoSize(session);
                    },
                    err => {
                    }
                )
            );
        }

        private void GetVideoSize(INvtSession session)
        {
            disposables.Add(
                session
                .GetProfiles()
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    profs => {
                        var profile = profs.FirstOrDefault();
                        if (profile != null)
                        {
                            GetVideoSize(session, profile);
                        }
                    },
                    err => {
                    }
                )
            );
        }

        private void GetVideoSize(INvtSession session, Profile profile)
        {
            var streamSetup = new StreamSetup()
            {
                stream = StreamType.rtpUnicast,
                transport = new Transport()
                {
                    protocol = TransportProtocol.tcp
                }
            };

            disposables.Add(
                session
                .GetStreamUri(streamSetup, profile.token)
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    muri => {
                        OnConnect(this, null);

                        videoSize = new Size(profile.videoEncoderConfiguration.resolution.width, profile.videoEncoderConfiguration.resolution.height);
                        frameRate = profile.videoEncoderConfiguration.rateControl.frameRateLimit;
                    },
                    err => {
                    }
                )
            );
        }

        private void Connect(CameraInfo cameraInfo)
        {
            if (!string.IsNullOrEmpty(cameraInfo.User))
            {
                credentials = new NetworkCredential(cameraInfo.User, cameraInfo.Password);
            }

            var cameraUrl = CameraHelper.BuildCameraUrl(cameraInfo.Url);

            var factory = new NvtSessionFactory(credentials);
            disposables.Add(
                factory
                .CreateSession(new Uri[] { new Uri(cameraUrl) })
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    session => {
                        GetProfiles(session);
                    },
                    err => {
                    }
                )
            );
        }

        private void GetProfiles(INvtSession session)
        {
            disposables.Add(
                session
                .GetProfiles()
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    profs => {
                        var profile = profs.FirstOrDefault();
                        if (profile != null)
                        {
                            GetStreamUri(session, profile);
                        }
                    },
                    err => {
                    }
                )
            );
        }

        private void GetStreamUri(INvtSession session, Profile profile)
        {
            var streamSetup = new StreamSetup()
            {
                stream = StreamType.rtpUnicast,
                transport = new Transport()
                {
                    protocol = TransportProtocol.tcp
                }
            };

            disposables.Add(
                session
                .GetStreamUri(streamSetup, profile.token)
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    muri => {
                        InitializePlayer(muri.uri.ToString(), credentials, videoSize);
                    },
                    err => {
                    }
                )
            );
        }

        private void InitializePlayer(string videoUri, NetworkCredential account, Size size = default(Size))
        {
            IsConnected = true;

            dispatcher = Dispatcher.CurrentDispatcher;

            player = new HostedPlayer();
            disposables.Add(player);

            if (size != default(Size))
            {
                buffer = new VideoBuffer(size.Width, size.Height, PixFrmt.bgra32);
            }
            else
            {
                buffer = new VideoBuffer(720, 576, PixFrmt.bgra32);
            }
            player.SetVideoBuffer(buffer);

            MediaStreamInfo.Transport transport = MediaStreamInfo.Transport.Tcp;
            MediaStreamInfo streamInfo = null;
            if (account != null)
            {
                streamInfo = new MediaStreamInfo(videoUri, transport, new UserNameToken(account.UserName, account.Password));
            }
            else
            {
                streamInfo = new MediaStreamInfo(videoUri, transport);
            }

            disposables.Add(player.Play(streamInfo, this));

            TimeSpan renderInterval;
            try
            {
                int fps = 25;
                fps = (fps <= 0 || fps > 100) ? 100 : fps;
                renderInterval = TimeSpan.FromMilliseconds(1000 / fps);
            }
            catch
            {
                renderInterval = TimeSpan.FromMilliseconds(1000 / 30);
            }

            var cancellationTokenSource = new CancellationTokenSource();
            disposables.Add(Disposable.Create(() => {
                cancellationTokenSource.Cancel();
            }));

            var cancellationToken = cancellationTokenSource.Token;

            var renderingTask = Task.Factory.StartNew(() => {
                using (buffer.Lock())
                {
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            using (var processingEvent = new ManualResetEventSlim(false))
                            {
                                var dispOp = dispatcher.BeginInvoke(() => {
                                    using (Disposable.Create(() => processingEvent.Set()))
                                    {
                                        if (!cancellationToken.IsCancellationRequested)
                                        {
                                            DrawFrame(buffer);
                                        }
                                    }
                                });
                                processingEvent.Wait(cancellationToken);
                            }
                            cancellationToken.WaitHandle.WaitOne(renderInterval);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //swallow exception
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {

                    }
                }
            }, cancellationToken);
        }

        private void DrawFrame(VideoBuffer videoBuffer)
        {
            if (IsRecording && IsConnected)
            {
                Bitmap bitmap = new Bitmap(videoBuffer.width, videoBuffer.height);
                BitmapData bitmapData = null;

                try
                {
                    bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    using (var md = videoBuffer.Lock())
                    {
                        NativeMethods.CopyMemory(bitmapData.Scan0, md.value.scan0Ptr, buffer.stride * buffer.height);
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                bitmaps.Enqueue(bitmap);
            }
        }

        private void SaveBitmaps(object state)
        {
            while (!IsConnected)
            {
                if (IsReset)
                {
                    disposables.Dispose();
                    return;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(500));
            }

            string appDataFolder;
            if (Debugger.IsAttached)
            {
                appDataFolder = String.Empty;
            }
            else
            {
                appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FireDeptStopwatch");
            }

            var sourceVideoName = Guid.NewGuid().ToString() + ".avi";
            var targetVideoName = "video_" + instanceId + ".avi";

            var sourcePath = Path.Combine(appDataFolder, "TempRecordings", sourceVideoName);

            using (var writer = new VideoFileWriter())
            {
                writer.Width = videoSize.Width;
                writer.Height = videoSize.Height;
                writer.FrameRate = frameRate;
                writer.VideoCodec = cameraInfo.HdRecording ? VideoCodec.FfvHuff : VideoCodec.Default;

                writer.Open(sourcePath);

                while (bitmaps != null)
                {
                    if (bitmaps.Count == 0 && !IsRecording)
                    {
                        bitmaps.Clear();
                        bitmaps = null;
                    }
                    else if (bitmaps.Count > 0)
                    {
                        var bitmap = bitmaps.Dequeue();
                        writer.WriteVideoFrame(bitmap);
                        bitmap.Dispose();
                    }
                }
            }

            if (!IsReset)
            {
                OnConvertStart(this, null);

                string targetPath = Path.Combine(appDataFolder, "Recordings", result.DateTime.ToString(@"dd\.MM\.yyyy-HH\.mm\.ss") + "-" + result.Result.ToString(@"mm\.ss\.ffff"));
                Directory.CreateDirectory(targetPath);

                File.Move(sourcePath, Path.Combine(targetPath, targetVideoName));

                OnConvertEnd(this, null);
            }

            //if (!IsReset)
            //{
            //    OnConvertStart(this, null);

            //    string targetPath = Path.Combine(appDataFolder, "Recordings", result.DateTime.ToString(@"dd\.MM\.yyyy-HH\.mm\.ss") + "-" + result.Result.ToString(@"mm\.ss\.ffff"));
            //    Directory.CreateDirectory(targetPath);

            //    targetPath = Path.Combine(targetPath, targetVideoName);

            //    using (var reader = new VideoFileReader())
            //    using (var writer = new VideoFileWriter())
            //    {
            //        reader.Open(sourcePath);

            //        writer.Width = reader.Width;
            //        writer.Height = reader.Height;
            //        writer.FrameRate = reader.FrameRate;
            //        writer.VideoCodec = VideoCodec.FfvHuff;

            //        writer.Open(targetPath);

            //        while (true)
            //        {
            //            var bitmap = reader.ReadVideoFrame();
            //            if (bitmap == null)
            //            {
            //                break;
            //            }

            //            writer.WriteVideoFrame(bitmap);

            //            bitmap.Dispose();
            //        }
            //    }
            //}

            //OnConvertEnd(this, null);

            //File.Delete(sourcePath);

            disposables.Dispose();
        }
    }
}
