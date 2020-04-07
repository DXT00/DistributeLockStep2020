using System;
using System.Timers;
namespace StateServer
{
    public class EntryPoint
    {
        static GameServer gameServer = GameServer.get_singleton();
        static double fps =Config.fps;//30 frame/s
        static public Timer timer = new Timer();
        static private bool tick = false;
        ~EntryPoint()
        {
            //gameServer.stop();

        }
        static void Main(string[] args)
        {
            Log.INFO ("Hello World!");
            gameServer.start();

            timer.Enabled = true;
            timer.Interval = 1.0 / fps *1000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(server_update);

            while (gameServer.m_isServerRuning)
            {
                if (tick)
                {
                    tick = false;
                    gameServer.update();

                }
               
            }
            
  

            Console.ReadKey();
        }
        static void server_update(object obj, ElapsedEventArgs e)
        {
                tick = true;
        }
    }
}
