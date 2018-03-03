﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parametrs;
using System.Drawing.Drawing2D;

namespace TriangleDeloneWithMagnetic
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            parametrs = new Painter();
            parametrs.xmax = 10;
            parametrs.ymax = 10;
            parametrs.xmin = -10;
            parametrs.ymin = -10;
        }

        Painter parametrs;


    }

    public class Magnet
    {
        public PointF center;
        public float width, height;
        public float angle;
        public float step;

        List<PointF> points;

        public Magnet(float p_width, float p_height, PointF p_center, float p_angle, float p_step)
        {
            points = new List<PointF>();
            width = p_width;
            height = p_height;
            center = p_center;
            angle = p_angle;
            step = p_step;
        }

        private void CreateDiscret()
        {
            PointF A=new PointF(center.X-width/2, center.Y-height/2);
            PointF B=new PointF(center.X+width/2, center.Y-height/2);
            PointF C=new PointF(center.X+width/2, center.Y+height/2);
            PointF D=new PointF(center.X-width/2, center.Y+height/2);
          
            for (float p=A.X; p<=B.X; p+=step)
            {
                PointF bufPoint = new PointF(p, A.Y);
                points.Add(bufPoint);
            }
            for (float p = B.Y; p <= C.Y; p += step)
            {
                PointF bufPoint = new PointF(B.X, p);
                points.Add(bufPoint);
            }

            for (float p = C.X; p >= D.X; p -= step)
            {
                PointF bufPoint = new PointF(p, C.Y);
                points.Add(bufPoint);
            }

            for (float p = D.Y; p >= A.Y; p -= step)
            {
                PointF bufPoint = new PointF(A.X, p);
                points.Add(bufPoint);
            }
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

        public List <PointF> ReturnRectangleDdiscret()
        {
            CreateDiscret();
            RotateCoordinate();
            return points;
        }

      
    }
}