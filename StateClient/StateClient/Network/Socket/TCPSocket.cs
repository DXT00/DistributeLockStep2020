using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using StateClient.Network.Messages;
using StateClient.RobotComponent;
using System.Text;
using System.Net.Sockets;

namespace StateClient.Network.Socket
{
    public class TCPSocket
    {
        int m_robotId;
        IPEndPoint m_hostIpEndPort;
        public System.Net.Sockets.Socket m_socket;
        public SocketAsyncEventArgs m_connectEventArg;
        public SocketAsyncEventArgs m_sendEventArg;
        public SocketAsyncEventArgs m_receiveEventArg;
        //public Thread m_sendMessageWorker;// = new Thread(new ThreadStart(SendQueueMessage));
        //public Thread m_processReceivedMessageWorker;//= new Thread(new ThreadStart(ProcessReceivedMessage));
        public bool m_isConnected;
        Queue<NetworkMsg> m_sendQueue;
        Queue<NetworkMsg> m_receivedQueue;



        //AutoResetEvent -- Represents a thread synchronization event that, when signaled, resets automatically
        //after releasing a single waiting thread. This class cannot be inherited.
        private  AutoResetEvent m_autoConnectSignal;
        private  AutoResetEvent m_autoSendEvent;
      private AutoResetEvent m_autoReceiveEvent;

        int m_sendBufferSize;
        int m_receiveBufferSize;

        public TCPSocket(int id,string hostIp, int hostPort)
        {
            m_robotId = id;
            m_hostIpEndPort = new IPEndPoint(IPAddress.Parse(hostIp), hostPort);
            m_socket = new System.Net.Sockets.Socket(m_hostIpEndPort.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_sendQueue = new Queue<NetworkMsg>();
            m_receivedQueue = new Queue<NetworkMsg>();
            m_sendBufferSize = Config.sendBufferSize;
            m_receiveBufferSize = Config.receiveBufferSize;

            m_autoConnectSignal = new AutoResetEvent(false);
            m_autoSendEvent = new AutoResetEvent(false);
            m_autoReceiveEvent = new AutoResetEvent(false);

            m_connectEventArg = new SocketAsyncEventArgs();
            m_connectEventArg.UserToken = m_socket;
            m_connectEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_connect);


            m_sendEventArg = new SocketAsyncEventArgs();
            m_sendEventArg.UserToken = m_socket;
            m_sendEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_send);



            m_receiveEventArg = new SocketAsyncEventArgs();
            m_receiveEventArg.UserToken = m_socket;
            m_receiveEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_receive);

            m_isConnected = false;


        }
        public void connect( )
        {
            m_socket.ConnectAsync(m_connectEventArg);//Begins an asynchronous request for a connection to a remote host.
            m_autoConnectSignal.WaitOne();
            Log.ASSERT("connect failed!", m_connectEventArg.SocketError == SocketError.Success);
            m_isConnected = true;
        }
        private void process_connect(object sender,SocketAsyncEventArgs e)
        {
            m_autoConnectSignal.Set();
            Log.INFO("client {0} has connected to server {1}!", m_robotId,m_hostIpEndPort.ToString());
        }
        private void process_send(object sender, SocketAsyncEventArgs e)
        {
            //m_autoSendEvent.Set();
            if (e.SocketError == SocketError.Success)
            {
                Log.INFO(" msg {0} bytes sending done!", e.BytesTransferred);
            }
            else
            {
                Log.ERROR(" msg {0} bytes sending failed!", e.BytesTransferred);

            }

        }
        private void process_receive(object sender, SocketAsyncEventArgs e)
        {
            m_autoReceiveEvent.Set();
            if (e.SocketError == SocketError.Success)
            {
                Log.INFO(" receive {0} bytes  done!", e.BytesTransferred);
            }
            else
            {
                Log.ERROR("receive failed");

            }

        }
       
        public void receive_serverData(NetworkComponent comp)
        {
           // SocketAsyncEventArgs receiveEventArgs = networkComponent.m_readWriteEventArg;
        }
        public void send_queueData()
        {                    
            string msg = $"client {m_robotId} sending msg..";
            byte[] sendBuffer = Encoding.Unicode.GetBytes(msg);
            m_sendEventArg.SetBuffer(sendBuffer, 0, sendBuffer.Length);

            m_socket.SendAsync(m_sendEventArg);
           
            //m_autoSendEvent.WaitOne();
            Log.INFO("send sucess!");
        }

        
        public void close_socket()
        {
            try{
                m_socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception exp)
            {
                Log.ERROR("client {0} close socket error, message: {1}", m_robotId, exp.Message);
            }
            m_socket.Close();
           
            Log.INFO("client {0} has been disconnected from the server", m_robotId);

        }

        
    }
}
