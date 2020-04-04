using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
using StateClient.Network.Socket;
namespace StateClient.Network
{
    public class NetworkManager
    {
        static private NetworkManager s_singleton = null;
        static private readonly object s_locker = new object();


        private NetworkManager()
        {
           
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

        public void connect(List<Robot> robots)
        {
            Log.INFO("tcpSocket connecting ...");
            foreach (Robot robot in robots)
            {
                robot.m_tcpSocket.connect();
            }
        }

        public void receive_server_data()
        {

        }

        public void send_robotsData(List<Robot> robots)
        {
            Log.INFO("tcpSocket sending robots msgs...");
            foreach (Robot robot in robots)
            {
                robot.m_tcpSocket.send_queue_data();
            }
        }
    }
}
