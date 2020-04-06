using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using StateServer.Network.Protobuf;
using StateServer.RobotEntity;
using StateServer.RobotsSystem;
namespace StateServer.Network.Socket
{
    /*
     
    code from MicroSoft.Net: 
    https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socketasynceventargs?view=netframework-4.8#definition     
         
    */
    public sealed class AsyncUserToken
    {
        public AsyncUserToken(System.Net.Sockets.Socket socket)
        {
            this.Socket = socket;
        }
        public AsyncUserToken()
        {
            this.Socket = null;
        }
        public System.Net.Sockets.Socket Socket { get; set; }

    }
    public class ServerSocket
    {
        private static Mutex m_mutex = new Mutex();
        
        RobotSystem m_robotSystem =RobotSystem.get_singleton();

        int m_numConnectionsMax;
        int m_receiveBufSize;
        int m_numConnectedSocket;
        const int opsToPreAlloc = 2;    // read, write (don't alloc buffer space for accepts)
        int m_totalBytesRead;        // counter of the total # bytes received by the server

        SocketAsyncEventArgsPool m_receiveEventArgsPool;        // pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        SocketAsyncEventArgsPool m_sendEventArgsPool;        // pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations

        BufferManager m_bufferManager;//TODO::client过多时，内存预先分配会有问题

        System.Net.Sockets.Socket m_listenSocket;

        Semaphore m_numAcceptedClientsMax;
        private AutoResetEvent m_waitSendEvent;//等待发送锁

        public ServerSocket(int numConnectionsMax, int receiveBufSize)
        {
            m_numConnectionsMax = numConnectionsMax;
            m_receiveBufSize = receiveBufSize;
            m_numConnectedSocket = 0;
            m_totalBytesRead = 0;
            m_receiveEventArgsPool = new SocketAsyncEventArgsPool(numConnectionsMax);
            m_sendEventArgsPool = new SocketAsyncEventArgsPool(numConnectionsMax);
            m_bufferManager = new BufferManager(m_numConnectionsMax * m_receiveBufSize * opsToPreAlloc, m_receiveBufSize);
            m_numAcceptedClientsMax = new Semaphore(m_numConnectionsMax, m_numConnectionsMax);
            m_waitSendEvent = new AutoResetEvent(false);
            m_bufferManager.InitBuffer();
            for (int i = 0; i < m_numConnectionsMax; i++)
            {
                SocketAsyncEventArgs asyncEventArgs = new SocketAsyncEventArgs();
                asyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(io_completed);//Represents the method that will handle an event when the event provides data.
                asyncEventArgs.UserToken = new AsyncUserToken();
                m_bufferManager.SetBuffer(asyncEventArgs);
                m_sendEventArgsPool.push(asyncEventArgs);
            }
            for (int i = 0; i < m_numConnectionsMax; i++)
            {
                SocketAsyncEventArgs asyncEventArgs = new SocketAsyncEventArgs();
                asyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(io_completed);
                asyncEventArgs.UserToken = new AsyncUserToken();
                m_bufferManager.SetBuffer(asyncEventArgs);
                m_receiveEventArgsPool.push(asyncEventArgs);
            }


        }

        // This method is called whenever a receive or send operation is completed on a socket
        public void io_completed(Object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    process_receive(e);
                    break;
                case SocketAsyncOperation.Send:
                    process_send(e);
                    break;
                default:
                    Log.ERROR("The last operation completed on the socket was not a receive or send");
                    break;
            }

        }
        public void start(IPEndPoint localEndPoint)
        {
            m_listenSocket = new System.Net.Sockets.Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_listenSocket.Bind(localEndPoint);
            // start the server with a listen backlog of 100 connections
            m_listenSocket.Listen(100);
            start_accept(null);
            m_mutex.WaitOne();



        }
        public void start_accept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.UserToken = new AsyncUserToken();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArgs_completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }
            m_numAcceptedClientsMax.WaitOne();
           //return false if the I/O operation completed synchronously.
            if (!m_listenSocket.AcceptAsync(acceptEventArg))
            {
                process_accept(acceptEventArg);
            }
        }

        public void send_networkMessage(NetworkMsg msg, System.Net.Sockets.Socket socket)
        {
            SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();//  m_sendEventArgsPool.pop();
            sendEventArgs.Completed += io_completed;
            sendEventArgs.UserToken = new AsyncUserToken();
            //if (sendEventArgs != null)
            {
                sendEventArgs.UserToken = socket;
                byte[] data = PBSerializer.serialize(msg);
                byte[] length = new byte[4];
                length = BitConverter.GetBytes(data.Length);
                int dataLength = BitConverter.ToInt32(length);
                byte[] packedData = new byte[4 + data.Length];
                Buffer.BlockCopy(length, 0, packedData, 0, 4);
                Buffer.BlockCopy(data, 0, packedData, 4, data.Length);
                sendEventArgs.SetBuffer(packedData, 0, packedData.Length);
                socket.SendAsync(sendEventArgs);
            }
            //else
            //{
            //    m_waitSendEvent.WaitOne();
            //    send_networkMessage(msg, socket);
            //}
            
        }
       
        private void acceptEventArgs_completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                process_accept(e);
            }
            catch(Exception exp)
            {
                Log.ERROR("Accept client {0} error, message: {1}", e.AcceptSocket, exp.Message);
            }
        }

        void process_accept(SocketAsyncEventArgs e)
        {
            try
            {           
                SocketAsyncEventArgs readEventArg = m_receiveEventArgsPool.pop();
                Log.ASSERT("There are no more available sockets to allocate.", readEventArg != null);
                Log.ASSERT("Client form {0} has disconnected!", e.AcceptSocket.Connected,e.AcceptSocket.RemoteEndPoint.ToString());

                Interlocked.Increment(ref m_numConnectedSocket);
                ((AsyncUserToken)readEventArg.UserToken).Socket = e.AcceptSocket;

                //产生Robot，并返回socketid 到client
                m_robotSystem.generate_robot(e.AcceptSocket);

                Log.INFO("Client from {0} connection accepted.There are {1} clients connected to the server", e.AcceptSocket.RemoteEndPoint.ToString(), m_numConnectedSocket);            
                if (!e.AcceptSocket.ReceiveAsync(readEventArg))
                {
                    process_receive(readEventArg);
                }
                
            }
            catch (SocketException ex)
            {
               Log.ERROR("Error when processing data received from {0}:\r\n{1}", e.AcceptSocket.RemoteEndPoint, ex.ToString());
            }
            catch (Exception ex)
            {
                Log.ERROR(ex.ToString());
            }
            start_accept(e);        
        }
        private void process_receive(SocketAsyncEventArgs e)
        {
            // check if the remote host closed the connection
            if ( e.SocketError == SocketError.Success)
            {
                if (e.BytesTransferred > 0 )
                {
                    AsyncUserToken token = e.UserToken as AsyncUserToken;
                    System.Net.Sockets.Socket socket = token.Socket;
                   

                    //判断所有需接收的数据是否已经完成
                    if (socket.Available == 0)
                    {
                        byte[] packedData = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, packedData, 0, packedData.Length);

                        //放入对应的robot的queue中
                        int socketId = m_robotSystem.get_socketToRobotMap()[socket];
                        Robot robot = m_robotSystem.get_robots()[socketId];
                    
                        int packedDataOffset = 0;

                        while (packedDataOffset < e.BytesTransferred)
                        {
                            byte[] lengthData = new byte[4];
                            Array.Copy(packedData, packedDataOffset, lengthData, 0, 4);
                            packedDataOffset += 4;
                            int length = BitConverter.ToInt32(lengthData, 0);

                            byte[] msgData = new byte[length];
                            Array.Copy(packedData, packedDataOffset, msgData, 0, msgData.Length);
                            //放入对应的robot的queue中
                            NetworkMsg netMsg = PBSerializer.deserialize<NetworkMsg>(msgData);
                            robot.m_clientSocket.dump_receive_queue(netMsg);
                            packedDataOffset += msgData.Length;
                        }

                        Log.INFO(" receive from client endPort:{0}", socket.RemoteEndPoint.ToString());
                        Interlocked.Add(ref m_totalBytesRead, e.BytesTransferred);
                        Log.INFO("The server has read a total of {0} bytes", m_totalBytesRead);
                    }

                    if (!socket.ReceiveAsync(e))
                    {
                        process_receive(e);
                    }
                }

            }
            else
            {
                close_clientSocket(e);
            }
        }
        private void process_send(SocketAsyncEventArgs e)
        {

            if (e.SocketError == SocketError.Success)
            {
              
                m_sendEventArgsPool.push(e);
                m_waitSendEvent.Set();
            }
            else
            {   AsyncUserToken token = (AsyncUserToken)e.UserToken;
                Log.ERROR("sending msg error! client endPort:{0}", token.Socket.RemoteEndPoint.ToString());
            }

        }
        private void close_clientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            //close the socket associated with the client
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
                //删除socket对应的robot
                m_robotSystem.remove_robot(token.Socket);
                //TODO:删除MapManager对应areaList里的robotId;
            }
            catch (Exception)
            {
                Log.INFO("client process has already closed");
            }
            token.Socket.Close();

            Interlocked.Decrement(ref m_numConnectedSocket);
            m_receiveEventArgsPool.push(e);
            m_numAcceptedClientsMax.Release();
            Log.INFO("A client has been disconnected from the server. There are {0} clients connected to the server", m_numConnectedSocket);
        }

        public void stop()
        {
            try
            {
                m_listenSocket.Close();
            }
            catch { }
            m_mutex.ReleaseMutex();
        }
    }
}
