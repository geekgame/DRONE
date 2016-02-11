// --------------------------------------------------------------------------------------------------------------------
// <copyright file="flight.cs" company="geekgame">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Drone.Core.LowLevel.sensor;
using Drone.Properties;
using System;

namespace Drone.Core.nav
{
    internal class Flight
    {
        #region Public Methods

        /// <summary>
        ///     Balancing the drone TODO
        /// </summary>
        public static void Balance()
        {
            // doit être appelé en tant que thread différent.
            while (true)
            {
                if (Program.BalanceDrone)
                {
                    Accelerometer.get();

                    var x = double.Parse(Accelerometer.datas[0]);
                    var y = double.Parse(Accelerometer.datas[1]);

                    double R0 = Settings.Default.AcceleroR0;
                    var MR0 = Settings.Default.MotorR0;
                    var ML0 = Settings.Default.MotorL0;

                    // ServoBlaster.setValue(2, (int)Math.Round(ML0 + Math.Pow(10 * (x - R0), 2)));
                    // ServoBlaster.setValue(4, (int)Math.Round(ML0 - Math.Pow(10 * (x - R0), 2)));
                    Console.WriteLine(@"DROITE : " + (int) Math.Round(ML0 + Math.Pow(5*(x - R0), 1)));
                }
            }
        }

        public static bool SwitchMode(enums.mode m)
        {
            if (Program.CurMode == m)
            {
                Console.WriteLine(@"Le drone est déjà dans le mode désiré");
                return true;
            }

            switch (m)
            {
                case enums.mode.modeHorizontal:
                    Program.CurMode = enums.mode.modeVtoH;
                    return true;

                case enums.mode.modeVertical:
                    Program.CurMode = enums.mode.modeHtoV;
                    return true;

                default:

                    // Ne rien faire
                    return true;
            }
        }

        #endregion Public Methods
    }
}