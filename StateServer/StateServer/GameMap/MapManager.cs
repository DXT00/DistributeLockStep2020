using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer.GameMap
{
    public sealed class MapManager
    {
        static private MapManager s_singleton = null;
        private static readonly object locker = new object();

        private int m_areaAcount;
        private int m_singleAreaSize;
        private MapManager()
        {
            m_areaAcount = Config.areaCount;
            m_singleAreaSize = Config.areaSize/Config.areaCount;

        }

        public static MapManager get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new MapManager();
                    }
                }
            }

            return s_singleton;
        }

        public void judge_areaId(Position position)
        {

        }
    }
}
