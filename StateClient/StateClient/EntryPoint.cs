using System;
using System.Collections.Generic;
using System.Text;

namespace StateClient
{
    class EntryPoint
    {
        static void Main(string[] args)
        {   
            Log.INFO("Hello World!");
            SandBox application = new SandBox();
            string input;

            //set time here..
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
