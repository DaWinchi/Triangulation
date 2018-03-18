using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct Piramide {public List<Triangle> base_triangles; public PointF vertex;
                            public float z; public float volume; }
    public class Galerkin
    {
        public List<Piramide> piramides;
        private List<Triangle> triangles;
        private List<PointF> points;
        public Galerkin  ()
        {
            triangles = new List<Triangle>();
            
            points = new List<PointF>();
           

            piramides = new List<Piramide>();
                       
        }

        private float SquareTriangle(Triangle triangle)
        {
            float square = 0;
            square =Math.Abs((triangle.point2.X - triangle.point1.X) *
                             (triangle.point3.Y - triangle.point1.Y) -
                             (triangle.point2.Y - triangle.point1.Y) *
                             (triangle.point3.X - triangle.point1.X));
            return square;
        }


        private float VolumePiramide(Piramide piramide)
        {
            float volume = 0; //объём пирамиды

            foreach(Triangle triangle in piramide.base_triangles)
            {
                volume += 1/3*SquareTriangle(triangle)*piramide.z;
            }

            return 0;
        }

       private void InitializePiramides(List<Triangle> p_triangles, List<PointF> p_points)
        {
            triangles.AddRange(p_triangles);
            points.AddRange(p_points);

            foreach(PointF point in points)
            {
                Piramide piramide = new Piramide();
                piramide.base_triangles = new List<Triangle>();
                piramide.vertex = point;
                piramide.z = 1;
                foreach (Triangle triangle in triangles)
                {
                    if (triangle.point1 == point || triangle.point2 == point || triangle.point3 == point)
                        piramide.base_triangles.Add(triangle);
                }
                piramide.volume = VolumePiramide(piramide);

            }
        }

    }
}
