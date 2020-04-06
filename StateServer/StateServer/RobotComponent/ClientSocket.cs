using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
namespace StateServer.RobotComponent
{
    public class ClientSocket
    {
        public Socket m_socket { private set; get; }
        private int m_socketId;
        private EndPoint m_remoteEndPort;
        private Queue<NetworkMsg> m_sendQueue;
        private Queue<NetworkMsg> m_receivedQueue;
        public ClientSocket(int socketId,Socket socket)
        {
            m_socketId = socketId;
            m_socket = socket;
            m_remoteEndPort = socket.RemoteEndPoint;
            m_sendQueue = new Queue<NetworkMsg>();
            m_receivedQueue = new Queue<NetworkMsg>();
        }
        public void dump_receive_queue(NetworkMsg msg)
        {
            m_receivedQueue.Enqueue(msg);
        }
        public void dump_send_queue(NetworkMsg msg)
        {
            m_sendQueue.Enqueue(msg);
        }

        public Queue<NetworkMsg> load_receive_queue()
        {
            return m_receivedQueue;
        }
        public Queue<NetworkMsg> load_send_queue()
        {
            return m_sendQueue;
        }
    }
}
