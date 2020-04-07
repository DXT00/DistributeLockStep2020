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
            this.Buffer = new List<byte>();
        }
        public AsyncUserToken()
        {
            this.Buffer = new List<byte>();
        }
        /// 通信SOKET  
        public System.Net.Sockets.Socket Socket { get; set; }
        /// 数据缓存区
        public List<byte> Buffer { get; set; }

    }
    public class ClientSocket
    {      
        static private readonly object s_locker = new object();
        public int m_socketId;// socket id to server
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
        //  private AutoResetEvent m_autoReceiveEvent;

        int m_sendBufferSize;
        int m_receiveBufferSize;


        public ClientSocket( string hostIp, int hostPort)
        {
           // m_robotId = id;
            m_socketId = -1;//初始化为-1，connect再时从server获取
            m_hostIpEndPort = new IPEndPoint(IPAddress.Parse(hostIp), hostPort);
            m_socket = new System.Net.Sockets.Socket(m_hostIpEndPort.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_sendQueue = new Queue<NetworkMsg>();
            m_receivedQueue = new Queue<NetworkMsg>();
            m_sendBufferSize = Config.sendBufferSize;
            m_receiveBufferSize = Config.receiveBufferSize;

            m_autoConnectSignal = new AutoResetEvent(false);
            m_autoSendEvent = new AutoResetEvent(false);
            // m_autoReceiveEvent = new AutoResetEvent(false);

            m_connectEventArg = new SocketAsyncEventArgs();
            m_connectEventArg.UserToken = new AsyncUserToken(m_socket);
            m_connectEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_connectEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(handler_connect);

            m_sendEventArg = new SocketAsyncEventArgs();
            m_sendEventArg.UserToken = new AsyncUserToken(m_socket);
            m_sendEventArg.RemoteEndPoint = m_hostIpEndPort;
            m_sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(handler_send);

            m_receiveEventArg = new SocketAsyncEventArgs();
            m_receiveEventArg.UserToken = new AsyncUserToken(m_socket);
            m_receiveEventArg.RemoteEndPoint = m_hostIpEndPort;
            byte[] receiveBuffer = new byte[m_receiveBufferSize];
            m_receiveEventArg.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
            m_receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(handler_receive);

            m_isConnected = false;

        }
        //从server获取socketId
        public void set_socketId(int socketId)
        {
            m_socketId = socketId;
        }
        public void connect()
        {
            m_socket.ConnectAsync(m_connectEventArg);//Begins an asynchronous request for a connection to a remote host.
            m_autoConnectSignal.WaitOne();
            Log.ASSERT("connect failed!", m_connectEventArg.SocketError == SocketError.Success);
            if (!m_socket.ReceiveAsync(m_receiveEventArg))
            {
                process_receive(m_receiveEventArg);
            }
        }
        private void handler_connect(object sender, SocketAsyncEventArgs e)
        {
            m_autoConnectSignal.Set();
           // Log.INFO("robotId {0} has connected to server {1}!", m_robotId, m_hostIpEndPort.ToString());
        }
        private void handler_send(object sender, SocketAsyncEventArgs e)
        {
            //m_autoSendEvent.Set();
            if (e.SocketError == SocketError.Success)
                Log.INFO(" msg {0} bytes sending done!", e.BytesTransferred);

            else
                Log.ERROR(" msg {0} bytes sending failed!", e.BytesTransferred);
        }
        private void handler_receive(object sender, SocketAsyncEventArgs e)
        {
            process_receive(e);
        }
        private void process_receive(SocketAsyncEventArgs e)
        {
            lock (s_locker)
            {
                if (e.SocketError == SocketError.Success && e.BytesTransferred>0)
                {
                    AsyncUserToken token = e.UserToken as AsyncUserToken;

                
                    System.Net.Sockets.Socket socket = token.Socket;


                    //判断所有需接收的数据是否已经完成
                    //   if (socket.Available == 0)
                    //  {

                    byte[] data = new byte[e.BytesTransferred];
                    Array.Copy(e.Buffer, e.Offset, data, 0, data.Length);
                    lock (token.Buffer)
                    {
                        token.Buffer.AddRange(data);
                    }


                    do// (packedDataOffset < e.BytesTransferred)
                    {
                        byte[] lengthByte = token.Buffer.GetRange(0, 4).ToArray();

                        int length = BitConverter.ToInt32(lengthByte, 0);

                        if (length > token.Buffer.Count - 4)
                            break;

                        byte[] msgData = token.Buffer.GetRange(4, length).ToArray();
                        lock (token.Buffer)
                        {
                            token.Buffer.RemoveRange(0, length + 4);
                        }
                        //放入对应的robot的queue中
                        NetworkMsg netMsg = PBSerializer.deserialize<NetworkMsg>(msgData);
                        dump_receive_queue(netMsg);
                    } while (token.Buffer.Count > 4);

                    //  }
                    //继续接收
                    if (!socket.ReceiveAsync(e))
                        process_receive(e);
                        

                   // }

                }
                else
                {
                    close_socket();
                    Log.INFO("The connection to the server is broken!");
                }
            }
        }



        public void send_queue_data()
        {
            while (m_sendQueue.Count > 0)
            {
                NetworkMsg msg = m_sendQueue.Dequeue();
                byte[] data = PBSerializer.serialize(msg);
                byte[] length = new byte[4];
                length = BitConverter.GetBytes(data.Length);

                byte[] packedData = new byte[4 + data.Length];
                Buffer.BlockCopy(length, 0, packedData, 0, 4);
                Buffer.BlockCopy(data, 0, packedData, 4, data.Length);
                m_sendEventArg.SetBuffer(packedData, 0, packedData.Length);
                m_socket.SendAsync(m_sendEventArg);
                Log.INFO("client socketId {0} send sucess!", m_socketId);

            }

            //bool willRaiseEvent = m_socket.ReceiveAsync(m_receiveEventArg);
            //if (!willRaiseEvent)
            //{
            //    process_receive(m_receiveEventArg);
            //}

            //m_autoSendEvent.WaitOne();
            //if (!)//投递发送请求，这个函数有可能同步发送出去，这时返回false，并且不会引发SocketAsyncEventArgs.Completed事件
            //{
            //    // 同步发送时处理发送完成事件
            //    process_send(m_socket, m_sendEventArg);
            //}

        }

        public void close_socket()
        {
            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception exp)
            {
                Log.ERROR("client socketId {0} close socket error, message: {1}", m_socketId, exp.Message);
            }
            m_socket.Close();
            Log.INFO("client socketId {0} has been disconnected from the server", m_socketId);

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
