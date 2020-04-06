using System;
using System.Collections.Generic;
using System.Net;
using StateServer.GameMap;
using StateServer.Network.Socket;
using StateServer.RobotEntity;
using StateServer.RobotsSystem;

namespace StateServer.Network
{
    public sealed class NetworkManager
    {
        static private NetworkManager s_singleton = null;
        static private readonly object s_locker = new object();

        static private MapManager m_mapManager = MapManager.get_singleton();

        ServerSocket m_tcpServer;
        int m_numConnectionsMax;
        int m_recieveBufferSize;
        IPEndPoint m_ipEndPort;

        private NetworkManager()
        {
            m_numConnectionsMax = Config.numConnectionsMax;
            m_recieveBufferSize = Config.receiveBufferSize;
            m_tcpServer = new ServerSocket(m_numConnectionsMax, m_recieveBufferSize);
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
        //更新robot数组信息
        public void receive_robots_data()
        {
            List<Robot> robots = RobotSystem.get_singleton().get_robots();
            foreach(Robot robot in robots)
            {
                if (robot == null) continue;//断开连接时会使得robot = null
                Queue<NetworkMsg> receiveQue = robot.m_clientSocket.load_receive_queue();
                while(receiveQue.Count > 0){
                    NetworkMsg msg = receiveQue.Dequeue();
                    if (msg.MsgType == Type.StartGame)
                    {
                        Log.INFO("receivesd START msg from client {0}", robot.m_socketId);
                        GameServer.s_isGameStart = true;
                    }
                    else if (msg.MsgType == Type.RobotsData)
                    {
                        Log.INFO("receivesd ROBOT_DATA msg from client {0}", robot.m_socketId);

                        if (msg.RobotData.Count > 0)
                        {
                            robot.m_mapComponent.m_pos.X = msg.RobotData[0].Position.X;
                            robot.m_mapComponent.m_pos.Y = msg.RobotData[0].Position.Y;
                        }
                        
                    }
                   
                }
            }
        }
        //发送处理完的所有的robot的信息
        public void send_robots_data()
        {
            List<Robot> robots = RobotSystem.get_singleton().get_robots();
            lock (robots)
            {
                foreach (Robot robot in robots)
                {
                    Queue<NetworkMsg> sendQue = robot.m_clientSocket.load_send_queue();
                    while (sendQue.Count > 0)
                    {
                        NetworkMsg msg = sendQue.Dequeue();
                        m_tcpServer.send_networkMessage(msg, robot.m_clientSocket.m_socket);
                    }
                }
            }
            
        }

    }

    
}
