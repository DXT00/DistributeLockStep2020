using System;
using System.Collections.Generic;

namespace StateClient
{
    class EntryPoint
    {
        static SandBox application = new SandBox();
        double fps = Config.fps;//60 frame/s

        static void Main(string[] args)
        {
            Log.INFO("Hello World!");
            string input;

            application.init();
            //set time here..
            //Timer timer = new Timer();
            //timer.Enabled = true;
            //timer.Interval = 1.0 / fps * 1000;
            //timer.Start();
            //timer.Elapsed += new ElapsedEventHandler(sandbox_update);
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
        //static void sandbox_update(object obj, ElapsedEventArgs e)
        //{
        //}
    }
}
