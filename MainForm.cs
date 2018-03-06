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
            points = new List<PointF>();
            magnet1 = new Magnet(40, 20, center1, (float)(2 * Math.PI / 360 * 45), step_x);
            magnet2 = new Magnet(40, 20, center2, (float)(2 * Math.PI / 360 * 30), step_x);
            GlobalRect = new Magnet(160, 160, centerGlobal, (float)(2 * Math.PI / 360 * 0), 20);
            points.AddRange(magnet1.ReturnRectangleDdiscret()); points.AddRange(magnet2.ReturnRectangleDdiscret());
            points.AddRange(GlobalRect.ReturnRectangleDdiscret());
            ScrollHeight.Minimum = (int)GlobalRect.A.Y;
            ScrollHeight.Value = (int)magnet1.center.Y;
            ScrollHeight.Maximum = (int)GlobalRect.D.Y;

            ScrollWidth.Minimum = (int)GlobalRect.A.X;
            ScrollWidth.Value = (int)magnet1.center.X;
            ScrollWidth.Maximum = (int)GlobalRect.B.X;

            ScrollAngle.Minimum = 0;
            ScrollAngle.Maximum = 360;
            ScrollAngle.Value=45;

            StepXBox.Text = "7";
            StepYBox.Text = "7";

            Width1Box.Text = magnet1.width.ToString();
            Width2Box.Text = magnet2.width.ToString();
            Height1Box.Text = magnet1.height.ToString();
            Height2Box.Text = magnet2.height.ToString();
            Painting();
        }

        Painter parametrs;
        Bitmap bmp;
        Graphics graph;
        List<PointF> points;
        Triangulation triangulation;
        Magnet magnet1; PointF center1 = new PointF(0, -50);
        Magnet magnet2; PointF center2 = new PointF(0, 50);
        Magnet GlobalRect; PointF centerGlobal = new PointF(0, 0);

        float step_x = 7F, step_y = 7F, x_now, y_now;

        List<Triangle> list_triangles = new List<Triangle>();

        private void ScrollHeight_Scroll(object sender, EventArgs e)
        {
            int y = ScrollHeight.Value;
            if (RadioMagnet1.Checked)
            {
                magnet1.center.Y = y;
            }

            if (RadioMagnet2.Checked)
            {
                magnet2.center.Y = y;
            }
            points.Clear();
            points.AddRange(magnet1.ReturnRectangleDdiscret()); points.AddRange(magnet2.ReturnRectangleDdiscret());
            points.AddRange(GlobalRect.ReturnRectangleDdiscret());
            Painting();

        }

        private void ScrollWidth_Scroll(object sender, EventArgs e)
        {
            int x = ScrollWidth.Value;
            if (RadioMagnet1.Checked)
            {
                magnet1.center.X = x;
            }

            if (RadioMagnet2.Checked)
            {
                magnet2.center.X = x;
            }
            points.Clear();
            points.AddRange(magnet1.ReturnRectangleDdiscret()); points.AddRange(magnet2.ReturnRectangleDdiscret());
            points.AddRange(GlobalRect.ReturnRectangleDdiscret());
            Painting();
        }

        private void ScrollAngle_Scroll(object sender, EventArgs e)
        {
            float angle = (float)(2 * Math.PI / 360 *ScrollAngle.Value);
            if (RadioMagnet1.Checked)
            {
                magnet1.angle = angle;
            }

            if (RadioMagnet2.Checked)
            {
                magnet2.angle=angle;
            }
            points.Clear();
            points.AddRange(magnet1.ReturnRectangleDdiscret()); points.AddRange(magnet2.ReturnRectangleDdiscret());
            points.AddRange(GlobalRect.ReturnRectangleDdiscret());
            Painting();
        }

        private void RadioMagnet1_CheckedChanged(object sender, EventArgs e)
        {
            ScrollHeight.Value = (int)magnet1.center.Y;
            ScrollWidth.Value = (int)magnet1.center.X;
            ScrollAngle.Value = (int)(magnet1.angle * 360 / 2 / Math.PI);
        }

        private void RadioMagnet2_CheckedChanged(object sender, EventArgs e)
        {
            ScrollHeight.Value = (int)magnet2.center.Y;
            ScrollWidth.Value = (int)magnet2.center.X;
            ScrollAngle.Value = (int)(magnet2.angle * 360 / 2 / Math.PI);
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            list_triangles.Clear();
            step_x = (float)double.Parse(StepXBox.Text);
            step_y = (float)double.Parse(StepYBox.Text);

            magnet1.width = (float)double.Parse(Width1Box.Text);
            magnet2.width = (float)double.Parse(Width2Box.Text);
            magnet1.height = (float)double.Parse(Height1Box.Text);
            magnet2.height = (float)double.Parse(Height2Box.Text);

            points.Clear();
            points.AddRange(magnet1.ReturnRectangleDdiscret()); points.AddRange(magnet2.ReturnRectangleDdiscret());
            points.AddRange(GlobalRect.ReturnRectangleDdiscret());

            Painting();
        }

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

            foreach (PointF point in points)
            {
                graph.FillRectangle(brushPoint, (float)parametrs.X(width, point.X) - 2, (float)parametrs.Y(height, point.Y) - 2, 4, 4);

            }

            GraphicsBox.Image = bmp;
        }

        private void DrawBtn_Click(object sender, EventArgs e)
        {
            triangulation = new Triangulation();
            list_triangles.Clear();

            PointF point1 = new PointF(300, 0); PointF point2 = new PointF(-300, 0);
            PointF point3 = new PointF(0, 300); PointF point4 = new PointF(0, -300);

            triangulation.AddGlobalPoints(point1, point2, point3, point4,
                                           GlobalRect.ReturnRectangleDdiscret(),
                                           magnet1,
                                           magnet2);
            list_triangles.AddRange(triangulation.ReturnAllTriangles(triangulation.points));
            triangulation.UpdateListTriangle(list_triangles);

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
                if (y_now > GlobalRect.C.Y - step_y / 2)
                {
                    timer1.Stop(); list_triangles.Clear(); list_triangles.AddRange(triangulation.SortTriangle()); Painting();
                }
                x_now = GlobalRect.A.X;
            }

            Random rand = new Random();
            PointF point = new PointF(x_now + (float)rand.NextDouble() / 100, y_now + (float)rand.NextDouble() / 100);
            list_triangles.Clear();
            list_triangles.AddRange(triangulation.AddPoint(point));
            x_now += step_x;
            Painting();

        }
    }


}
