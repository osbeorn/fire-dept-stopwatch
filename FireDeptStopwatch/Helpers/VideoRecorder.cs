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

namespace FireDeptStopwatch.Helpers
{
    public class VideoRecorder : IPlaybackController
    {
        private Guid instanceId;

        private NetworkCredential credentials;

        private HostedPlayer player;
        private VideoBuffer buffer;
        private IPlaybackSession playbackSession;

        private Size videoSize = new Size(720, 576); // default size

        private Dispatcher dispatcher;

        private CompositeDisposable disposables = new CompositeDisposable();

        private bool isRecording;
        private TimerResult result;

        private Queue<Bitmap> bitmaps;

        public EventHandler OnConnect;

        public VideoRecorder(CameraInfo cameraInfo)
        {
            instanceId = Guid.NewGuid();
            bitmaps = new Queue<Bitmap>();

            Connect(cameraInfo);
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
            
            isRecording = true;

            ThreadPool.QueueUserWorkItem(SaveBitmaps, null);
        }

        public void StopRecording(TimerResult result)
        {
            this.result = result;

            isRecording = false;
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
            OnConnect(this, null);

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
                    catch (Exception e)
                    {

                    }
                    finally
                    {

                    }
                }
            }, cancellationToken);
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, int count);
        private void DrawFrame(VideoBuffer videoBuffer)
        {
            if (isRecording)
            {
                Bitmap bitmap = new Bitmap(videoBuffer.width, videoBuffer.height);
                BitmapData bitmapData = null;

                try
                {
                    bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    using (var md = videoBuffer.Lock())
                    {
                        CopyMemory(bitmapData.Scan0, md.value.scan0Ptr, buffer.stride * buffer.height);
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                bitmaps.Enqueue(bitmap);
                //bitmaps.Add(bitmap);
            }
        }

        private void SaveBitmaps(object state)
        {
            //using (var appScope = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    using (var stream = new IsolatedStorageFileStream("results.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, appScope))
            //    {
            //        BinaryFormatter bin = new BinaryFormatter();
            //        bin.Serialize(stream, resultList);
            //    }
            //}

            var videoName = "video_" + instanceId + ".avi";
            var sourcePath = Path.Combine(Path.GetTempPath(), videoName);

            using (var writer = new VideoFileWriter())
            {
                writer.Open(sourcePath, videoSize.Width, videoSize.Height, 25, VideoCodec.MPEG4);

                while (bitmaps != null)
                {
                    if (bitmaps.Count == 0 && !isRecording)
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

                writer.Close();
            }

            var targetPath = Path.Combine(ConfigurationManager.AppSettings["videosFolder"], result.DateTime.ToString(@"dd\.MM\.yyyy") + "-" + result.Result.ToString(@"mm\.ss\.ffff"));

            Directory.CreateDirectory(targetPath);
            File.Move(sourcePath, Path.Combine(targetPath, videoName));
        }

        //private void SaveRecording(string filePath)
        //{
        //    //while (bitmaps.Count > 0)
        //    //{
        //    //    continue;
        //    //}

        //    //var bitmapCheck = Image.FromFile("D:\\Users\\Benjamin\\Desktop\\screenshots\\" + instanceId + "\\0.png") as Bitmap;
        //    //if (bitmapCheck == null)
        //    //{
        //    //    return;
        //    //}

        //    //using (var writer = new VideoFileWriter())
        //    //{
        //    //    writer.Open("D:\\Users\\Benjamin\\Desktop\\video.mp4", bitmapCheck.Width, bitmapCheck.Height, 25, VideoCodec.MPEG4);
        //    //    foreach (var bitmap in Directory.GetFiles("D:\\Users\\Benjamin\\Desktop\\screenshots\\" + instanceId, "*.png").OrderBy(name => name))
        //    //    {
        //    //        if (bitmap != null)
        //    //        {
        //    //            writer.WriteVideoFrame(Image.FromFile(bitmap) as Bitmap);
        //    //            File.Delete(bitmap);
        //    //        }
        //    //    }
        //    //    writer.Close();
        //    //}

        //    if (bitmaps == null || bitmaps.Count == 0)
        //    {
        //        return;
        //    }

        //    using (var writer = new VideoFileWriter())
        //    {
        //        writer.Open("D:\\Users\\Benjamin\\Desktop\\video.mp4", bitmaps[0].Width, bitmaps[0].Height, 25, VideoCodec.MPEG4);
        //        foreach (var bitmap in bitmaps)
        //        {
        //            writer.WriteVideoFrame(bitmap);
        //        }
        //        writer.Close();
        //    }

        //    bitmaps.ForEach(b => b.Dispose());
        //    bitmaps.Clear();
        //}
    }
}
