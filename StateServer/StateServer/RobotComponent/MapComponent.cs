using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer.RobotComponent
{
    public class MapComponent
    {
        public Position m_pos { get; }
        public int m_areaId { set; get;}
        public MapComponent(Position initPos)
        {
            m_pos = initPos;
            m_areaId = 0;//初始化为0
        }
    }
}
