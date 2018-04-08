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
            magnet1Potential = new List<Potential>();
            magnet2Potential = new List<Potential>();
            RectPotential = new List<Potential>();
            unknownPotential = new List<Potential>();
            allPotential = new List<Potential>();

            trianglesWithPotential = new List<TrianglePotential>();
            levelLines = new List<LevelLines>();
            forceLines = new List<LevelLines>();

            magnet1 = new Magnet(70, 10, center1, (float)(2 * Math.PI / 360 * 45), 3);
            magnet2 = new Magnet(70, 10, center2, (float)(2 * Math.PI / 360 * 30), 3);
            GlobalRect = new Magnet(160, 160, centerGlobal, (float)(2 * Math.PI / 360 * 0), 5);


            magnet1Potential = magnet1.ReturnPotential(-10, 10);
            magnet2Potential = magnet2.ReturnPotential(-10, 10);
            RectPotential = GlobalRect.ReturnPotential(0, 0);

            ScrollHeight.Minimum = (int)GlobalRect.A.Y;
            ScrollHeight.Value = (int)magnet1.center.Y;
            ScrollHeight.Maximum = (int)GlobalRect.D.Y;
        
            ScrollWidth.Minimum = (int)GlobalRect.A.X ;
            ScrollWidth.Value = (int)magnet1.center.X;
            ScrollWidth.Maximum = (int)GlobalRect.B.X;

            ScrollAngle.Minimum = 0;
            ScrollAngle.Maximum = 360;
            ScrollAngle.Value = 45;

            StepXBox.Text = "7";
            StepYBox.Text = "7";

            plusUBox1.Text = "10";
            plusUBox2.Text = "10";
            minusUBox1.Text = "-10";
            minusUBox2.Text = "-10";

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

        List<Potential> unknownPotential;
        List<Potential> allPotential;

        List<TrianglePotential> trianglesWithPotential;
        List<LevelLines> levelLines;
        List<LevelLines> forceLines;

        List<Potential> magnet1Potential;
        List<Potential> magnet2Potential;
        List<Potential> RectPotential;
        Triangulation triangulation;

        Magnet magnet1; PointF center1 = new PointF(0, -50);
        Magnet magnet2; PointF center2 = new PointF(0, 50);
        Magnet GlobalRect; PointF centerGlobal = new PointF(0, 0);

        float step_x = 7F, step_y = 7F, x_now, y_now;

        List<Triangle> list_triangles = new List<Triangle>();
        private void Painting()
        {
            int width = GraphicsBox.Width, height = GraphicsBox.Height;

            bmp = new Bitmap(width, height);
            graph = Graphics.FromImage(bmp);
           graph.SmoothingMode = SmoothingMode.AntiAlias;

            SolidBrush brushBkgrd = new SolidBrush(Color.Black);
            SolidBrush brushPoint = new SolidBrush(Color.Red);

            SolidBrush brushMinPot = new SolidBrush(Color.Blue);
            SolidBrush brushMaxPot = new SolidBrush(Color.Red);
            SolidBrush brushNullPot = new SolidBrush(Color.White);
            graph.FillRectangle(brushBkgrd, 0, 0, width, height);


            Pen trianglePen = new Pen(Color.Yellow, 1);
            Pen magnetPen = new Pen(Color.DarkBlue, 3);



            if (radioTriangle.Checked)
            {
                for (int i = 0; i < list_triangles.Count; i++)
                {
                    graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point1.X), (float)parametrs.Y(height, list_triangles[i].point1.Y),
                                                (float)parametrs.X(width, list_triangles[i].point2.X), (float)parametrs.Y(height, list_triangles[i].point2.Y));
                    graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point2.X), (float)parametrs.Y(height, list_triangles[i].point2.Y),
                                                (float)parametrs.X(width, list_triangles[i].point3.X), (float)parametrs.Y(height, list_triangles[i].point3.Y));
                    graph.DrawLine(trianglePen, (float)parametrs.X(width, list_triangles[i].point1.X), (float)parametrs.Y(height, list_triangles[i].point1.Y),
                                                (float)parametrs.X(width, list_triangles[i].point3.X), (float)parametrs.Y(height, list_triangles[i].point3.Y));

                }
            }

           
            //for(int i=0; i<magnet1Potential.Count-1; i++)
            //{
            //    graph.DrawLine(magnetPen, (float)parametrs.X(width, magnet1Potential[i].point.X), (float)parametrs.Y(height, magnet1Potential[i].point.Y),
            //                                   (float)parametrs.X(width, magnet1Potential[i+1].point.X), (float)parametrs.Y(height, magnet1Potential[i+1].point.Y));

            //}


            foreach (Potential pot in magnet1Potential)
            {
                if (pot.value < 0)
                    graph.FillEllipse(brushMinPot, (float)parametrs.X(width, pot.point.X) - 4, (float)parametrs.Y(height, pot.point.Y) - 4, 8, 8);
                else
                    graph.FillEllipse(brushMaxPot, (float)parametrs.X(width, pot.point.X) - 4, (float)parametrs.Y(height, pot.point.Y) - 4, 8, 8);

            }

            foreach (Potential pot in magnet2Potential)
            {
                if (pot.value < 0)
                    graph.FillEllipse(brushMinPot, (float)parametrs.X(width, pot.point.X) - 4, (float)parametrs.Y(height, pot.point.Y) - 4, 8, 8);
                else
                    graph.FillEllipse(brushMaxPot, (float)parametrs.X(width, pot.point.X) - 4, (float)parametrs.Y(height, pot.point.Y) - 4, 8, 8);

            }

            foreach (Potential pot in RectPotential)
            {

                graph.FillRectangle(brushNullPot, (float)parametrs.X(width, pot.point.X) - 2, (float)parametrs.Y(height, pot.point.Y) - 2, 4, 4);

            }

            if (radioPotential.Checked)
            {
                foreach (Potential pot in unknownPotential)
                {
                    SolidBrush tempBrush;
                    if (pot.value > 0)
                    {
                        tempBrush = new SolidBrush(Color.FromArgb((int)(255 / 10 * pot.value), 0, 0));
                      // tempBrush = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        tempBrush = new SolidBrush(Color.FromArgb(0, 0, (int)(255 / 10 * Math.Abs(pot.value))));
                     //  tempBrush = new SolidBrush(Color.Blue);
                    }
                    graph.FillRectangle(tempBrush, (float)parametrs.X(width, pot.point.X) - 4, (float)parametrs.Y(height, pot.point.Y) - 4, 8, 8);

                }

                if (levelLines.Count > 0&&CheckIsoline.Checked)
                {
                    foreach (LevelLines line in levelLines)
                    {
                        graph.DrawLine(trianglePen, (float)parametrs.X(width, line.point1.X), (float)parametrs.Y(height, line.point1.Y),
                                               (float)parametrs.X(width, line.point2.X), (float)parametrs.Y(height, line.point2.Y));

                    }
                }

                if (forceLines.Count > 0&&CheckForce.Checked)
                {
                    foreach (LevelLines line in forceLines)
                    {
                        graph.DrawLine(trianglePen, (float)parametrs.X(width, line.point1.X), (float)parametrs.Y(height, line.point1.Y),
                                               (float)parametrs.X(width, line.point2.X), (float)parametrs.Y(height, line.point2.Y));

                    }
                }
            }

            GraphicsBox.Image = bmp;
        }

        #region Обработка событий скроллинга
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
            magnet1Potential.Clear();
            magnet2Potential.Clear();
            RectPotential.Clear();



            magnet1Potential = magnet1.ReturnPotential((float)double.Parse(minusUBox1.Text), (float)double.Parse(plusUBox1.Text));
            magnet2Potential = magnet2.ReturnPotential((float)double.Parse(minusUBox2.Text), (float)double.Parse(plusUBox2.Text));
            RectPotential = GlobalRect.ReturnPotential(0, 0);
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
            magnet1Potential.Clear();
            magnet2Potential.Clear();
            RectPotential.Clear();

            magnet1Potential = magnet1.ReturnPotential((float)double.Parse(minusUBox1.Text), (float)double.Parse(plusUBox1.Text));
            magnet2Potential = magnet2.ReturnPotential((float)double.Parse(minusUBox2.Text), (float)double.Parse(plusUBox2.Text));
            RectPotential = GlobalRect.ReturnPotential(0, 0);
            Painting();
        }

        private void ScrollAngle_Scroll(object sender, EventArgs e)
        {
            float angle = (float)(2 * Math.PI / 360 * ScrollAngle.Value);
            if (RadioMagnet1.Checked)
            {
                magnet1.angle = angle;
            }

            if (RadioMagnet2.Checked)
            {
                magnet2.angle = angle;
            }
            magnet1Potential.Clear();
            magnet2Potential.Clear();
            RectPotential.Clear();

            magnet1Potential = magnet1.ReturnPotential((float)double.Parse(minusUBox1.Text), (float)double.Parse(plusUBox1.Text));
            magnet2Potential = magnet2.ReturnPotential((float)double.Parse(minusUBox2.Text), (float)double.Parse(plusUBox2.Text));
            RectPotential = GlobalRect.ReturnPotential(0, 0);
            Painting();
        }
        #endregion

        #region Обработка событий радиокнопок
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

        #endregion

        private void radioTriangle_CheckedChanged(object sender, EventArgs e)
        {
            Painting();
        }

        private void radioPotential_CheckedChanged(object sender, EventArgs e)
        {
            Painting();
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

            magnet1Potential.Clear();
            magnet2Potential.Clear();
            RectPotential.Clear();

            magnet1Potential = magnet1.ReturnPotential((float)double.Parse(minusUBox1.Text), (float)double.Parse(plusUBox1.Text));
            magnet2Potential = magnet2.ReturnPotential((float)double.Parse(minusUBox2.Text), (float)double.Parse(plusUBox2.Text));
            RectPotential = GlobalRect.ReturnPotential(0, 0);

            Painting();
        }

        private void CheckIsoline_CheckedChanged(object sender, EventArgs e)
        {
            Painting();
        }

        private void CheckForce_CheckedChanged(object sender, EventArgs e)
        {
            Painting();
        }

        private void DrawBtn_Click(object sender, EventArgs e)
        {
            triangulation = new Triangulation();
            list_triangles.Clear();

            PointF point1 = new PointF(300, 0); PointF point2 = new PointF(-300, 0);
            PointF point3 = new PointF(0, 300); PointF point4 = new PointF(0, -300);

            triangulation.AddGlobalPoints(point1, point2, point3, point4,
                                           GlobalRect.points,
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
                    timer1.Stop();
                    list_triangles.Clear();
                    list_triangles.AddRange(triangulation.SortTriangle());
                    Painting();
                    Galerkin galerkin = new Galerkin(list_triangles,
                                                    points,
                                                    GlobalRect.points,
                                                    magnet1.points,
                                                    magnet2.points,
                                                    magnet1Potential,
                                                    magnet2Potential,
                                                    RectPotential,
                                                    triangulation.unknownPotential);
                    galerkin.CalculatePotential();
                    unknownPotential.Clear();
                    unknownPotential.AddRange(galerkin.unknownPotential);
                    allPotential.Clear();
                    allPotential.AddRange(magnet1Potential);
                    allPotential.AddRange(magnet2Potential);
                    allPotential.AddRange(RectPotential);
                    allPotential.AddRange(unknownPotential);
                    CreateTrianglesWithPotential();

                    Lines lines = new Lines(trianglesWithPotential, 5, -5, 2);
                    levelLines.Clear();
                    levelLines.AddRange(lines.GetLevelLines());
                    ForceLines force = new ForceLines(trianglesWithPotential, magnet1Potential, magnet2Potential);
                    forceLines.Clear();
                    forceLines.AddRange(force.GetForceLines());

                }
                x_now = GlobalRect.A.X+step_x;
            }

            Random rand = new Random();
            PointF point = new PointF(x_now + (float)rand.NextDouble()/10, y_now + (float)rand.NextDouble()/10);
            list_triangles.Clear();
            list_triangles.AddRange(triangulation.AddPoint(point));



            points.Add(point);
            x_now += step_x;
            Painting();

        }

        private void CreateTrianglesWithPotential()
        {
            trianglesWithPotential.Clear();
            foreach(Triangle triangle in list_triangles)
            {
                TrianglePotential TP = new TrianglePotential();

                foreach(Potential pot in allPotential)
                {
                    if (pot.point == triangle.point1) { TP.point1.point = triangle.point1; TP.point1.value = pot.value; continue; }
                    if (pot.point == triangle.point2) { TP.point2.point = triangle.point2; TP.point2.value = pot.value; continue; }
                    if (pot.point == triangle.point3) { TP.point3.point = triangle.point3; TP.point3.value = pot.value; continue; }
                }
                trianglesWithPotential.Add(TP);
                
            }
        }
    }


}
