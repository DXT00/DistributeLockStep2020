using System;
namespace StateServer
{
    public class SandBox
    {
        static GameServer gameServer = GameServer.get_singleton();
        static void Main(string[] args)
        {
            Log.INFO ("Hello World!");
            gameServer.start();
            while (true)
            {
                gameServer.update();
            }
        }
    }
}
