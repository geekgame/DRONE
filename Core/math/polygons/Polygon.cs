//using Drone.Extensions.SharpDX;

using System.Collections.Generic;
using Point = Drone.Core.math.polygons.Point;

namespace Drone.Core.math.Polygons
{
    public class Polygon
    {
        private IList<Point> _points = new List<Point>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public Polygon()
        {
        }

        /// <summary>
        /// Constructor with points arguments
        /// </summary>
        /// <param name="points"></param>
        public Polygon(IList<Point> points)
        {
            _points = points;
        }

        /// <summary>
        /// Add point in the list.
        /// </summary>
        /// <param name="p"></param>
        public void AddPoints(Point p)
        {
            _points.Add(p);
        }

        /// <summary>
        /// Check if a point (p) is inside a polygon
        /// </summary>
        /// <param name="p">The point to check</param>
        /// <returns>True if point is contained.</returns>
        public bool Contains(Point p)
        {
            var result = false;
            var j = _points.Count - 1;
            for (var i = 0; i < _points.Count; i++)
            {
                if (_points[i].Y < p.Y && _points[j].Y >= p.Y || _points[j].Y < p.Y && _points[i].Y >= p.Y)
                {
                    if (_points[i].X + (p.Y - _points[i].Y) / (_points[j].Y - _points[i].Y) * (_points[j].X - _points[i].X) < p.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}