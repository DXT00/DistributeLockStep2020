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
            //set time here..
            application.init();
            while (true)
            {
                application.update();

            }
           
        }
    }
}
