using System;
using System.Collections.Generic;
using System.Text;
using StateClient.RobotEntity;
namespace StateClient.LogicGame
{
    public sealed class LogicManager
    {
        static private LogicManager s_singleton = null;
        private static readonly object locker = new object();
        private LogicManager()
        {


        }

        public static LogicManager get_singleton()
        {
            if (s_singleton == null)
            {
                lock (locker)
                {
                    if (s_singleton == null)
                    {
                        s_singleton = new LogicManager();
                    }
                }
            }

            return s_singleton;
        }

        public void update(List<Robot> robots)
        {

        }

    }
}
