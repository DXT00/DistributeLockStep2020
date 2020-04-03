using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
namespace StateClient.RobotSystem
{
    public class RobotSystem
    {

        static private RobotSystem s_singleton = null;
        private static readonly object locker = new object();

        int m_numRobotsMax;
        int m_numRobots;
        List<Robot> m_robots;
        private RobotSystem()
        {
            m_numRobotsMax = Config.numRobotsMax;
            m_robots = new List<Robot>();
        }
        public void generate_robots(int robotsNum)
        {
            
            Log.ASSERT("robotsNum:{0} exceed the max robot number: {1}", robotsNum <= m_numRobotsMax, robotsNum, m_numRobotsMax);
            m_numRobots = robotsNum;
            for (int i = 0; i < m_numRobots; i++)
            {
                Robot robot = new Robot(i);
                m_robots.Add(robot);
            }

        }
        public List<Robot> get_robots(){
            return m_robots;    
        }
        public static RobotSystem get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new RobotSystem();
                    }
                }
            }

            return s_singleton;
        }
    }
}
