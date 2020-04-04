using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer.RobotComponent
{
    public class MapComponent
    {
        Position m_pos { set; get; }
        int m_areaId { set; get;}
        public MapComponent(Position initPos)
        {
            m_pos = initPos;
        }
    }
}
