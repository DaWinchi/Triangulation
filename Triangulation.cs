using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    struct Triangle { public PointF point1; public PointF point2; public PointF point3; }
    class Triangulation
    {

        public Triangulation()
        {
            points = new List<PointF>();
        }

        public List<PointF> points;
        List<Triangle> list_triangle;
        List<PointF> GlobalPoints;
        List<PointF> GlobalRectangle;
        List<PointF> Magnet1;
        List<PointF> Magnet2;
        struct ParametrsEllipse { public double R; public PointF center; }

        public void AddGlobalPoints(PointF p1, PointF p2, PointF p3, PointF p4,
                     List<PointF> globRect,
                     List<PointF> magnet1,
                     List<PointF> magnet2)
        {
            GlobalPoints = new List<PointF> { p1, p2, p3, p4 };
            GlobalRectangle = new List<PointF>(); GlobalRectangle.AddRange(globRect);
            Magnet1 = new List<PointF>(); Magnet1.AddRange(magnet1);
            Magnet2 = new List<PointF>(); Magnet2.AddRange(magnet2);

            points.AddRange(GlobalPoints);
            points.AddRange(GlobalRectangle);
            points.AddRange(Magnet1);
            points.AddRange(Magnet2);
        }

        private ParametrsEllipse SearchEllipse(Triangle triangle)
        {
            ParametrsEllipse param = new ParametrsEllipse();
            PointF point1 = triangle.point1;
            PointF point2 = triangle.point2;
            PointF point3 = triangle.point3;

            param.center.X = (float)(((point2.X * point2.X - point1.X * point1.X + point2.Y * point2.Y - point1.Y * point1.Y) * (point3.Y - point1.Y) -
              (point3.X * point3.X - point1.X * point1.X + point3.Y * point3.Y - point1.Y * point1.Y) * (point2.Y - point1.Y))
              / (2 * ((point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y))));


            param.center.Y = (float)(((point3.X * point3.X - point1.X * point1.X + point3.Y * point3.Y - point1.Y * point1.Y) * (point2.X - point1.X) -
                (point2.X * point2.X - point1.X * point1.X + point2.Y * point2.Y - point1.Y * point1.Y) * (point3.X - point1.X))
                / (2 * ((point2.X - point1.X) * (point3.Y - point1.Y) - (point3.X - point1.X) * (point2.Y - point1.Y))));

            param.R = Math.Sqrt((param.center.X - point1.X) * (param.center.X - point1.X) + (param.center.Y - point1.Y) * (param.center.Y - point1.Y));

            return param;
        }

        public List<Triangle> ReturnTriangles()
        {
            list_triangle = new List<Triangle>();
            int count = points.Count();
            for (int i = 0; i < count - 2; i++)
            {
                for (int j = i + 1; j < count - 1; j++)
                {

                    for (int k = j + 1; k < count; k++)
                    {
                        Triangle triangle = new Triangle();
                        ParametrsEllipse param = new ParametrsEllipse();
                        triangle.point1 = points[i];
                        triangle.point2 = points[j];
                        triangle.point3 = points[k];
                        param = SearchEllipse(triangle);

                        bool isOk = true;
                        for (int n = 0; n < count; n++)
                        {
                            if ((n == i) || (n == j) || (n == k)) continue;
                            float distance = (float)Math.Sqrt((points[n].X - param.center.X) * (points[n].X - param.center.X) +
                                (points[n].Y - param.center.Y) * (points[n].Y - param.center.Y));
                            if (distance < param.R) { isOk = false; break; };

                        }
                        if (isOk) list_triangle.Add(triangle);
                    }
                }
            }
            return SortTriangle();
        }

        List<Triangle> SortTriangle()
        {
            List<Triangle> buftriangles = new List<Triangle>();
            foreach (Triangle triangle in list_triangle)
            {
                bool isOk = true;
                for (int i = 0; i < 4; i++)
                {
                    if ((triangle.point1 == GlobalPoints[i])
                        || (triangle.point2 == GlobalPoints[i])
                        || (triangle.point3 == GlobalPoints[i]))
                    {
                        isOk = false; break;
                    }
                }
                if (isOk) buftriangles.Add(triangle);
            }
            return buftriangles;
        }

    }
}
