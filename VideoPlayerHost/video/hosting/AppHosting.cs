using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace video.hosting
{
    public static class AppHosting
    {
        private static IpcChannel m_hostChannel = null;
        private static object sync = new object();

        public static IpcChannel SetupChannel()
        {
            lock (sync)
            {
                if (m_hostChannel == null)
                {
                    string str = Guid.NewGuid().ToString();
                    BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider();
                    BinaryClientFormatterSinkProvider clientSinkProvider = new BinaryClientFormatterSinkProvider();
                    serverSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
                    Dictionary<string, string> properties = new Dictionary<string, string> {
                        { 
                            "portName",
                            str
                        }
                    };
                    m_hostChannel = new IpcChannel(properties, clientSinkProvider, serverSinkProvider);
                    ChannelServices.RegisterChannel(m_hostChannel, false);
                }
            }
            return m_hostChannel;
        }
    }
}

