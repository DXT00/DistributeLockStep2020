using StateServer.RobotEntity;
using StateServer.RobotsSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateServer.GameMap
{
    public sealed class MapManager
    {
        static private MapManager s_singleton = null;
        private static readonly object locker = new object();


        private uint m_areaAcount;
        private uint m_singleAreaSize;
        private List<Area> m_areasMap;
        private MapManager()
        {
            m_areaAcount = Config.areaCount;
            m_singleAreaSize = Config.areaSize / Config.areaCount;
            m_areasMap = new List<Area>();
            for (int i = 0; i < m_areaAcount * m_areaAcount; i++)
            {
                m_areasMap.Add(new Area(i));
            }

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
        public void update()
        {
            List<Robot> robots = RobotSystem.get_singleton().get_robots();
            foreach (Robot robot in robots)
            {
                int areaIdOld = robot.m_mapComponent.m_areaId;
                int areaIdNew = calculate_robot_area(robot);
                robot.m_mapComponent.m_areaId = areaIdNew;
                update_map(robot, areaIdOld, areaIdNew);
            }
            foreach (Robot robot in robots)
            {
                load_nearby_area_msg(robot,robots);
            }

        }
        private int calculate_robot_area(Robot robot)
        {
            Position pos = robot.m_mapComponent.m_pos;
            int areaId = calculate_area(pos);
            robot.m_mapComponent.m_areaId = areaId;
            return areaId;
        }
        private void update_map(Robot robot, int areaIdOld, int areaIdNew)
        {
            if (areaIdOld != areaIdNew)
            {
                m_areasMap[areaIdOld].remove_robot(robot.m_socketId);
                m_areasMap[areaIdNew].add_robot(robot.m_socketId);
            }
            else
            {
                if (m_areasMap[areaIdNew].is_robotsIdList_empty())
                    m_areasMap[areaIdNew].add_robot(robot.m_socketId);
            }
        }
        private void load_nearby_area_msg(Robot robot, List<Robot> robots)
        {       
            int areaId = robot.m_mapComponent.m_areaId;
            int[,] offset = new int[,]
            {
                {-1,-1},
                {0 ,-1},
                {1 ,-1},
                {-1,0 },
                {1 ,0 },
                {-1,1 },
                {0 ,1 },
                {1 ,1 }
            };
            int y = areaId / (int)m_areaAcount;
            int x = areaId % (int)m_areaAcount;
            //发送robot周围区域的所有robots的位置信息
            NetworkMsg robotDataMsg = new NetworkMsg();
            robotDataMsg.MsgType = Type.RobotsData;
            for (int i = 0; i < 8; i++)
            {
                int nearbyX = x + offset[i,0];
                int nearbyY = y + offset[i,1];

                if (nearbyX < 0 || nearbyX >= m_areaAcount || nearbyY < 0 || nearbyY >= m_areaAcount)
                    continue;

                int nearbyAreaId = nearbyX + (int)m_areaAcount * nearbyY;
                Area nearbyArea = m_areasMap[nearbyAreaId];
                List<int> nearbyRobotsList = nearbyArea.get_robotsIdList();
                for(int k= 0;k< nearbyRobotsList.Count;k++)
                {
                    int nearbyRobotId = nearbyRobotsList[k];

                    RobotData nearbyRobotData = new RobotData();
                    nearbyRobotData.ClientInfo = new ClientInfo();
                    nearbyRobotData.Position = new Position();
                    nearbyRobotData.ClientInfo.RobotSocketId = nearbyRobotId;
                    nearbyRobotData.Position = robots[nearbyRobotId].m_mapComponent.m_pos;
                    robotDataMsg.RobotData.Add(nearbyRobotData);
                    // robot.m_clientSocket.dump_send_queue(nearbyRobotMsg);
                }
                
            }
            //发送robot自己的位置信息
            RobotData robotData = new RobotData();
            robotData.ClientInfo = new ClientInfo();
            robotData.Position = new Position();
            robotData.ClientInfo.RobotSocketId = robot.m_socketId;
            robotData.Position = robot.m_mapComponent.m_pos;
            robotDataMsg.RobotData.Add(robotData);
            robot.m_clientSocket.dump_send_queue(robotDataMsg);

        }
        private int calculate_area(Position pos)
        {
            pos.X = Math.Clamp(pos.X, 0, Config.areaSize - 1);
            pos.Y = Math.Clamp(pos.Y, 0, Config.areaSize - 1);
            uint x = pos.X / m_singleAreaSize;
            uint y = pos.Y / m_singleAreaSize;
            uint areaId = x + m_areaAcount * y;
            return (int)areaId;
        }
    }
}
