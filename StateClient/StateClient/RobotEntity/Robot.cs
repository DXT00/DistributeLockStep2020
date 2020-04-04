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
        public MapComponent m_mapComponent;
        public ClientSocket m_tcpSocket;

        public Robot(int id)
        {
            m_id = id;
            m_tcpSocket = new ClientSocket(m_id, Config.serverIp, Config.serverPort);
        }
    }
}
