using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotComponent;
namespace StateClient.RobotEntity
{
    //游戏中模拟的除了自身Client的其他可见范围内的Robot
    public class VisualRobot
    {
        public readonly int m_socketId;

        public MapComponent m_mapComponent;
        public VisualRobot(int socketId)
        {
            m_mapComponent = new MapComponent();
            m_socketId = socketId;
        }
    }
}
