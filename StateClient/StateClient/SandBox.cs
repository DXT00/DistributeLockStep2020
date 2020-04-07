using System;
using System.Collections.Generic;
using StateClient.Network;
using StateClient.RobotEntity;
using StateClient.RobotsSystem;
using StateClient.LogicGame;
using System.Timers;

namespace StateClient
{
    public class SandBox
    {
        static RobotSystem s_robotSystem = RobotSystem.get_singleton();
        static NetworkManager s_networkManager = NetworkManager.get_singleton();
        static LogicManager s_logicGameManager = LogicManager.get_singleton();
        List<Robot> m_robots;
        private bool m_tick = false;

        public SandBox()
        {
            //Robot 输入操作定时器，robot每隔timer.Interval产生一次操作
            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 1.0 / Config.fps * 1000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(sandbox_update);

            s_robotSystem.generate_robots(Config.numRobotsCreate);
            m_robots = s_robotSystem.get_robots();
        }

        public void init()
        {
            s_networkManager.connect(m_robots);
        }
        public void update()
        {
            //更新每个Robot数据
            s_networkManager.receive_server_data(m_robots);
            if (m_tick)
            {
                m_tick = false;
                s_logicGameManager.update(m_robots);
            }
            //发送每个Robot新的操作数据
            s_networkManager.send_robots_data(m_robots);       

        }

        public void start_game()
        {
            Log.ASSERT("there is no robot in Game", m_robots.Count > 0);
            s_networkManager.send_start_game(m_robots[0]);
            //m_isGameStart = true;
        }

        void sandbox_update(object obj, ElapsedEventArgs e)
        {
            m_tick = true;
        }
    }
}
