using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace Drone.Core.LowLevel.servo
{
    static class ServoBlaster
    {
        [DllImport("./dll.so")]
        public static extern void setValue(int port, int value);
    }
}
