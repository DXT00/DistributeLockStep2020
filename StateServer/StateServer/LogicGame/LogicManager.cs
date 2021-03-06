﻿using System;
using System.Collections.Generic;
using System.Text;
using StateServer.GameMap;
namespace StateServer.LogicGame
{
    public sealed class LogicManager
    {
        static private LogicManager s_singleton = null;
        private static readonly object locker = new object();
        private static MapManager s_mapManager = MapManager.get_singleton();

        private int m_frameNum = 0;
        private LogicManager()
        {
          

        }

        public static LogicManager get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new LogicManager();
                    }
                }
            }

            return s_singleton;
        }
        
        public void run()
        {
            s_mapManager.update();
            s_mapManager.show_map_areas_info();
            m_frameNum++;

           
        }
        public int get_frameNum()
        {
            return m_frameNum;
        }
    }
}
