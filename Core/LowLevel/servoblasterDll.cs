using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Drone.Core.LowLevel
{
    using System.Diagnostics.CodeAnalysis;

    internal class servoblasterDll
    {
        /// <summary>
        /// Create dll to control servos
        /// </summary>
        public static void CreateDll()
        {

            #region fonction controle turbine
            var content = "#include <stdio.h>" + Environment.NewLine;
            content += "#include <stdlib.h>" + Environment.NewLine;
            content += "#include <math.h>" + Environment.NewLine;
            content += "#include <signal.h>" + Environment.NewLine;
            content += "#include <fcntl.h>" + Environment.NewLine;
            content += "#include <string.h>" + Environment.NewLine;
            content += "#include <time.h>" + Environment.NewLine;
            content += "#include \"sensor.c\"" + Environment.NewLine;
            content += Environment.NewLine;
            content += "extern int __cdecl setValue(int port, int value)" + Environment.NewLine;
            content += "{" + Environment.NewLine;
            content += "char command[50] = \"\";" + Environment.NewLine;
            content += "sprintf(\"echo %d=%d > /dev/servoblaster\", port, value);" + Environment.NewLine;
            content += "}" + Environment.NewLine;
            #endregion

            #region fonction lire accelerometre
            content += Environment.NewLine;
            content += "extern int __cdecl ";

            #endregion


            // System.IO.File.Create("./dll.c");
            File.WriteAllText("./dll2.c", content);

            Console.WriteLine("Fichier dll.c créé avec succès.");

            //DEBUG SOUS WINDOWS :
            if (Environment.OSVersion.ToString().Contains("Microsoft"))
            {
                goto suite;
            }
            //FIN DEBUG SOUS WINDOWS

            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "/usr/bin/gcc",
                Arguments = "-shared -o dll.so -fPIC dll2.c"
            };

            //psi.RedirectStandardError = true;
            psi.UseShellExecute = true;
            var p = Process.Start(psi);

            while (!p.HasExited)
            {
                Thread.Sleep(100);
            }

            suite:
            Console.WriteLine("lel");
        }
    }
}