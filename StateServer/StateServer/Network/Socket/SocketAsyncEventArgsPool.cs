using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
namespace StateServer.Network.Socket
{

    // Represents a collection of reusable SocketAsyncEventArgs objects.  
    public class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> m_pool;
        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }
        public void push(SocketAsyncEventArgs item)
        {
            Log.ASSERT("item is null!", item != null);
            lock (m_pool)
            {
                m_pool.Push(item);

            }
        }
        public SocketAsyncEventArgs pop()
        {
            lock (m_pool)
            {
                Log.ASSERT("m_pool is empty", m_pool.Count > 0);
                return m_pool.Pop();
            }
        }

    }
}
