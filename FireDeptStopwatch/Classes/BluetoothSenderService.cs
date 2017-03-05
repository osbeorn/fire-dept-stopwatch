using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDeptStopwatch.Classes
{
    public class BluetoothSenderService
    {
        private readonly Guid serviceId = new Guid("8E2BD875-1BD3-4155-BEA5-1424022DF60D");

        private static BluetoothSenderService instance;

        private BluetoothSenderService() { }

        public static BluetoothSenderService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BluetoothSenderService();
                }
                return instance;
            }
        }

        public async Task<bool> Send(BluetoothDeviceInfo device, string content)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            // for not block the UI it will run in a different threat
            var task = Task<bool>.Factory.StartNew(() =>
            {
                using (var bluetoothClient = new BluetoothClient())
                {
                    try
                    {
                        var endpoint = new BluetoothEndPoint(device.DeviceAddress, serviceId);

                        // connecting
                        bluetoothClient.Connect(endpoint);

                        // get stream for send the data
                        var bluetoothStream = bluetoothClient.GetStream();

                        // if all is ok to send
                        if (bluetoothClient.Connected && bluetoothStream != null)
                        {
                            // write the data in the stream
                            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                            bluetoothStream.Write(buffer, 0, buffer.Length);
                            bluetoothStream.Flush();
                            bluetoothStream.Close();

                            return true;
                        }

                        return false;
                    }
                    catch
                    {
                        // the error will be ignored and the send data will report as not sent
                        // for understood the type of the error, handle the exception
                    }
                }

                return false;
            });

            return await task;
        }
    }
}
