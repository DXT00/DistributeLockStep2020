using System;
using System.Collections.Generic;
using System.Collections;
using StateServer.RobotEntity;
using System.Net.Sockets;
namespace StateServer.RobotsSystem
{
    public class RobotSystem
    {
        static private RobotSystem s_singleton = null;
        private static readonly object locker = new object();
        private static readonly object removeRobotLocker = new object();
        private static readonly object addRobotLocker = new object();

        private List<Robot> m_robots;//断开连接的robot不会删除，只会置为null! 通过socketId来取m_robots
        private Dictionary<Socket,int> m_socketToRobotMap;//socket to socketId
        private Random m_randomPos = new Random();
        int m_socketId = 0;
        int m_numrobots = 0;
        private RobotSystem()
        {
            m_robots = new List<Robot>();
            m_socketToRobotMap = new Dictionary<Socket, int>();
        }
        public void generate_robot(Socket clientSocket)
        {
            lock (addRobotLocker)
            {
                Position initPos = generate_init_position();
                Robot robot = new Robot(m_socketId, clientSocket, initPos);
                m_robots.Add(robot);
                m_socketToRobotMap.Add(clientSocket, m_socketId);
                m_socketId++;
                m_numrobots++;
                //返回给client sockectId
                NetworkMsg connectedMsg = new NetworkMsg();
                connectedMsg.MsgType = Type.ConnectedId;
                connectedMsg.SocketId = robot.m_socketId;
                robot.m_clientSocket.dump_send_queue(connectedMsg);
            }
            

        }
        private Position generate_init_position()
        {
            Position pos = new Position();
            pos.X = (uint)m_randomPos.Next(0, Config.areaSize);
            pos.Y = (uint)m_randomPos.Next(0, Config.areaSize);
            pos.Z = 1;
            return pos;
        }
        public List<Robot> get_robots()
        {
            return m_robots;
        }
        public static RobotSystem get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new RobotSystem();
                    }
                }
            }

            return s_singleton;
        }
        public void remove_robot(Socket socket)
        {
            lock (removeRobotLocker)
            {
                if (m_socketToRobotMap.ContainsKey(socket))
                {
                    int socketId = m_socketToRobotMap[socket];
                    m_socketToRobotMap.Remove(socket);
                    m_robots[socketId] = null;
                    m_numrobots--;
                    if (m_numrobots == 0)
                    {
                        //停止游戏
                        GameServer.s_isGameStart = false;
                    }
                }
            }
            
            
        }
        public Dictionary<Socket,int> get_socketToRobotMap()
        {
            return m_socketToRobotMap;
        }
    }
}
