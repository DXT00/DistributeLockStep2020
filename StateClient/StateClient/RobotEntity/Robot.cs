using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotComponent;
using StateClient.Network.Socket;
namespace StateClient.RobotEntity
{
    public class Robot
    {
        public readonly int m_id;
      //  public NetworkComponent m_networkComponent;
        public MapComponent m_mapComponent;
        public TCPSocket m_tcpSocket;

        public Robot(int id)
        {
            m_id = id;
            //m_networkComponent = new NetworkComponent(m_id,Config.serverIp, Config.serverPort);
            m_tcpSocket = new TCPSocket(m_id, Config.serverIp, Config.serverPort);
        }
    }
}
