using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using StateClient.Network.Messages;
namespace StateClient.RobotComponent
{
    public class NetworkComponent
    {
        public readonly int m_robotId;
        IPEndPoint m_hostIpEndPort;
        public Socket m_socket;
        public SocketAsyncEventArgs m_connectEventArg;
        public SocketAsyncEventArgs m_sendEventArg = null;
        public SocketAsyncEventArgs m_receiveEventArg = null;
        //public Thread m_sendMessageWorker;// = new Thread(new ThreadStart(SendQueueMessage));
        //public Thread m_processReceivedMessageWorker;//= new Thread(new ThreadStart(ProcessReceivedMessage));
        public bool m_isConnected;
        Queue<NetworkMsg> m_sendQueue;
        Queue<NetworkMsg> m_receivedQueue;

        public NetworkComponent(int id,string hostIp,int hostPort)
        {
            m_robotId = id;
            m_hostIpEndPort = new IPEndPoint(IPAddress.Parse(hostIp), hostPort);
            m_connectEventArg = new SocketAsyncEventArgs();
            m_socket = new Socket(m_hostIpEndPort.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_sendQueue = new Queue<NetworkMsg>();
            m_receivedQueue = new Queue<NetworkMsg>();
            m_isConnected = false;
           

        }

        public IPEndPoint get_hostIpEndport()
        {
            return m_hostIpEndPort;
        }

        public void dump_msg(NetworkMsg msg)
        {
            m_sendQueue.Enqueue(msg);
        }
        public Queue<NetworkMsg> get_sendQueue()
        {
            return m_sendQueue;
        }

    }
}
