using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
using StateClient.RobotsSystem;
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
                robot.m_clientSocket.connect();
            }
        }

        public void receive_server_data(List<Robot> robots)
        {
            //List<Robot> robots = RobotSystem.get_singleton().get_robots();
            lock (robots)
            {
                foreach (Robot robot in robots)
                {
                    Queue<NetworkMsg> receiveQue = robot.m_clientSocket.load_receive_queue();
                    while (receiveQue.Count > 0)
                    {
                        NetworkMsg msg = receiveQue.Dequeue();
                        if (msg.MsgType == Type.ConnectedId)
                        {
                            on_connected_msg(msg, robot);
                        }
                        else if (msg.MsgType == Type.RobotsData)
                        {
                            on_robotData_msg(msg, robot);
                        }

                    }
                }
            }
           
        }

        public void send_robots_data(List<Robot> robots)
        {
            Log.INFO("tcpSocket sending robots msgs...");
            foreach (Robot robot in robots)
            {
                robot.m_clientSocket.send_queue_data();
            }
        }
        //一个robot发送游戏开始消息
        public void send_start_game(Robot robot)
        {
            Log.INFO("sending start msg..");
            NetworkMsg msg = new NetworkMsg();
            msg.MsgType = Type.StartGame;
            robot.m_clientSocket.dump_send_queue(msg);
            robot.m_clientSocket.send_queue_data();
        }


        private void on_connected_msg(NetworkMsg msg, Robot robot)
        {

            robot.m_clientSocket.set_socketId(msg.SocketId);
            Log.INFO("client {0} receivesd CONNECTED msg from server", robot.m_socketId);

        }

        private void on_robotData_msg(NetworkMsg msg, Robot robot)
        {
            Log.INFO("client {0} receivesd ROBOT_DATA msg from server", robot.m_socketId);

            robot.m_robotGame.reset();

            foreach (RobotData robotData in msg.RobotData)
            {
                if (robotData.ClientInfo.RobotSocketId == robot.m_socketId)
                {
                    robot.m_mapComponent.m_pos.X = robotData.Position.X;
                    robot.m_mapComponent.m_pos.Y = robotData.Position.Y;
                }
                else
                {
                    int nearbyRobotId = robotData.ClientInfo.RobotSocketId;
                    VisualRobot visualRobot = new VisualRobot(nearbyRobotId);
                    visualRobot.m_mapComponent.m_pos.X = robotData.Position.X;
                    visualRobot.m_mapComponent.m_pos.Y = robotData.Position.Y;
                    robot.m_robotGame.add_visual_robot(nearbyRobotId, visualRobot);

                    
                }
            }
        }
    }
}
