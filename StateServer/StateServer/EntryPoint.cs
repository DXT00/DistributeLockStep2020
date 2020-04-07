using System;
using System.Timers;
namespace StateServer
{
    public class EntryPoint
    {
        static GameServer gameServer = GameServer.get_singleton();
        static double fps =Config.fps;//60 frame/s
        ~EntryPoint()
        {
            gameServer.stop();

        }
        static void Main(string[] args)
        {
            Log.INFO ("Hello World!");
            gameServer.start();

            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 1.0 / fps *1000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(server_update);
         
            Console.ReadKey();
        }
        static void server_update(object obj, ElapsedEventArgs e)
        {
            gameServer.update();
        }
    }
}
