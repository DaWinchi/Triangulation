using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct Piramide { List<Triangle> base_triangles; PointF vertex; float value; }
    public class Galerkin
    {
        public List<Piramide> piramides;
        private List<Triangle> triangles;
        private List<PointF> points;
        public Galerkin  (List<Triangle> p_triangles, List<PointF> p_points)
        {
            triangles = new List<Triangle>();
            triangles.AddRange(p_triangles);

            points = new List<PointF>();
            points.AddRange(p_points);

            piramides = new List<Piramide>();
            
        }


        private 
            
    }
}
