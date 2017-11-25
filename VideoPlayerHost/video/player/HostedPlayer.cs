using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using utils;
using video.hosting;

namespace video.player
{
    public class HostedPlayer : IPlayer, IDisposable
    {
        private Process hostProcess;
        private PlayerHost playerHost;
        private PlayerTask playerTask = new PlayerTask();
        private Timer startHostProcessTimer;
        private volatile bool unexpectedTermination = true;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.unexpectedTermination = false;
            if (this.playerHost != null)
            {
                RemotingServices.Disconnect(this.playerHost);
                this.playerHost.Dispose();
                this.playerHost = null;
            }
            if (disposing)
            {
                this.playerTask = null;
            }
            if (this.startHostProcessTimer != null)
            {
                this.startHostProcessTimer.Dispose();
            }
            if (this.hostProcess != null)
            {
                this.hostProcess.Dispose();
            }
        }

        ~HostedPlayer()
        {
            this.Dispose(false);
        }

        public IDisposable Play(MediaStreamInfo mediaStreamInfo, IPlaybackController playbackController)
        {
            Uri uri = new Uri(mediaStreamInfo.url);
            if ((uri == null) || !uri.IsAbsoluteUri)
            {
                throw new Exception("Invalid playback url");
            }
            if (mediaStreamInfo.transport != MediaStreamInfo.Transport.Http)
            {
                if (string.Compare(uri.Scheme, "rtsp", true) != 0)
                {
                    throw new Exception("Invalid playback url");
                }
            }
            else if (string.Compare(uri.Scheme, "rtsp", true) != 0)
            {
                int num;
                if (string.Compare(uri.Scheme, Uri.UriSchemeHttp, true) == 0)
                {
                    num = 80;
                }
                else
                {
                    if (string.Compare(uri.Scheme, Uri.UriSchemeHttps, true) != 0)
                    {
                        throw new Exception("Invalid playback url");
                    }
                    num = 0x1bb;
                }
                UriBuilder builder = new UriBuilder(uri) {
                    Scheme = "rtsp"
                };
                if (builder.Port == -1)
                {
                    builder.Port = num;
                }
                mediaStreamInfo = new MediaStreamInfo(builder.Uri.ToString(), mediaStreamInfo.transport, mediaStreamInfo.userNameToken);
            }
            new SingleAssignmentDisposable();
            this.playerTask.mediaStreamInfo = mediaStreamInfo;
            this.playerTask.playbackController = playbackController;
            if (this.playerHost != null)
            {
                this.playerHost.Dispose();
                RemotingServices.Disconnect(this.playerHost);
                this.playerHost = null;
            }
            this.playerHost = new PlayerHost(this.playerTask);
            RemotingServices.Marshal(this.playerHost);
            IpcChannel channel = AppHosting.SetupChannel();
            string objectUri = RemotingServices.GetObjectUri(this.playerHost);
            string stringToEscape = channel.GetUrlsForUri(objectUri).First<string>();
            CommandLineArgs args = new CommandLineArgs();
            Uri.EscapeDataString(stringToEscape);
            List<string> list = new List<string> {
                stringToEscape
            };
            args.Add("controller-url", list);
            ProcessStartInfo pi = new ProcessStartInfo {
                FileName = Assembly.GetExecutingAssembly().Location,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = string.Join(" ", args.Format())
            };
            pi.EnvironmentVariables["PATH"] = string.Join(
                "; ",
                Enumerable
                    .Select<Bootstrapper.SpecialFolderDescription, string>(Bootstrapper.specialFolders.dlls, sfd => sfd.directory.FullName)
                    .Append<string>(pi.EnvironmentVariables["PATH"])
            );
            this.StartHostProcess(pi);
            return Disposable.Create(() => this.Dispose());
        }

        public void SetMetadataReciever(IMetadataReceiver metadataReceiver)
        {
            this.playerTask.metadataReceiver = metadataReceiver;
        }

        public void SetVideoBuffer(VideoBuffer videoBuffer)
        {
            this.playerTask.videoBuffer = videoBuffer;
        }

        private void StartHostProcess(ProcessStartInfo pi)
        {
            if (this.hostProcess != null)
            {
                this.hostProcess.Dispose();
            }
            this.hostProcess = Process.Start(pi);
            this.hostProcess.Exited += delegate (object s, EventArgs o) {
                TimerCallback callback = null;
                if (this.unexpectedTermination)
                {
                    if (this.startHostProcessTimer != null)
                    {
                        this.startHostProcessTimer.Dispose();
                    }
                    if (callback == null)
                    {
                        callback = state => this.StartHostProcess(pi);
                    }
                    this.startHostProcessTimer = new Timer(callback, null, TimeSpan.FromSeconds(1.5), TimeSpan.FromMilliseconds(-1.0));
                }
            };
            this.hostProcess.EnableRaisingEvents = true;
        }

        private class PlayerHost : MarshalByRefObject, IHostController, IPlaybackController, IDisposable
        {
            private IPlaybackController playbackController;
            private IPlaybackSession playbackSession;
            public HostedPlayer.PlayerTask playerTask;
            private object syn = new object();

            public PlayerHost(HostedPlayer.PlayerTask playerTask)
            {
                this.playbackController = playerTask.playbackController;
                playerTask.playbackController = this;
                this.playerTask = playerTask;
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    IPlaybackController playbackController = null;
                    IPlaybackSession playbackSession = null;
                    lock (this.syn)
                    {
                        playbackSession = this.playbackSession;
                        playbackController = this.playbackController;
                        this.playbackController = null;
                        this.playbackSession = null;
                    }
                    if (playbackController != null)
                    {
                        playbackController.Shutdown();
                    }
                    if (playbackSession != null)
                    {
                        try
                        {
                            playbackSession.Close();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            ~PlayerHost()
            {
                this.Dispose(false);
            }

            public override object InitializeLifetimeService() => 
                null;

            void IHostController.Bye()
            {
            }

            Action<IHostController> IHostController.Hello()
            {
                if (this.playbackController != null)
                {
                    return new Action<IHostController>(this.playerTask.Start);
                }
                return delegate (IHostController hostController) {
                };
            }

            bool IHostController.isAlive() => 
                (this.playbackController != null);

            bool IPlaybackController.Initialized(IPlaybackSession playbackSession)
            {
                IPlaybackController playbackController = null;
                lock (this.syn)
                {
                    if (this.playbackController == null)
                    {
                        return false;
                    }
                    playbackController = this.playbackController;
                }
                bool flag = playbackController.Initialized(playbackSession);
                lock (this.syn)
                {
                    if (this.playbackController == null)
                    {
                        return false;
                    }
                    if (flag)
                    {
                        this.playbackSession = playbackSession;
                    }
                    return flag;
                }
            }

            void IPlaybackController.Shutdown()
            {
                this.Dispose();
            }
        }

        [Serializable]
        public class PlayerTask
        {
            public bool keepAlive = true;
            public MediaStreamInfo mediaStreamInfo;
            public IMetadataReceiver metadataReceiver;
            public IPlaybackController playbackController;
            public VideoBuffer videoBuffer;

            public void Start(IHostController hostController)
            {
                Action<long> action = null;
                AppHosting.SetupChannel();
                SingleAssignmentDisposable d = new SingleAssignmentDisposable();
                if (this.keepAlive)
                {
                    if (action == null)
                    {
                        action = delegate (long i) {
                            try
                            {
                                if (!hostController.isAlive())
                                {
                                    d.Dispose();
                                    Process.GetCurrentProcess().Kill();
                                }
                            }
                            catch (Exception)
                            {
                                Process.GetCurrentProcess().Kill();
                            }
                        };
                    }
                    d.Disposable = ObservableExtensions.Subscribe<long>(Observable.Interval(TimeSpan.FromMilliseconds(500.0)), action);
                }
                new Live555(this.videoBuffer, this.metadataReceiver).Play(this.mediaStreamInfo, this.playbackController);
                d.Dispose();
            }
        }
    }
}

