using System;
using System.Collections.Generic;
using StateClient.Network;
using StateClient.RobotEntity;
using StateClient.LogicGame;
namespace StateClient
{
    public class SandBox
    {
        static StateClient.RobotSystem.RobotSystem s_robotSystem = StateClient.RobotSystem.RobotSystem.get_singleton();
        static NetworkManager s_networkManager = NetworkManager.get_singleton();
        static LogicManager s_logicGameManager = LogicManager.get_singleton();
        List<Robot> m_robots;
        string input;
        public SandBox()
        {
            s_robotSystem.generate_robots(Config.numRobotsCreate);
            m_robots = s_robotSystem.get_robots();
        }

        public void init()
        {
            s_networkManager.connect(m_robots);
        }
        public void update()
        {

            Console.Write(">");
            input = Console.ReadLine();
            switch (input)
            {
                case "s":
                    s_networkManager.send_robotsData(m_robots);
                    break;
               


            }
            //s_networkManager.receive_serverData();
            //s_logicGameManager.update(m_robots);


        }



    }
}
