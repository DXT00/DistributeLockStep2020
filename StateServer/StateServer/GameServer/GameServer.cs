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

        static private NetworkManager s_networkManager = NetworkManager.get_singleton();
        static private LogicManager s_logicManager = LogicManager.get_singleton();
        static public bool s_isGameStart = false;
        private GameServer()
        {

        }
        public static GameServer get_singleton()
        {
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
            s_networkManager.start_tcpServer();
        }
        public void update()
        {
            //reveive接收游戏开始消息
            s_networkManager.receive_robots_data();
            if (s_isGameStart)
            {
                s_logicManager.run();
                //要等所有clients都连接完才开始发送
                s_networkManager.send_robots_data();
            }  
        }
    
    
        public void stop()
        {
            s_networkManager.stop_tcpServer();
        }
    }
}
