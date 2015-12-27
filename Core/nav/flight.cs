using System;
using Drone.Core.LowLevel.sensor;
using Drone.Properties;

namespace Drone.Core.nav
{
    class flight
    {
        public static bool SwitchMode(enums.mode m)
        {
            if(Program.curMode == m)
            {
                Console.WriteLine("Le drone est déjà dans le mode désiré");
                return true;
            }
            switch(m)
            {
                case enums.mode.modeHorizontal:
                    Program.curMode = enums.mode.modeVtoH;
                    return true;
                    break;
                case enums.mode.modeVertical:
                    Program.curMode = enums.mode.modeHtoV;
                    return true;
                    break;
                default:
                    //Ne rien faire
                    return true;
                    break;
            }
        }

        public static void Balance()
        {
            //doit être appelé en tant que thread différent.
            while(true)
            {
                if (Program.BalanceDrone)
                {
                    Accelerometer.get();

                    double x = double.Parse(Accelerometer.datas[0]);
                    double y = double.Parse(Accelerometer.datas[1]);

                    double R0 = Settings.Default.AcceleroR0;
                    int MR0 = Settings.Default.MotorR0;
                    int ML0 = Settings.Default.MotorL0;

                    //ServoBlaster.setValue(2, (int)Math.Round(ML0 + Math.Pow(10 * (x - R0), 2)));
                    //ServoBlaster.setValue(4, (int)Math.Round(ML0 - Math.Pow(10 * (x - R0), 2)));
                    Console.WriteLine(@"DROITE : " + (int) Math.Round(ML0 + Math.Pow(5*(x - R0), 1)));

                }
            }
        }
    }
}
