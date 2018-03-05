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
        float step_x = 7F, step_y = 7F, x_now, y_now;

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

            Magnet magnet1 = new Magnet(40, 20, center1, angle1, 10);
            Magnet magnet2 = new Magnet(40, 20, center2, angle2, 10);
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

            //timer1.Start();
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

    
}
