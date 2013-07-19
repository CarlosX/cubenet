using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeNet.Definitions
{
    class Bootlogo
    {
        public static void _Load()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Title = "CubeNet " + Global.Versions.appVersion;
            Console.WriteLine("=============================CubeNet=============================");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
