using System;
using System.Collections.Generic;
using System.Text;
using StateServer.Network;
using StateServer.GameMap;
using StateServer.LogicGame;
namespace StateServer
{
    public sealed class GameServer
    {
        static private GameServer s_singleton = null;
        private static readonly object s_locker = new object();

        static private NetworkManager m_networkManager = NetworkManager.get_singleton();
        static private LogicManager m_logicManager = LogicManager.get_singleton();

        private GameServer()
        {       
           
        }
        public static GameServer get_singleton(){
            if (s_singleton == null)
            {
                lock (s_locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new GameServer();
                    }
                }
            }
            
            return s_singleton;
          }
        public void start()
        {
            m_networkManager.start_tcpServer();
        }
        public void update()
        {
            m_networkManager.receive_data();
            m_logicManager.run();
            m_networkManager.send_data();

        }
    }
}
