using odm.core;
using onvif.utils;
using utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using onvif.services;
using video.player;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows.Threading;
using FireDeptStopwatch.Classes;

namespace FireDeptStopwatch.Forms
{
    public partial class CameraDisplayForm : Form, IPlaybackController
    {
        private string cameraUrl;

        private string user;
        private string password;

        HostedPlayer hostedPlayer;
        VideoBuffer videoBuffer;
        IPlaybackSession playbackSession;

        object sync = new object();

        CompositeDisposable disposables = new CompositeDisposable();
        CompositeDisposable playerDisposables = new CompositeDisposable();

        Dispatcher dispatcher;
        Image image;

        NetworkCredential credentials = null;

        INvtSession session;
        Profile profile;
        string profileToken;
        string nodeToken;

        Space2DDescription panTiltSpace;

        public CameraDisplayForm(CameraInfo camera)
        {
            InitializeComponent();

            dispatcher = Dispatcher.CurrentDispatcher;

            if (camera.Url.StartsWith("http://"))
            {
                this.cameraUrl = camera.Url;
            }
            else
            {
                this.cameraUrl = "http://" + camera.Url;
            }
            this.cameraUrl += "/onvif/device_service";

            this.user = camera.User;
            this.password = camera.Password;

            Connect();
        }

        public bool Initialized(IPlaybackSession playbackSession)
        {
            this.playbackSession = playbackSession;
            return true;
        }

        public void Shutdown()
        {

        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            //Connect();
        }

        //private void VideoNewFrame(object sender, NewFrameEventArgs eventArgs)
        //{
        //    // get new frame
        //    Bitmap bitmap = eventArgs.Frame;
        //    // process the frame
        //}

        private void Connect()
        {
            if (!string.IsNullOrEmpty(user))
            {
                credentials = new NetworkCredential(user, password);
            }

            var factory = new NvtSessionFactory(credentials);
            disposables.Add(
                factory
                .CreateSession(new Uri[] { new Uri(cameraUrl) })
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    session => {
                        this.session = session;

                        GetProfiles(session);
                        //GetImagingSettings(session);
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
                        profile = profs.FirstOrDefault();
                        if (profile != null)
                        {
                            profileToken = profile.token;
                            nodeToken = profile.ptzConfiguration.nodeToken;

                            GetPtzConfiguration(session, profile);
                            GetStreamUri(session, profile);
                        }
                    },
                    err => {
                    }
                )
            );
        }

        //private void GetImagingSettings(INvtSession session)
        //{
        //    disposables.Add(
        //        session
        //        .GetImagingSettings("")
        //        .ObserveOnCurrentDispatcher()
        //        .Subscribe(
        //            profs => {
        //                if (profile != null)
        //                {
        //                    profileToken = profile.token;
        //                    nodeToken = profile.ptzConfiguration.nodeToken;

        //                    GetPtzConfiguration(session, profile);
        //                    GetStreamUri(session, profile);
        //                }
        //            },
        //            err => {
        //            }
        //        )
        //    );
        //}

        private void GetPtzConfiguration(INvtSession session, Profile profile)
        {
            disposables.Add(
                session
                .GetNode(nodeToken)
                .ObserveOnCurrentDispatcher()
                .Subscribe(
                    node =>
                    {
                        if (node != null)
                        {
                            panTiltSpace = node.supportedPTZSpaces.continuousPanTiltVelocitySpace.FirstOrDefault();
                        }
                    },
                    err =>
                    {
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
                        var videoSize = new Size(profile.videoEncoderConfiguration.resolution.width, profile.videoEncoderConfiguration.resolution.height);
                        InitPlayer(muri.uri.ToString(), credentials, videoSize);
                    },
                    err => {
                    }
                )
            );
        }

        private void InitPlayer(string videoUri, NetworkCredential account, Size size = default(Size))
        {
            hostedPlayer = new HostedPlayer();
            playerDisposables.Add(hostedPlayer);

            if (size != default(Size))
            {
                videoBuffer = new VideoBuffer(size.Width, size.Height, PixFrmt.bgra32);
            }
            else
            {
                videoBuffer = new VideoBuffer(720, 576, PixFrmt.bgra32);
            }
            hostedPlayer.SetVideoBuffer(videoBuffer);

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

            playerDisposables.Add(
                hostedPlayer.Play(streamInfo, this)
            );

            InitPlayback(videoBuffer, true);
        }

        private void InitPlayback(VideoBuffer videoBuffer, bool isInitial)
        {
            if (videoBuffer == null)
            {
                dbg.Break();
                log.WriteError("video buffer is null");
            }

            TimeSpan renderInterval;
            try
            {
                int fps = 30;
                fps = (fps <= 0 || fps > 100) ? 100 : fps;
                renderInterval = TimeSpan.FromMilliseconds(1000 / fps);
            }
            catch
            {
                renderInterval = TimeSpan.FromMilliseconds(1000 / 30);
            }

            var cancellationTokenSource = new CancellationTokenSource();
            playerDisposables.Add(Disposable.Create(() => {
                cancellationTokenSource.Cancel();
            }));

            image = new Bitmap(videoBuffer.width, videoBuffer.height);
            pictureBox.Image = image;

            var cancellationToken = cancellationTokenSource.Token;

            var renderingTask = Task.Factory.StartNew(delegate {
                //var statistics = PlaybackStatistics.Start(Restart, isInitial);
                using (videoBuffer.Lock())
                {
                    try
                    {
                        //start rendering loop
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            using (var processingEvent = new ManualResetEventSlim(false))
                            {
                                var dispOp = dispatcher.BeginInvoke(() => {
                                    using (Disposable.Create(() => processingEvent.Set()))
                                    {
                                        if (!cancellationToken.IsCancellationRequested)
                                        {
                                            DrawFrame(videoBuffer);
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
                    catch (Exception error)
                    {
                        dbg.Break();
                        log.WriteError(error);
                    }
                    finally
                    {
                    }
                }
            }, cancellationToken);
        }

        void Restart()
        {
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int count);
        private void DrawFrame(VideoBuffer videoBuffer/*, PlaybackStatistics statistics*/)
        {
            Bitmap bitmap = image as Bitmap;
            BitmapData bitmapData = null;
            try
            {
                bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                using (var md = videoBuffer.Lock())
                {
                    CopyMemory(bitmapData.Scan0, md.value.scan0Ptr, this.videoBuffer.stride * this.videoBuffer.height);
                }
            }
            catch (Exception)
            {
                //errBox.Text = err.Message;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            pictureBox.Image = bitmap;
        }

        private void Release()
        {
            disposables.Dispose();
            disposables = new CompositeDisposable();

            playerDisposables.Dispose();
            playerDisposables = new CompositeDisposable();
        }

        private float GetPanSpeed()
        {
            try
            {
                if (panTiltSpace.xRange.max - panTiltSpace.xRange.min == 0)
                    return 0;
                else
                    return panTiltSpace.xRange.max;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private float GetTiltSpeed()
        {
            try
            {
                if (panTiltSpace.yRange.max - panTiltSpace.yRange.min == 0)
                {
                    return 0;
                }
                else
                {
                    return panTiltSpace.yRange.max;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private bool upButtonDown = false;
        private void TurnUpButton_MouseDown(object sender, MouseEventArgs e)
        {
            upButtonDown = true;

            do
            {
                lock (sync)
                {
                    var speed = new PTZSpeed
                    {
                        zoom = null,
                        panTilt = new Vector2D
                        {
                            x = 0,
                            y = GetTiltSpeed()
                        }
                    };

                    session.ContinuousMove(profileToken, speed, null).RunSynchronously();
                    session.Stop(profileToken, true, true).RunSynchronously();

                    Application.DoEvents();
                }
            } while (upButtonDown);
        }

        private void TurnUpButton_MouseUp(object sender, MouseEventArgs e)
        {
            upButtonDown = false;
        }

        private bool downButtonDown = false;
        private void TurnDownButton_MouseDown(object sender, MouseEventArgs e)
        {
            downButtonDown = true;

            do
            {
                lock (sync)
                {
                    var speed = new PTZSpeed
                    {
                        zoom = null,
                        panTilt = new Vector2D
                        {
                            x = 0,
                            y = -1 * GetTiltSpeed()
                        }
                    };

                    session.ContinuousMove(profileToken, speed, null).RunSynchronously();
                    session.Stop(profileToken, true, true).RunSynchronously();

                    Application.DoEvents();
                }
            } while (downButtonDown);
        }

        private void TurnDownButton_MouseUp(object sender, MouseEventArgs e)
        {
            downButtonDown = false;
        }

        private bool leftButtonDown = false;
        private void TurnLeftButton_MouseDown(object sender, MouseEventArgs e)
        {
            leftButtonDown = true;

            do
            {
                lock (sync)
                {
                    var speed = new PTZSpeed
                    {
                        zoom = null,
                        panTilt = new Vector2D
                        {
                            x = -1 * GetPanSpeed(),
                            y = 0
                        }
                    };

                    session.ContinuousMove(profileToken, speed, null).RunSynchronously();
                    session.Stop(profileToken, true, true).RunSynchronously();

                    Application.DoEvents();
                }
            } while (leftButtonDown);
        }

        private void TurnLeftButton_MouseUp(object sender, MouseEventArgs e)
        {
            leftButtonDown = false;
        }

        private bool rightButtonDown = false;
        private void TurnRightButton_MouseDown(object sender, MouseEventArgs e)
        {
            rightButtonDown = true;

            do
            {
                lock (sync)
                {
                    var speed = new PTZSpeed
                    {
                        zoom = null,
                        panTilt = new Vector2D
                        {
                            x = GetPanSpeed(),
                            y = 0
                        }       
                    };

                    session.ContinuousMove(profileToken, speed, null).RunSynchronously();
                    session.Stop(profileToken, true, true).RunSynchronously();

                    Application.DoEvents();
                }
            } while (rightButtonDown);
        }

        private void TurnRightButton_MouseUp(object sender, MouseEventArgs e)
        {
            rightButtonDown = false;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            playerDisposables.Dispose();
            disposables.Dispose();

            Dispose();
            Close();
        }
    }
}
