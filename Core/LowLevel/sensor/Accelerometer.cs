namespace Drone.Core.LowLevel.sensor
{
    using System.Diagnostics;

    internal class Accelerometer
    {
        public static string[] datas = new string[2];

        public static void get()
        {
            var a = new Process();
            a.StartInfo.FileName = "gyro2.exe";
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.UseShellExecute = false;

            a.Start();

            var sr = a.StandardOutput;

            datas = sr.ReadToEnd().Split('|');
        }
    }
}