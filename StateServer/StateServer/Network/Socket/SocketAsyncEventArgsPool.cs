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
            //  m_pool = new Stack<SocketAsyncEventArgs>(capacity);
            m_pool = new ConcurrentQueue<SocketAsyncEventArgs>();
        }
        public void push(SocketAsyncEventArgs item)
        {
            Log.ASSERT("item is null!", item != null);

           
           m_pool.Enqueue(item);
            //lock (m_pool)
            //{
            //    m_pool.Push(item);

            //}
        }
        public SocketAsyncEventArgs pop()
        {
            //lock (m_pool)
            //{
            //    //Log.ASSERT("m_pool is empty", m_pool.Count > 0);
            //    if(m_pool.Count<=0)
            //        Log.INFO("m_pool is empty");

            //    return m_pool.Pop();
            //}
            SocketAsyncEventArgs args;
            if (m_pool.TryDequeue(out args))
            {
                return args;
            }
            return null;
        }

    }
}
