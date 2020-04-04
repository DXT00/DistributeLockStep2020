using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using StateClient.Network.Protobuf;
using StateClient.RobotComponent;
using System.Text;
using System.Net.Sockets;
using StateClient.Network.Protobuf;
namespace StateClient.Network.Socket
{
    public sealed class AsyncUserToken
    {
        public AsyncUserToken(System.Net.Sockets.Socket socket)
        {
            this.Socket = socket;
        }
        public AsyncUserToken()
        {

        }
        public System.Net.Sockets.Socket Socket { get; set; }
      
    }
    public class ClientSocket
    {
        int m_robotId;//id in Game
        int m_socketId;// socket id to server
        IPEndPoint m_hostIpEndPort;
        public System.Net.Sockets.Socket m_socket;
        public SocketAsyncEventArgs m_connectEventArg;
        public SocketAsyncEventArgs m_sendEventArg;
        public SocketAsyncEventArgs m_receiveEventArg;
       
        public bool m_isConnected;
        Queue<NetworkMsg> m_sendQueue;
        Queue<NetworkMsg> m_receivedQueue;
        

        //AutoResetEvent -- Represents a thread synchronization event that, when signaled, resets automatically
        //after releasing a single waiting thread. This class cannot be inherited.
        private AutoResetEvent m_autoConnectSignal;
        private AutoResetEvent m_autoSendEvent;
        private AutoResetEvent m_autoReceiveEvent;

        int m_sendBufferSize;
        int m_receiveBufferSize;


        public ClientSocket(int id, string hostIp, int hostPort)
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
            m_connectEventArg.UserToken =new AsyncUserToken(m_socket);
            m_connectEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_connect);

            m_sendEventArg = new SocketAsyncEventArgs();
            m_sendEventArg.UserToken = new AsyncUserToken(m_socket);
            m_sendEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_send);

            m_receiveEventArg = new SocketAsyncEventArgs();
            m_receiveEventArg.UserToken = new AsyncUserToken(m_socket);
            m_receiveEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(process_receive);

            m_isConnected = false;

        }
        public void connect()
        {
            m_socket.ConnectAsync(m_connectEventArg);//Begins an asynchronous request for a connection to a remote host.
            m_autoConnectSignal.WaitOne();
            Log.ASSERT("connect failed!", m_connectEventArg.SocketError == SocketError.Success);
            m_isConnected = true;
        }
        private void process_connect(object sender, SocketAsyncEventArgs e)
        {
            m_autoConnectSignal.Set();
            Log.INFO("robotId {0} has connected to server {1}!", m_robotId, m_hostIpEndPort.ToString());
        }
        private void process_send(object sender, SocketAsyncEventArgs e)
        {
            //m_autoSendEvent.Set();
            if (e.SocketError == SocketError.Success)
                Log.INFO(" msg {0} bytes sending done!", e.BytesTransferred);

            else
                Log.ERROR(" msg {0} bytes sending failed!", e.BytesTransferred);
        }
        private void process_receive(object sender, SocketAsyncEventArgs e)
        {
            m_autoReceiveEvent.Set();
            if (e.SocketError == SocketError.Success)
                Log.INFO(" receive {0} bytes  done!", e.BytesTransferred);
            else
                Log.ERROR("receive failed");
        }

        public void receive_server_data(NetworkComponent comp)
        {
            // SocketAsyncEventArgs receiveEventArgs = networkComponent.m_readWriteEventArg;
        }

        public void send_queue_data()
        {
            string msg = $"client {m_robotId} sending msg..";
            NetworkMsg netMsg = new NetworkMsg();
            Position pos = new Position();
            pos.X = 1;
            pos.Y = 2;
            pos.Z = 3;
            netMsg.Position = pos;
            netMsg.ClientInfo = new ClientInfo();
            netMsg.ClientInfo.RobotId = m_robotId;
           

            byte[] sendBuffer = PBSerializer.serialize(netMsg);
            m_sendEventArg.SetBuffer(sendBuffer, 0, sendBuffer.Length);
            m_socket.SendAsync(m_sendEventArg);


            //m_autoSendEvent.WaitOne();
            //if (!)//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
            //{
            //    // 同步发送时处理发送完成事件
            //    process_send(m_socket, m_sendEventArg);
            //}

            Log.INFO("client {0} send sucess!", m_robotId);
        }

        public void close_socket()
        {
            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception exp)
            {
                Log.ERROR("client {0} close socket error, message: {1}", m_robotId, exp.Message);
            }
            m_socket.Close();
            Log.INFO("client {0} has been disconnected from the server", m_robotId);

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
