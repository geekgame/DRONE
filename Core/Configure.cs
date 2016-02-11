using Drone.Core.Networking;
using Drone.Properties;

namespace Drone.Core
{
    internal class Configure
    {
        #region Public Methods

        public static void Config()
        {
            Sock.Send(Sock.mySock, "DroneAPlat");

            while (!isDroneAPlat)
            {
                ;
            }

            // Drone.Properties.Settings.Default.AcceleroR0 = ServoBlaster.getRoulis();
            // Drone.Properties.Settings.Default.AcceleroT0 = ServoBlaster.getTangage();
            Sock.Send(Sock.mySock, "RoulisDroite");

            while (!isDroneTurned)
            {
                ;
            }

            // Properties.Settings.Default.AcceleroIsTrigoPositive = (ServoBlaster.getRoulis() > Properties.Settings.Default.AcceleroR0) ? false : true;
            Sock.Send(Sock.mySock, "TangageHaut");

            while (!isDroneUp)
            {
                ;
            }

            // Properties.Settings.Default.AcceleroIsDownPositive = (ServoBlaster.getTangage() > Properties.Settings.Default.AcceleroT0) ? false : true;
            Sock.Send(Sock.mySock, "OKACCELERO");

            Settings.Default.isConfigured = true;
            Settings.Default.Save();
        }

        #endregion Public Methods

        #region Public Fields

        public static bool isConfigured = false;

        public static bool isDroneAPlat = false;

        public static bool isDroneTurned = false;

        public static bool isDroneUp = false;

        #endregion Public Fields
    }
}