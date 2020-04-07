using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace StateServer.Network.Socket
{

    // Represents a collection of reusable SocketAsyncEventArgs objects.  
    public class SocketAsyncEventArgsPool
    {
       // Stack<SocketAsyncEventArgs> m_pool;
        ConcurrentQueue<SocketAsyncEventArgs> m_pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new ConcurrentQueue<SocketAsyncEventArgs>();
        }
        public void push(SocketAsyncEventArgs item)
        {
            Log.ASSERT("item is null!", item != null);

           
           m_pool.Enqueue(item);
           
        }
        public SocketAsyncEventArgs pop()
        {
            
            SocketAsyncEventArgs args;
            if (m_pool.TryDequeue(out args))
            {
                return args;
            }
            return null;
        }

    }
}
