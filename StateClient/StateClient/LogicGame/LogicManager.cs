using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
namespace StateClient.LogicGame
{
    public sealed class LogicManager
    {
        enum GameOperate
        {
            FRONT,
            BACK,
            LEFT,
            RIGHT
        }

        static private LogicManager s_singleton = null;
        private static readonly object locker = new object();
        private Random m_randomOperate = new Random();
        private uint m_movingSpeed = Config.movingSpeed;
        private LogicManager()
        {


        }

        public static LogicManager get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new LogicManager();
                    }
                }
            }

            return s_singleton;
        }

        public void update(List<Robot> robots)
        {
            foreach(Robot robot in robots)
            {
                if (robot.m_clientSocket.m_isConnected)
                    play_game(robot);
            }
        }

        //运行游戏逻辑
        private void play_game(Robot robot)
        {
            Dictionary<int, VisualRobot> visualRobots;

            Position pos = robot.m_mapComponent.m_pos;
            int operate = m_randomOperate.Next(0, 4);
            if (operate == (int)GameOperate.FRONT)
            {
                pos.Y+= m_movingSpeed;
            }
            else if (operate == (int)GameOperate.BACK)
            {
                pos.Y-= m_movingSpeed;
            }
            else if(operate == (int)GameOperate.LEFT)
            {
                pos.X-=m_movingSpeed;
            }
            else
            {
                pos.X+=m_movingSpeed;
            }
            //robot.m_mapComponent.m_pos = pos;

            //发送位置消息给server
            NetworkMsg msg = new NetworkMsg();
            msg.MsgType = Type.RobotsData;
            msg.SocketId = robot.m_clientSocket.m_socketId;
            RobotData robotData = new RobotData();
            robotData.Position = pos;
            robotData.ClientInfo = new ClientInfo();
            robotData.ClientInfo.RobotSocketId = robot.m_clientSocket.m_socketId;

            robot.m_clientSocket.dump_send_queue(msg);
        }

        
    }
}
