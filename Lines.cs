using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct LevelLines { public PointF point1; public PointF pooint2; }
    public class Lines
    {
        private List<TrianglePotential> triangles;
        public List<LevelLines> lines;

        public Lines(List<TrianglePotential> p_triangles)
        {
            triangles = new List<TrianglePotential>();
            triangles.Clear();
        }
    }
}
