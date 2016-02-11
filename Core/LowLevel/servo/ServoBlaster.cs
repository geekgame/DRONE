
using System.Runtime.InteropServices;

namespace Drone.Core.LowLevel.servo
{
    internal static class ServoBlaster
    {
        #region Public Methods

        [DllImport("./dll.so")]
        public static extern void setValue(int port, int value);

        #endregion Public Methods
    }
}