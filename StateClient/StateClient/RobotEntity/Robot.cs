using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotComponent;
using StateClient.Network.Socket;
namespace StateClient.RobotEntity
{
    public class Robot
    {
        public readonly int m_id;//id in Game
        //public readonly int m_socketId = 0;//需要从server获取，初始化为0
        public MapComponent m_mapComponent;
        public ClientSocket m_clientSocket;
        public RobotGameComponent m_robotGame;//robot本身和其区域范围内的robots模拟的Game

        public Robot(int id)
        {
            m_id = id;
            m_clientSocket = new ClientSocket( Config.serverIp, Config.serverPort);
            m_mapComponent = new MapComponent();
            m_robotGame = new RobotGameComponent();
        }
        
    }
}
