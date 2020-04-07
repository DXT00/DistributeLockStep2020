using System;
using System.Collections.Generic;

namespace StateClient
{
    class EntryPoint
    {
        static SandBox application = new SandBox();
        double fps = Config.fps;//30 frame/s

        static void Main(string[] args)
        {
            Log.INFO("Hello World!");
            string input;

            application.init();
           
            Console.Write(">");
            input = Console.ReadLine();

            switch (input)
            {
                case "start":
                    application.start_game();
                    break;

            }
            while (true)
            {
                application.update();

            }
        }
       
    }
}
