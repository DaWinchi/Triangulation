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

        public List<Triangle> ReturnTriangles()
        {
            
            int count = points.Count;
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
                            if (((distance <= param.R)) || (distance > 1e+6) || (param.R > 1e+6)) { isOk = false; break; };

                        }
                        if (isOk) list_triangle.Add(triangle);
                    }
                }
            }

            return SortTriangle();
            //return list_triangle;

        }

        public List<Triangle> AddPoint(PointF point)
        {
            int numDeleting = 0; bool isOk = false;
            //foreach (Triangle triangle in list_triangle)
            //{
            //    float value1 = (triangle.point1.X - point.X) * (triangle.point2.Y - triangle.point1.Y)
            //                    - (triangle.point2.X - triangle.point1.X) * (triangle.point1.Y - point.Y);
            //    float value2 = (triangle.point2.X - point.X) * (triangle.point3.Y - triangle.point2.Y)
            //                 - (triangle.point3.X - triangle.point2.X) * (triangle.point2.Y - point.Y);
            //    float value3 = (triangle.point3.X - point.X) * (triangle.point1.Y - triangle.point3.Y)
            //                 - (triangle.point1.X - triangle.point3.X) * (triangle.point3.Y - point.Y);

            //    if (((value1 >= 0) && (value2 >= 0) && (value3 >= 0)) || ((value1 <= 0) && (value2 <= 0) && (value3 <= 0))) { isOk = true;  break; }
            //    numDeleting++;
            //}


            foreach (Triangle triangle in list_triangle)
            {
                ParametrsEllipse param = new ParametrsEllipse();
                param = SearchEllipse(triangle);

                float distance = (float)Math.Sqrt((point.X - param.center.X) * (point.X - param.center.X) +
                               (point.Y - param.center.Y) * (point.Y - param.center.Y));
                if (distance < param.R) { isOk = false; break; }
                numDeleting++;
            }

            if (isOk)
            {
                Triangle newTriangle1 = new Triangle
                {
                    point1 = list_triangle[numDeleting].point1,
                    point2 = list_triangle[numDeleting].point2,
                    point3 = point
                };

                Triangle newTriangle2 = new Triangle
                {
                    point1 = list_triangle[numDeleting].point1,
                    point2 = point,
                    point3 = list_triangle[numDeleting].point3
                };

                Triangle newTriangle3 = new Triangle
                {
                    point1 = point,
                    point2 = list_triangle[numDeleting].point2,
                    point3 = list_triangle[numDeleting].point3
                };

                list_triangle.RemoveAt(numDeleting);
                list_triangle.Add(newTriangle1); list_triangle.Add(newTriangle2); list_triangle.Add(newTriangle3);

            }
            else
            {
                points.Add(point);
                ReturnTriangles();
            }
            return list_triangle;
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
