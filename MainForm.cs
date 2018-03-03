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
        Triangulation triangulation = new Triangulation();
        List<Triangle> list_triangles = new List<Triangle>();

        private void Painting()
        {
            int width=GraphicsBox.Width, height= GraphicsBox.Height;

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
            list_triangles.Clear();
            PointF center1 = new PointF(0, -50);
            float angle1 = (float)(2 * Math.PI / 360 * 0);
            PointF center2 = new PointF(0, 50);
            float angle2 = (float)(2 * Math.PI / 360 * 0);
            PointF centerGlobal = new PointF(0, 0);

            Magnet magnet1 = new Magnet(20, 20, center1,angle1, 5);
            Magnet magnet2 = new Magnet(20, 20, center2, angle2,5);
            Magnet GlobalRect = new Magnet(160, 160, centerGlobal, 0, 20);
            PointF point1 = new PointF(200, 0); PointF point2 = new PointF(-200, 0);
            PointF point3 = new PointF(0, 200); PointF point4 = new PointF(0, -200);

            triangulation.AddGlobalPoints(point1, point2, point3, point4,
                                           GlobalRect.ReturnRectangleDdiscret(),
                                           magnet1.ReturnRectangleDdiscret(),
                                           magnet2.ReturnRectangleDdiscret());
            list_triangles.AddRange(triangulation.ReturnTriangles());

            Painting();
        }
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
            points.Clear();

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
