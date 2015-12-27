namespace Drone.Core.LowLevel.servo
{
    using System.Runtime.InteropServices;

    internal static class ServoBlaster
    {
        [DllImport("./dll.so")]
        public static extern void setValue(int port, int value);
    }
}