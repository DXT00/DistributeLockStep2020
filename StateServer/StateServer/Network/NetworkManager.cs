using System;
using System.Collections.Generic;
using System.Net;
using StateServer.GameMap;
using StateServer.Network.Socket;
namespace StateServer.Network
{
    public sealed class NetworkManager
    {
        static private NetworkManager s_singleton = null;
        static private readonly object s_locker = new object();

        static private MapManager m_mapManager = MapManager.get_singleton();

        TCPSocket m_tcpServer;
        int m_numConnectionsMax;
        int m_recieveBufferSize;
        IPEndPoint m_ipEndPort;

        private NetworkManager()
        {
            m_numConnectionsMax = Config.numConnectionsMax;
            m_recieveBufferSize = Config.receiveBufferSize;
            m_tcpServer = new TCPSocket(m_numConnectionsMax, m_recieveBufferSize);
            IPAddress ip = IPAddress.Parse(Config.ip);
            m_ipEndPort = new IPEndPoint(ip, Config.port);

        }

        public static NetworkManager get_singleton()
        {
            if (s_singleton == null)
            {
                lock (s_locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new NetworkManager();
                    }
                }
            }

            return s_singleton;
        }
        
        public void start_tcpServer()
        {
            Log.INFO("tcpServer start listening ...");
            m_tcpServer.start(m_ipEndPort);
        }

        public void stop_tcpServer()
        {
            m_tcpServer.stop();
        }
        public void receive_data()
        {

        }
        public void send_data()
        {

        }

    }

    
}
