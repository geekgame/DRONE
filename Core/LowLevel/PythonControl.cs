
//INUTILE, UNIQUEMENT CONSERVE POUR HISTORIQUE

using System;

using System.Diagnostics;
using System.IO;
using Drone.Properties;

namespace Drone.Core.LowLevel
{
    class PythonControl
    {
        public static bool InitPythonFiles(string pythonExePath = "")
        {
            debut:
            if (String.CompareOrdinal(pythonExePath, "") != 0 && File.Exists(pythonExePath)) return true;
            //TODO : télécharger python
            //Sous linux uniquement
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "apt-get";
                start.Arguments = "install python3";
                start.UseShellExecute = true;
                Process process = Process.Start(start);
            }
            else
            {
                Console.WriteLine(Resources.NeedPython3);
                Console.WriteLine(Resources.PleaseInstallAndEnterPath);
                pythonExePath = Console.ReadLine();
                goto debut;
            }

            string servoL = "import sys, os; os.system(\"echo 1=\"+sys.argv[1]);";
            string servoR = "import sys, os; os.system(\"echo 2=\"+sys.argv[1]);";
            string engL = "import sys, os; os.system(\"echo 3=\"+sys.argv[1]);";
            string engR = "import sys, os; os.system(\"echo 4=\"+sys.argv[1]);";
            //Le moteur arrière est géré avec les relais, donc GPIO, donc pas dans ce fichier. Voir Core.LowLevel.GPIO
            return true;
        }

        public static void ExecutePython(string cmd, string args)
        {
            //http://stackoverflow.com/a/27801632
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            using(Process process = Process.Start(start))
            {
                if (process != null)
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        Console.WriteLine(result);
                        if (String.CompareOrdinal(result, "") != 0)
                            return;
                    }
            } 
        }
    }
}
