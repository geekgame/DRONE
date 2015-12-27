//INUTILE, UNIQUEMENT CONSERVE POUR HISTORIQUE
namespace Drone.Core.LowLevel
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using Drone.Properties;

    internal class PythonControl
    {
        public static void ExecutePython(string cmd, string args)
        {
            // http://stackoverflow.com/a/27801632
            var start = new ProcessStartInfo
                            {
                                FileName = cmd, 
                                Arguments = args, 
                                UseShellExecute = false, 
                                RedirectStandardOutput = true
                            };
            using (var process = Process.Start(start))
            {
                if (process != null)
                {
                    using (var reader = process.StandardOutput)
                    {
                        var result = reader.ReadToEnd();
                        Console.WriteLine(result);
                        if (string.CompareOrdinal(result, string.Empty) != 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public static bool InitPythonFiles(string pythonExePath = "")
        {
            debut:
            if (string.CompareOrdinal(pythonExePath, string.Empty) != 0 && File.Exists(pythonExePath))
            {
                return true;
            }

            // TODO : télécharger python
            // Sous linux uniquement
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var start = new ProcessStartInfo();
                start.FileName = "apt-get";
                start.Arguments = "install python3";
                start.UseShellExecute = true;
                var process = Process.Start(start);
            }
            else
            {
                Console.WriteLine(Resources.NeedPython3);
                Console.WriteLine(Resources.PleaseInstallAndEnterPath);
                pythonExePath = Console.ReadLine();
                goto debut;
            }

            var servoL = "import sys, os; os.system(\"echo 1=\"+sys.argv[1]);";
            var servoR = "import sys, os; os.system(\"echo 2=\"+sys.argv[1]);";
            var engL = "import sys, os; os.system(\"echo 3=\"+sys.argv[1]);";
            var engR = "import sys, os; os.system(\"echo 4=\"+sys.argv[1]);";

            // Le moteur arrière est géré avec les relais, donc GPIO, donc pas dans ce fichier. Voir Core.LowLevel.GPIO
            return true;
        }
    }
}