using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.math.polygons
{
    public class Point
    {
        public double X;
        public double Y;

        public Point(double dx, double dy)
        {
            this.X = dx;
            this.Y = dy;
        }
    }
}
