using System;
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
            parametrs = new Painter
            {
                xmax = 100,
                ymax = 100,
                xmin = -100,
                ymin = -100
            };


        }

        Painter parametrs;
        Bitmap bmp;
        Graphics graph;
        List<PointF> points;
        Triangulation triangulation;

        Magnet GlobalRect;
        float step_x = 30F, step_y = 30F, x_now, y_now;

        List<Triangle> list_triangles = new List<Triangle>();

        private void Painting()
        {
            int width = GraphicsBox.Width, height = GraphicsBox.Height;

            bmp = new Bitmap(width, height);
            graph = Graphics.FromImage(bmp);


            SolidBrush brushBkgrd = new SolidBrush(Color.Black);
            SolidBrush brushPoint = new SolidBrush(Color.Red);
            graph.FillRectangle(brushBkgrd, 0, 0, width, height);


            Pen trianglePen = new Pen(Color.Yellow, 1);



            for (int i = 0; i < list_triangles.Count; i++)
            {
                graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point1.X), (float)parametrs.Y(height, list_triangles[i].point1.Y),
                                            (float)parametrs.X(width, list_triangles[i].point2.X), (float)parametrs.Y(height, list_triangles[i].point2.Y));
                graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point2.X), (float)parametrs.Y(height, list_triangles[i].point2.Y),
                                            (float)parametrs.X(width, list_triangles[i].point3.X), (float)parametrs.Y(height, list_triangles[i].point3.Y));
                graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point1.X), (float)parametrs.Y(height, list_triangles[i].point1.Y),
                                            (float)parametrs.X(width, list_triangles[i].point3.X), (float)parametrs.Y(height, list_triangles[i].point3.Y));

            }

            GraphicsBox.Image = bmp;
        }

        private void DrawBtn_Click(object sender, EventArgs e)
        {
            triangulation = new Triangulation();
            list_triangles.Clear();
            PointF center1 = new PointF(0, -50);
            float angle1 = (float)(2 * Math.PI / 360 * 45);
            PointF center2 = new PointF(0, 50);
            float angle2 = (float)(2 * Math.PI / 360 * 0);
            PointF centerGlobal = new PointF(0, 0);

            Magnet magnet1 = new Magnet(20, 20, center1, angle1, 20);
            Magnet magnet2 = new Magnet(20, 20, center2, angle2, 20);
            GlobalRect = new Magnet(160, 160, centerGlobal, (float)(2 * Math.PI / 360 * 0), 20);
            List<PointF> fake = new List<PointF>();
            fake.AddRange(magnet1.FakePoints());
            fake.AddRange(magnet2.FakePoints());
            PointF point1 = new PointF(300, 0); PointF point2 = new PointF(-300, 0);
            PointF point3 = new PointF(0, 300); PointF point4 = new PointF(0, -300);

            triangulation.AddGlobalPoints(point1, point2, point3, point4,
                                           GlobalRect.ReturnRectangleDdiscret(),
                                           magnet1.ReturnRectangleDdiscret(),
                                           magnet2.ReturnRectangleDdiscret(),
                                           fake);
            list_triangles.AddRange(triangulation.ReturnTriangles());

            x_now = GlobalRect.A.X + step_x;
            y_now = GlobalRect.A.Y + step_y;

            timer1.Start();
            Painting();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (x_now > GlobalRect.C.X)
            {
                y_now += step_y;
                if (y_now > GlobalRect.C.Y) { timer1.Stop(); list_triangles.Clear(); list_triangles.AddRange(triangulation.SortTriangle()); Painting(); }
                x_now = GlobalRect.A.X;
            }
            PointF point = new PointF(x_now, y_now);
            triangulation.AddPoint(point);
            list_triangles.Clear();
            list_triangles.AddRange(triangulation.list_triangle);
            x_now += step_x;
            Painting();

        }
    }

    public class Magnet
    {
        public PointF center;
        public float width, height;
        public float angle;
        public float step;
        public PointF A, B, C, D;

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
            points.Clear();

            A = new PointF(center.X - width / 2, center.Y - height / 2);
            B = new PointF(center.X + width / 2, center.Y - height / 2);
            C = new PointF(center.X + width / 2, center.Y + height / 2);
            D = new PointF(center.X - width / 2, center.Y + height / 2);

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

        public List<PointF> FakePoints()
        {

            Magnet magnet = new Magnet(width - width / 5, height - height / 5, center, angle, step / 3);

            List<PointF> bufPoints = new List<PointF>();
            bufPoints = magnet.ReturnRectangleDdiscret();

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
        }

        public List<PointF> ReturnRectangleDdiscret()
        {
            CreateDiscret();
            RotateCoordinate();
            return points;
        }


    }
}
