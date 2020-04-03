using System;
using System.Diagnostics;

namespace StateClient
{
    public class Log
    {
        static public void ERROR(string s)
        {
            Console.WriteLine("Error occur:" + s);
            Debugger.Break();
        }
        static public void ERROR(string s, params object[] args)
        {
            Console.WriteLine("Error occur:" + s, args);
            Debugger.Break();
        }
        static public void INFO(string s)
        {
            Console.WriteLine("Info:" + s);
        }
        static public void INFO(string s, params object[] args)
        {
            Console.WriteLine("Info:" + s, args);
        }
        static public void ASSERT(string s, bool condition)
        {
            if (!(condition))
            {
                Console.WriteLine("Assert :" + s);
                Debugger.Break();
            }
        }
        static public void ASSERT(string s, bool condition, params object[] args)
        {
            if (!(condition))
            {
                Console.WriteLine("Assert :" + s, args);
                Debugger.Break();
            }
        }
    }
}