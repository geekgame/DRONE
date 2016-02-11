using System;

namespace Drone.Core.utils
{
    internal class Console2
    {
        #region Public Methods

        public static void WriteLine(string a, ConsoleColor b)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = b;
            Console.WriteLine(a);
            Console.ForegroundColor = old;
        }

        #endregion Public Methods
    }
}