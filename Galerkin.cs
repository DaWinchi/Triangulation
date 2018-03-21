using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct Piramide
    {
        public List<Triangle> base_triangles; public PointF vertex;
        public float z; public float volume;
    }
    public struct Potential { public PointF point; public float value; }
    public class Galerkin
    {
        public List<Piramide> piramides;
        private List<Triangle> triangles;
        private List<PointF> all_points;
        private List<PointF> rect_points;
        private List<PointF> magnet1_points;
        private List<PointF> magnet2_points;
        public List<Potential> potential;
        private List<List<float>> A;

        public Galerkin(List<Triangle> p_triangle,
                           List<PointF> p_allpoints,
                           List<PointF> p_rectpoints,
                           List<PointF> p_magnet1,
                           List<PointF> p_magnet2)
        {
            triangles = new List<Triangle>();
            triangles.AddRange(p_triangle);
            all_points = new List<PointF>();
            all_points.AddRange(p_allpoints);
            rect_points = new List<PointF>();
            rect_points.AddRange(p_rectpoints);
            magnet1_points = new List<PointF>();
            magnet1_points.AddRange(p_magnet1);
            magnet2_points = new List<PointF>();
            magnet2_points.AddRange(p_magnet2);

            piramides = new List<Piramide>();
            potential = new List<Potential>();
            A = new List<List<float>>();
        }

        private float SquareTriangle(Triangle triangle)
        {
            float square = 0;
            square = Math.Abs((triangle.point2.X - triangle.point1.X) *
                             (triangle.point3.Y - triangle.point1.Y) -
                             (triangle.point2.Y - triangle.point1.Y) *
                             (triangle.point3.X - triangle.point1.X));
            return square;
        }

        private float DpDx(Triangle triangle, PointF vertex)
        {
            PointF point1=vertex, point2=vertex, point3=vertex; //просто начальная инициализация
            if (triangle.point1 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point3; }
            if (triangle.point2 == vertex) { point1 = vertex; point2 = triangle.point1; point3 = triangle.point3; }
            if (triangle.point3 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point1; }

            float value = (point2.Y - point1.Y) * (-1) - (point3.Y - point1.Y) * (-1);
            return value;
        }

        private float DpDy(Triangle triangle, PointF vertex)
        {
            PointF point1 = vertex, point2 = vertex, point3 = vertex; //просто начальная инициализация
            if (triangle.point1 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point3; }
            if (triangle.point2 == vertex) { point1 = vertex; point2 = triangle.point1; point3 = triangle.point3; }
            if (triangle.point3 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point1; }

            float value = (point2.X - point1.X) - (point3.X - point1.X);
            return value;
        }

        private float VolumePiramide(Piramide piramide)
        {
            float volume = 0; //объём пирамиды

            foreach (Triangle triangle in piramide.base_triangles)
            {
                volume += 1 / 3 * SquareTriangle(triangle) * piramide.z;
            }

            return 0;
        }

        private void InitializePiramides(List<Triangle> p_triangles, List<PointF> p_points)
        {
            triangles.AddRange(p_triangles);
            all_points.AddRange(p_points);

            foreach (PointF point in all_points)
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

        public void CreateMatrixA()
        {
            for (int i = 0; i < all_points.Count; i++)
            {
                List<float> list_a = new List<float>();
                for (int j = 0; j < all_points.Count; j++)
                {
                    float value = 0;
                    if (i == j)
                    {
                        List<Triangle> tempTringles = new List<Triangle>();
                        foreach (Triangle triangle in triangles)
                        {
                            if (triangle.point1 == all_points[i] ||
                                triangle.point2 == all_points[i] ||
                                triangle.point3 == all_points[i])
                                tempTringles.Add(triangle);
                        }

                        foreach(Triangle triangle in tempTringles)
                        {
                            value += SquareTriangle(triangle) *
                                (DpDx(triangle, all_points[j]) * DpDx(triangle, all_points[j]) +
                                DpDy(triangle, all_points[j]) * DpDy(triangle, all_points[j]));
                        }
                        list_a.Add(value);
                    }
                    else if (i!=j)
                    {
                        List<Triangle> tempTringles = new List<Triangle>();
                        foreach (Triangle triangle in triangles)
                        {
                            if ((triangle.point1 == all_points[i]&&triangle.point2 == all_points[j]) ||
                                (triangle.point1 == all_points[i]&& triangle.point3 == all_points[j]) ||
                                (triangle.point2 == all_points[i]&& triangle.point1 == all_points[j])||
                                (triangle.point2 == all_points[i] && triangle.point3 == all_points[j])||
                                (triangle.point3 == all_points[i] && triangle.point1 == all_points[j]) ||
                                (triangle.point3 == all_points[i] && triangle.point2 == all_points[j]))
                                tempTringles.Add(triangle);
                        }

                        if (tempTringles.Count == 0) { value = 0; list_a.Add(value); }
                        else
                        {
                            foreach(Triangle triangle in tempTringles)
                            {
                                value += SquareTriangle(triangle) *
                                    (DpDx(triangle, all_points[i]) * DpDx(triangle, all_points[j]) +
                                    (DpDy(triangle, all_points[i]) * DpDy(triangle, all_points[j])));

                            }
                            list_a.Add(value);
                        }
                    }
                }
                A.Add(list_a);
            }
        }

    }
}
