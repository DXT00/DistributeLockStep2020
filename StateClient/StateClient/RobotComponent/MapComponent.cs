using System;
using System.Collections.Generic;
using System.Text;

namespace StateClient.RobotComponent
{
    public class MapComponent
    {
        public Position m_pos { get; set; }
        public MapComponent()
        {
            m_pos = new Position();
        }
    }
}
