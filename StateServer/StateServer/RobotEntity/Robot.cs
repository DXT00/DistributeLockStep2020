using System;
using System.Collections.Generic;
using StateServer.RobotComponent;
using System.Net.Sockets;
namespace StateServer.RobotEntity
{
    public class Robot
    {
        public readonly int m_socketId;
        public MapComponent m_mapComponent;
        public ClientSocket m_clientSocket;

        public Robot(int socketId, Socket socket,Position initPos)
        {
            m_socketId = socketId;
            m_clientSocket = new ClientSocket(socketId, socket);
            m_mapComponent = new MapComponent(initPos);
        }
    }
}
