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
            list_triangle = new List<Triangle>();
        }

        public List<PointF> points;
        public List<Triangle> list_triangle;
        List<PointF> GlobalPoints;
        List<PointF> FakePoints;

        List<PointF> GlobalRectangle;
        List<PointF> Magnet1;
        List<PointF> Magnet2;
        struct ParametrsEllipse { public double R; public PointF center; }

        public void AddGlobalPoints(PointF p1, PointF p2, PointF p3, PointF p4,
                     List<PointF> globRect,
                     List<PointF> magnet1,
                     List<PointF> magnet2,
                     List<PointF> fake)
        {
            GlobalPoints = new List<PointF> { p1, p2, p3, p4 };
            GlobalRectangle = new List<PointF>(); GlobalRectangle.AddRange(globRect);
            Magnet1 = new List<PointF>(); Magnet1.AddRange(magnet1);
            Magnet2 = new List<PointF>(); Magnet2.AddRange(magnet2);
            FakePoints = new List<PointF>(); FakePoints.AddRange(fake);

            points.AddRange(FakePoints);
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

        public List<Triangle> ReturnAllTriangles(List<PointF> p_points)
        {
            List<Triangle> list_trianglesBuf = new List<Triangle>();
            int count = p_points.Count;
            for (int i = 0; i < count - 2; i++)
            {
                for (int j = i + 1; j < count - 1; j++)
                {

                    for (int k = j + 1; k < count; k++)
                    {
                        Triangle triangle = new Triangle();
                        ParametrsEllipse param = new ParametrsEllipse();
                        triangle.point1 = p_points[i];
                        triangle.point2 = p_points[j];
                        triangle.point3 = p_points[k];
                        param = SearchEllipse(triangle);

                        bool isOk = true;
                        for (int n = 0; n < count; n++)
                        {
                            if ((n == i) || (n == j) || (n == k)) continue;
                            float distance = (float)Math.Sqrt((p_points[n].X - param.center.X) * (p_points[n].X - param.center.X) +
                                (p_points[n].Y - param.center.Y) * (p_points[n].Y - param.center.Y));
                            if (((distance <= param.R)) || (distance > 1e+6) || (param.R > 1e+6)) { isOk = false; break; };

                        }
                        if (isOk) list_trianglesBuf.Add(triangle);
                    }
                }
            }

            //return SortTriangle();
            list_triangle.AddRange(list_trianglesBuf);
            return list_triangle;

        }

        public List<Triangle> AddPoint(PointF point)
        {
            List<PointF> newPoints = new List<PointF>();
            newPoints.Add(point);
            int numDeleting = 0;
            while (numDeleting < list_triangle.Count)
            {
                bool detected = false;
                numDeleting = 0;
                foreach (Triangle triangle in list_triangle)
                {
                    ParametrsEllipse param = new ParametrsEllipse();
                    param = SearchEllipse(triangle);

                    float distance = (float)Math.Sqrt((point.X - param.center.X) * (point.X - param.center.X) +
                                   (point.Y - param.center.Y) * (point.Y - param.center.Y));
                    if (distance < param.R)
                    {
                        bool new1 = false, new2 = false, new3 = false;
                        foreach (PointF bufPoint in newPoints)
                        {
                            if (triangle.point1 != bufPoint) new1 = true;
                            if (triangle.point2 != bufPoint) new2 = true;
                            if (triangle.point3 != bufPoint) new3 = true;
                        }
                        if(new1) newPoints.Add(triangle.point1);
                        if (new2) newPoints.Add(triangle.point2);
                        if (new3) newPoints.Add(triangle.point3);
                        detected = true;
                        break;
                    }
                    numDeleting++;
                }
                if (detected) list_triangle.RemoveAt(numDeleting);
            }
            

            return ReturnAllTriangles(newPoints);
        }

        public List<Triangle> SortTriangle()
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

                for (int i = 0; i < FakePoints.Count; i++)
                {
                    if ((triangle.point1 == FakePoints[i])
                        || (triangle.point2 == FakePoints[i])
                        || (triangle.point3 == FakePoints[i]))
                    {
                        isOk = false; break;
                    }
                }




                if (isOk) buftriangles.Add(triangle);
            }
            list_triangle.Clear();
            list_triangle.AddRange(buftriangles);
            return list_triangle;
        }

    }
}
