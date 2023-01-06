using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Tools
{
    static public class Logger
    {
        public static bool debug { private get; set; } = true;
        public static bool log { private get; set; } = false;

       
        
        public static void infoMSG(string msg)
        {
            if (debug)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("INFO");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] ");
                Console.Write(msg + "\n");
            }
            
        }
        public static void successMSG(string msg)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("OKAY");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.Write(msg + "\n");
        }
        public static void warningMSG(string msg)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARN");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.Write(msg + "\n");
        }
        public static void errorMSG(string msg)
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ERRO");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.Write(msg + "\n");
        }
    }
}
