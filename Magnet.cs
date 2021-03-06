﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public class Magnet
    {
        public PointF center;
        public float width, height;
        public float angle;
        public float step;
        public PointF A, B, C, D;

        public List<PointF> points;
        public List<Potential> potential;
        public Magnet(float p_width, float p_height, PointF p_center, float p_angle, float p_step)
        {
            points = new List<PointF>();
            potential = new List<Potential>();
            width = p_width;
            height = p_height;
            center = p_center;
            angle = p_angle;
            step = p_step;
        }

        private void CreateDiscret()
        {
            points.Clear();
            Random rand = new Random();
            A = new PointF(center.X - width / 2 + (float)rand.NextDouble() / 10, center.Y - height / 2 + (float)rand.NextDouble() / 10);
            B = new PointF(center.X + width / 2 + (float)rand.NextDouble() / 10, center.Y - height / 2 + (float)rand.NextDouble() / 10);
            C = new PointF(center.X + width / 2 + (float)rand.NextDouble() / 10, center.Y + height / 2 + (float)rand.NextDouble() / 10);
            D = new PointF(center.X - width / 2 + (float)rand.NextDouble() / 10, center.Y + height / 2 + (float)rand.NextDouble() / 10);

            for (float p = A.X; p < B.X; p += step)
            {
                PointF bufPoint = new PointF(p, A.Y);
                points.Add(bufPoint);
            }
            for (float p = B.Y; p < C.Y; p += step)
            {
                PointF bufPoint = new PointF(B.X, p);
                points.Add(bufPoint);
            }

            for (float p = C.X; p > D.X; p -= step)
            {
                PointF bufPoint = new PointF(p, C.Y);
                points.Add(bufPoint);
            }

            for (float p = D.Y; p > A.Y; p -= step)
            {
                PointF bufPoint = new PointF(A.X, p);
                points.Add(bufPoint);
            }
        }
        public List<Potential> ReturnPotential(float minU, float maxU)
        {
            potential.Clear();
            CreateDiscret();
            List<PointF> coordinates = new List<PointF>();
            int size_points = points.Count;
            for(int i=0; i<size_points; i++)
            {
                if(points[i].X<center.X)
                {
                    PointF newPoint = new PointF(
                    (float)(points[i].X * Math.Cos(angle) - points[i].Y * Math.Sin(angle)),
                    (float)(points[i].X * Math.Sin(angle) + points[i].Y * Math.Cos(angle))
                    );
                    Potential pot = new Potential { point = newPoint, value = minU };
                    potential.Add(pot);
                }
                else
                {
                    PointF newPoint = new PointF(
                    (float)(points[i].X * Math.Cos(angle) - points[i].Y * Math.Sin(angle)),
                    (float)(points[i].X * Math.Sin(angle) + points[i].Y * Math.Cos(angle))
                    );
                    Potential pot = new Potential { point = newPoint, value = maxU };
                    potential.Add(pot);
                }
            }
            points.Clear();

            int size_potential = potential.Count;
            for(int i=0; i<size_potential;i++)
            {
                points.Add(potential[i].point);
            }

            PointF newPoint1 = new PointF((float)(A.X * Math.Cos(angle) - A.Y * Math.Sin(angle)),
                                       (float)(A.X * Math.Sin(angle) + A.Y * Math.Cos(angle)));
            A = newPoint1;

            PointF newPoint2 = new PointF((float)(B.X * Math.Cos(angle) - B.Y * Math.Sin(angle)),
                                        (float)(B.X * Math.Sin(angle) + B.Y * Math.Cos(angle)));
            B = newPoint2;
            PointF newPoint3 = new PointF((float)(C.X * Math.Cos(angle) - C.Y * Math.Sin(angle)),
                                        (float)(C.X * Math.Sin(angle) + C.Y * Math.Cos(angle)));
            C = newPoint3;
            PointF newPoint4 = new PointF((float)(D.X * Math.Cos(angle) - D.Y * Math.Sin(angle)),
                                        (float)(D.X * Math.Sin(angle) + D.Y * Math.Cos(angle)));
            D = newPoint4;

            return potential;
        }
        public List< PointF> FakePoints()
        {

            Magnet magnet = new Magnet(width - width / 5, height - height / 5, center, angle, step);

            List<PointF> bufPoints = new List<PointF>();
            magnet.ReturnPotential(0,0);
            bufPoints = magnet.points;
           

            return bufPoints;
        }

        private void RotateCoordinate()
        {
            int size = points.Count;

            for (int i = 0; i < size; i++)
            {
                PointF newPoint = new PointF(
                    (float)(points[i].X * Math.Cos(angle) - points[i].Y * Math.Sin(angle)),
                    (float)(points[i].X * Math.Sin(angle) + points[i].Y * Math.Cos(angle))
                    );
                points[i] = newPoint;
            }


           
        }

       

    }
}
