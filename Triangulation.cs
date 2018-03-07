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
        List<PointF> MagnetPoints;

        List<PointF> GlobalRectangle;
        Magnet Magnet1;
        Magnet Magnet2;
        struct ParametrsEllipse { public double R; public PointF center; }

        public void AddGlobalPoints(PointF p1, PointF p2, PointF p3, PointF p4,
                     List<PointF> globRect,
                     Magnet magnet1,
                     Magnet magnet2)
        {
            GlobalPoints = new List<PointF> { p1, p2, p3, p4 };
            GlobalRectangle = new List<PointF>(); GlobalRectangle.AddRange(globRect);
            Magnet1 = magnet1;
            Magnet2 = magnet2;
            FakePoints = new List<PointF>(); FakePoints.AddRange(Magnet1.FakePoints()); FakePoints.AddRange(Magnet2.FakePoints());

            points.AddRange(FakePoints);
            points.AddRange(GlobalPoints);
            points.AddRange(GlobalRectangle);
            points.AddRange(Magnet1.ReturnRectangleDdiscret());
            points.AddRange(Magnet2.ReturnRectangleDdiscret());


            MagnetPoints = new List<PointF>();
            MagnetPoints.AddRange(Magnet1.ReturnRectangleDdiscret());
            MagnetPoints.AddRange(Magnet2.ReturnRectangleDdiscret());
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


            return list_trianglesBuf;
        }

        public List<Triangle> UpdateListTriangle(List<Triangle> p_list)
        {
            list_triangle.AddRange(p_list);
            return list_triangle;
        }

        public bool IsInMagnets(PointF point)
        {
            Triangle magnet11 = new Triangle
            {
                point1 = Magnet1.A,
                point2 = Magnet1.B,
                point3 = Magnet1.C
            };

            Triangle magnet12 = new Triangle
            {
                point1 = Magnet1.A,
                point2 = Magnet1.D,
                point3 = Magnet1.C
            };
            Triangle magnet21 = new Triangle
            {
                point1 = Magnet2.A,
                point2 = Magnet2.B,
                point3 = Magnet2.C
            };
            Triangle magnet22 = new Triangle
            {
                point1 = Magnet2.A,
                point2 = Magnet2.D,
                point3 = Magnet2.C
            };

            List<Triangle> bufList = new List<Triangle> { magnet11, magnet12, magnet21, magnet22 };
            bool pointInMagnets = false;

            foreach (Triangle triangle in bufList)
            {
                float value1 = (triangle.point1.X - point.X) * (triangle.point2.Y - triangle.point1.Y)
                               - (triangle.point2.X - triangle.point1.X) * (triangle.point1.Y - point.Y);
                float value2 = (triangle.point2.X - point.X) * (triangle.point3.Y - triangle.point2.Y)
                             - (triangle.point3.X - triangle.point2.X) * (triangle.point2.Y - point.Y);
                float value3 = (triangle.point3.X - point.X) * (triangle.point1.Y - triangle.point3.Y)
                             - (triangle.point1.X - triangle.point3.X) * (triangle.point3.Y - point.Y);

                if (((value1 >= 0) && (value2 >= 0) && (value3 >= 0)) || ((value1 <= 0) && (value2 <= 0) && (value3 <= 0))) { pointInMagnets = true; break; }
            }

            return pointInMagnets;
        }

        public List<Triangle> AddPoint(PointF point)
        {

            if (!IsInMagnets(point))
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
                        bool IsInMagnet = false;
                        foreach (PointF bufPoint in FakePoints)
                        {
                            if ((triangle.point1 == bufPoint)
                                   || (triangle.point2 == bufPoint)
                                   || (triangle.point3 == bufPoint)) { IsInMagnet = true; break; }
                        }
                        if (!IsInMagnet)
                        {
                            ParametrsEllipse param = new ParametrsEllipse();
                            param = SearchEllipse(triangle);

                            float distance = (float)Math.Sqrt((point.X - param.center.X) * (point.X - param.center.X) +
                                           (point.Y - param.center.Y) * (point.Y - param.center.Y));
                            if (distance < param.R)
                            {
                                bool new1 = true, new2 = true, new3 = true;
                                foreach (PointF bufPoint in newPoints)
                                {
                                    if (triangle.point1 == bufPoint) new1 = false;
                                    if (triangle.point2 == bufPoint) new2 = false;
                                    if (triangle.point3 == bufPoint) new3 = false;
                                }


                                if (new1) newPoints.Add(triangle.point1);
                                if (new2) newPoints.Add(triangle.point2);
                                if (new3) newPoints.Add(triangle.point3);
                                detected = true;

                                break;
                            }
                        }
                        numDeleting++;
                    }
                    if (detected) list_triangle.RemoveAt(numDeleting);

                }

                List<Triangle> buffer_list2 = new List<Triangle>();
                buffer_list2.AddRange(ReturnAllTriangles(newPoints));

                numDeleting = 0;
                while (numDeleting < buffer_list2.Count)
                {
                    bool detected = false;
                    numDeleting = 0;
                    foreach (Triangle triangle in buffer_list2)
                    {
                        if (!((triangle.point1 == point) || (triangle.point1 == point) || (triangle.point1 == point)))
                        {
                            detected = true;
                            break;
                        }
                        numDeleting++;
                    }
                    if (detected) buffer_list2.RemoveAt(numDeleting);

                }
                return UpdateListTriangle(buffer_list2);
            }
            else return list_triangle;
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
