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

        private Dispatcher dispatcher;

        private CompositeDisposable disposables = new CompositeDisposable();

        private bool IsConnected { get; set; }
        private bool IsRecording { get; set; }

        private TimerResult result;

        private Queue<Bitmap> bitmaps;

        public EventHandler OnConnect;

        public VideoRecorder(CameraInfo cameraInfo)
        {
            this.cameraInfo = cameraInfo;

            instanceId = Guid.NewGuid();
            bitmaps = new Queue<Bitmap>();

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
            IsRecording = true;

            Connect(cameraInfo);

            ThreadPool.QueueUserWorkItem(SaveBitmaps, null);
        }

        public void StopRecording(TimerResult result)
        {
            this.result = result;

            IsConnected = false;
            IsRecording = false;
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
            OnConnect(this, null);

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
                        videoSize = new Size(profile.videoEncoderConfiguration.resolution.width, profile.videoEncoderConfiguration.resolution.height);
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
                        videoSize = new Size(profile.videoEncoderConfiguration.resolution.width, profile.videoEncoderConfiguration.resolution.height);
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
                ;
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
                writer.Open(sourcePath, videoSize.Width, videoSize.Height, 25, VideoCodec.Default);

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

            string targetPath = Path.Combine(appDataFolder, "Recordings", result.DateTime.ToString(@"dd\.MM\.yyyy-HH\.mm\.ss") + "-" + result.Result.ToString(@"mm\.ss\.ffff"));
            Directory.CreateDirectory(targetPath);

            File.Move(sourcePath, Path.Combine(targetPath, targetVideoName));
        }
    }
}
