using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
namespace StateClient.RobotComponent
{
    public class RobotGameComponent
    {
        //socketId->Robot
        public Dictionary<int,VisualRobot> m_nearbyAreaRobots;
        public RobotGameComponent()
        {
            m_nearbyAreaRobots = new Dictionary<int, VisualRobot>();
        }
        //public void remove_robot(int socketId)
        //{
        //    if (m_nearbyAreaRobots.ContainsKey(socketId))
        //    {
        //        m_nearbyAreaRobots.Remove(socketId);
        //    }
        //}
        public void add_visual_robot(int socketId, VisualRobot robot)
        {
            m_nearbyAreaRobots.Add(socketId, robot);
        }
        public void update_robot_position(int socketId,Position pos)
        {

        }
        public void reset()
        {
            m_nearbyAreaRobots.Clear();
        }
        public Dictionary<int, VisualRobot> get_nearby_area_robots()
        {
            return m_nearbyAreaRobots;
        }
    }
}
