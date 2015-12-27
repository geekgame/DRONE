using System.Diagnostics;
using System.IO;

namespace Drone.Core.LowLevel.sensor
{
    class Accelerometer
    {
        public static string[] datas = new string[2];

        public static void get()
        {
            Process a = new Process();
            a.StartInfo.FileName = "gyro2.exe";
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.UseShellExecute = false;

            a.Start();

            StreamReader sr = a.StandardOutput;

            datas = sr.ReadToEnd().Split('|');
        }
    }
}
