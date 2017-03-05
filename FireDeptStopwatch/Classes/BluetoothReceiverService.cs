using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FireDeptStopwatch.Classes
{
    public class BluetoothReceiverService
    {
        private readonly Guid serviceId = new Guid("2227A283-88B4-43D8-A67D-CDA436DA74B7");

        private static BluetoothReceiverService instance;

        private BluetoothListener listener;
        private Action<string> responseAction;
        private CancellationTokenSource cancelSource;
        private bool WasStarted { get; set; }
        private string status;

        private BluetoothReceiverService() { }

        public static BluetoothReceiverService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BluetoothReceiverService();
                }
                return instance;
            }
        }

        public void Start(Action<string> reportAction)
        {
            WasStarted = true;
            responseAction = reportAction;

            if (cancelSource != null && listener != null)
            {
                Dispose(true);
            }

            listener = new BluetoothListener(serviceId)
            {
                ServiceName = "FireDeptStopwatchBTReceiverService"
            };

            listener.Start();

            cancelSource = new CancellationTokenSource();

            Task.Factory.StartNew(() => Listener(cancelSource));
        }

        public void Stop()
        {
            WasStarted = false;
            cancelSource.Cancel();
        }

        private void Listener(CancellationTokenSource token)
        {
            try
            {
                while (true)
                {
                    using (var client = listener.AcceptBluetoothClient())
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        using (var streamReader = new StreamReader(client.GetStream()))
                        {
                            try
                            {
                                var content = streamReader.ReadToEnd();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    responseAction(content);
                                }
                            }
                            catch (IOException)
                            {
                                client.Close();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // todo handle the exception
                // for the sample it will be ignored
            }
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cancelSource != null)
                {
                    listener.Stop();
                    listener = null;
                    cancelSource.Dispose();
                    cancelSource = null;
                }
            }
        }
    }
}
