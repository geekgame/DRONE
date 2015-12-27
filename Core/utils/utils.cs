using System;

namespace Drone.Core.utils
{
    class Console2
    {
        public static void WriteLine(string a, ConsoleColor b)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = b;
            Console.WriteLine(a);
            Console.ForegroundColor = old;
        }
    }
}
