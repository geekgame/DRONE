using Drone.Core.Networking;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Drone.Core.LowLevel
{
    /// <summary>
    ///     Dll to handle servoblaster dll
    /// </summary>
    internal static class servoblasterDll
    {
        #region Public Methods

        /// <summary>
        ///     Create dll to control servos
        /// </summary>
        public static void CreateDll()
        {
            debut:

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

            content += Environment.NewLine;
            content += "extern int __cdecl ";

            // System.IO.File.Create("./dll.c");
            File.WriteAllText("./dll2.c", content);

            Console.WriteLine("Fichier dll.c créé avec succès.");

            // debug sous windows
            if (Environment.OSVersion.ToString().Contains("Microsoft"))
            {
                // pas de compilation sous Windows
                return;
            }

            // fin debug sous win
            var psi = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "/usr/bin/gcc",
                Arguments = "-shared -o dll.so -fPIC dll2.c"
            };

            // psi.RedirectStandardError = true;
            psi.UseShellExecute = true;
            try
            {
                var p = Process.Start(psi);
                while (!p.HasExited)
                {
                    Thread.Sleep(100);
                }
            }
            catch (FileNotFoundException)
            {
                // send debug packet ERR <file> !line! ErrorMsg
                Sock.Send(Sock.mySock, "ERR <servoblaster.dll> !l75! FileNotFound");
                goto debut;
            }
        }

        #endregion Public Methods
    }
}