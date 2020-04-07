using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer.GameMap
{
    public class Area
    {
        List<int> m_robotsIdList;
        int m_areaId;
        public Area(int areaId)
        {
            m_areaId = areaId;
            m_robotsIdList = new List<int>();
        }
        public void add_robot(int robot_socketId)
        {
            m_robotsIdList.Add(robot_socketId);
        }
        public void remove_robot(int robot_socketId)
        {
            if (m_robotsIdList.Contains(robot_socketId))
            {
                m_robotsIdList.Remove(robot_socketId);
            }
        }
        public bool is_robotsIdList_empty()
        {
            return m_robotsIdList.Count == 0;
        }
        public List<int> get_robotsIdList()
        {
            return m_robotsIdList;
        }
        public int get_areaId()
        {
            return m_areaId;
        }
    }
}
