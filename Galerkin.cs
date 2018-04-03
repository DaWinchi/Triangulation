using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct Piramide
    {
        public List<Triangle> base_triangles; public PointF vertex;
        public float z; public float volume;
    }
    public struct Potential { public PointF point; public float value; }
    public class Galerkin
    {
        public List<Piramide> piramides;
        private List<Triangle> triangles;

        private List<PointF> all_points;
        private List<PointF> searching_points;
        private List<PointF> rect_points;
        private List<PointF> magnet1_points;
        private List<PointF> magnet2_points;

        public List<Potential> unknownPotential;
        public List<Potential> magnet1Potential;
        public List<Potential> magnet2Potential;
        public List<Potential> rectPotential;

        private List<List<float>> A;
        private List<float> B;
        private List<float> C;
        public Galerkin(List<Triangle> p_triangle,
                           List<PointF> p_allpoints,
                           List<PointF> p_rectpoints,
                           List<PointF> p_magnet1,
                           List<PointF> p_magnet2,
                           List<Potential> p_magnet1Potential,
                           List<Potential> p_magnet2Potential,
                           List<Potential> p_rectPotential,
                           List<Potential> p_unknownPotential)
        {
            triangles = new List<Triangle>();
            triangles.AddRange(p_triangle);
            all_points = new List<PointF>();
            all_points.AddRange(p_allpoints);
            rect_points = new List<PointF>();
            rect_points.AddRange(p_rectpoints);
            magnet1_points = new List<PointF>();
            magnet1_points.AddRange(p_magnet1);
            magnet2_points = new List<PointF>();
            magnet2_points.AddRange(p_magnet2);

            piramides = new List<Piramide>();

            unknownPotential = new List<Potential>();
            magnet1Potential = new List<Potential>();
            magnet2Potential = new List<Potential>();
            rectPotential = new List<Potential>();

            magnet1Potential.AddRange(p_magnet1Potential);
            magnet2Potential.AddRange(p_magnet2Potential);
            rectPotential.AddRange(p_rectPotential);
            unknownPotential.AddRange(p_unknownPotential);

            A = new List<List<float>>();
            B = new List<float>();
            C = new List<float>();

        }

        private float SquareTriangle(Triangle triangle)
        {
            float square = 0;
            square = Math.Abs((triangle.point2.X - triangle.point1.X) *
                             (triangle.point3.Y - triangle.point1.Y) -
                             (triangle.point2.Y - triangle.point1.Y) *
                             (triangle.point3.X - triangle.point1.X))/2;
            return square;
        }

        private float DpDx(Triangle triangle, PointF vertex)
        {
            PointF point1 = vertex, point2 = vertex, point3 = vertex; //просто начальная инициализация
            if (triangle.point1 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point3; }
            if (triangle.point2 == vertex) { point1 = vertex; point2 = triangle.point1; point3 = triangle.point3; }
            if (triangle.point3 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point1; }

            float value = (point2.Y - point1.Y) * (-1) - (point3.Y - point1.Y) * (-1);
            return value;
        }

        private float DpDy(Triangle triangle, PointF vertex)
        {
            PointF point1 = vertex, point2 = vertex, point3 = vertex; //просто начальная инициализация
            if (triangle.point1 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point3; }
            if (triangle.point2 == vertex) { point1 = vertex; point2 = triangle.point1; point3 = triangle.point3; }
            if (triangle.point3 == vertex) { point1 = vertex; point2 = triangle.point2; point3 = triangle.point1; }

            float value = (point2.X - point1.X) - (point3.X - point1.X);
            return value;
        }

        private float VolumePiramide(Piramide piramide)
        {
            float volume = 0; //объём пирамиды

            foreach (Triangle triangle in piramide.base_triangles)
            {
                volume += 1 / 3 * SquareTriangle(triangle) * piramide.z;
            }

            return 0;
        }

        private void InitializePiramides(List<Triangle> p_triangles, List<PointF> p_points)
        {
            triangles.AddRange(p_triangles);
            all_points.AddRange(p_points);

            foreach (PointF point in all_points)
            {
                Piramide piramide = new Piramide();
                piramide.base_triangles = new List<Triangle>();
                piramide.vertex = point;
                piramide.z = 1;
                foreach (Triangle triangle in triangles)
                {
                    if (triangle.point1 == point || triangle.point2 == point || triangle.point3 == point)
                        piramide.base_triangles.Add(triangle);
                }
                piramide.volume = VolumePiramide(piramide);

            }
        }

        public void CreateMatrixA()
        {
            A.Clear();
            for (int i = 0; i < unknownPotential.Count; i++)
            {
                List<float> list_a = new List<float>();
                for (int j = 0; j < unknownPotential.Count; j++)
                {
                    float value = 0;
                    if (i == j)
                    {
                        List<Triangle> tempTringles = new List<Triangle>();
                        foreach (Triangle triangle in triangles)
                        {
                            if (triangle.point1 == unknownPotential[i].point ||
                                triangle.point2 == unknownPotential[i].point ||
                                triangle.point3 == unknownPotential[i].point)
                                tempTringles.Add(triangle);
                        }

                        foreach (Triangle triangle in tempTringles)
                        {
                            value += SquareTriangle(triangle) *
                                (DpDx(triangle, unknownPotential[i].point) * DpDx(triangle, unknownPotential[i].point) +
                                DpDy(triangle, unknownPotential[i].point) * DpDy(triangle, unknownPotential[i].point));
                        }
                        list_a.Add(value);
                    }
                    else if (i != j)
                    {
                        List<Triangle> tempTringles = new List<Triangle>();
                        foreach (Triangle triangle in triangles)
                        {
                            PointF pi = unknownPotential[i].point, pj = unknownPotential[j].point;
                            if ((triangle.point1 == pi && triangle.point2 == pj) ||
                                (triangle.point1 == pi && triangle.point3 == pj) ||
                                (triangle.point2 == pi && triangle.point1 == pj) ||
                                (triangle.point2 == pi && triangle.point3 == pj) ||
                                (triangle.point3 == pi && triangle.point1 == pj) ||
                                (triangle.point3 == pi && triangle.point2 == pj))
                                tempTringles.Add(triangle);
                        }

                        if (tempTringles.Count == 0) { value += 0; list_a.Add(value); }
                        else
                        {
                            foreach (Triangle triangle in tempTringles)
                            {
                                PointF point1 = new PointF();
                                PointF point2 = new PointF();
                                PointF point3 = new PointF();

                                if(triangle.point1==unknownPotential[i].point)
                                {
                                    if(triangle.point2==unknownPotential[j].point)
                                    {
                                        point1 = triangle.point1;
                                        point2 = triangle.point2;
                                        point3 = triangle.point3;
                                    }
                                    if (triangle.point3 == unknownPotential[j].point)
                                    {
                                        point1 = triangle.point1;
                                        point2 = triangle.point3;
                                        point3 = triangle.point2;
                                    }
                                }
                                if (triangle.point2 == unknownPotential[i].point)
                                {
                                    if (triangle.point1 == unknownPotential[j].point)
                                    {
                                        point1 = triangle.point2;
                                        point2 = triangle.point1;
                                        point3 = triangle.point3;
                                    }
                                    if (triangle.point3 == unknownPotential[j].point)
                                    {
                                        point1 = triangle.point2;
                                        point2 = triangle.point3;
                                        point3 = triangle.point1;
                                    }
                                }
                                if (triangle.point3 == unknownPotential[i].point)
                                {
                                    if (triangle.point2 == unknownPotential[j].point)
                                    {
                                        point1 = triangle.point3;
                                        point2 = triangle.point2;
                                        point3 = triangle.point1;
                                    }
                                    if (triangle.point1 == unknownPotential[j].point)
                                    {
                                        point1 = triangle.point3;
                                        point2 = triangle.point1;
                                        point3 = triangle.point2;
                                    }
                                }

                                float koef_Ai = (point2.Y - point1.Y) * (-1) - (point3.Y - point1.Y) * (-1);
                                float koef_Bi = (point3.X - point1.X) * (-1) - (point2.X - point1.X) * (-1);

                                float koef_Aj = (point2.Y - point1.Y) * (0) - (point3.Y - point1.Y) * (1);
                                float koef_Bj = (point3.X - point1.X) * (1) - (point2.X - point1.X) * (0);

                                value += SquareTriangle(triangle) *
                                    (koef_Ai * koef_Aj + koef_Bi * koef_Bj);

                            }
                            list_a.Add(value);
                        }
                    }
                }
                A.Add(list_a);
            }
        }
        public void CreateB()
        {
            B.Clear();
            List<Potential> boardPotential = new List<Potential>();
            boardPotential.AddRange(magnet1Potential);
            boardPotential.AddRange(magnet2Potential);
            boardPotential.AddRange(rectPotential);
            for (int i = 0; i < unknownPotential.Count; i++)
            {
                float value = 0;
                for (int j = 0; j < boardPotential.Count; j++)
                {
                    List<Triangle> tempTringles = new List<Triangle>();
                    foreach (Triangle triangle in triangles)
                    {
                        PointF pi = unknownPotential[i].point, pj = boardPotential[j].point;
                        if ((triangle.point1 == pi && triangle.point2 == pj) ||
                            (triangle.point1 == pi && triangle.point3 == pj) ||
                            (triangle.point2 == pi && triangle.point1 == pj) ||
                            (triangle.point2 == pi && triangle.point3 == pj) ||
                            (triangle.point3 == pi && triangle.point1 == pj) ||
                            (triangle.point3 == pi && triangle.point2 == pj))
                            tempTringles.Add(triangle);
                    }

                    if (tempTringles.Count != 0)
                    {
                        foreach (Triangle triangle in tempTringles)
                        {
                            PointF point1 = new PointF();
                            PointF point2 = new PointF();
                            PointF point3 = new PointF();


                            if (triangle.point1 == unknownPotential[i].point)
                            {
                                if (triangle.point2 == boardPotential[j].point)
                                {
                                    point1 = triangle.point1;
                                    point2 = triangle.point2;
                                    point3 = triangle.point3;
                                }
                                if (triangle.point3 == boardPotential[j].point)
                                {
                                    point1 = triangle.point1;
                                    point2 = triangle.point3;
                                    point3 = triangle.point2;
                                }
                            }
                            if (triangle.point2 == unknownPotential[i].point)
                            {
                                if (triangle.point1 == boardPotential[j].point)
                                {
                                    point1 = triangle.point2;
                                    point2 = triangle.point1;
                                    point3 = triangle.point3;
                                }
                                if (triangle.point3 == boardPotential[j].point)
                                {
                                    point1 = triangle.point2;
                                    point2 = triangle.point3;
                                    point3 = triangle.point1;
                                }
                            }
                            if (triangle.point3 == unknownPotential[i].point)
                            {
                                if (triangle.point2 == boardPotential[j].point)
                                {
                                    point1 = triangle.point3;
                                    point2 = triangle.point2;
                                    point3 = triangle.point1;
                                }
                                if (triangle.point1 == boardPotential[j].point)
                                {
                                    point1 = triangle.point3;
                                    point2 = triangle.point1;
                                    point3 = triangle.point2;
                                }
                            }

                            float koef_Ai = (point2.Y - point1.Y) * (-1) - (point3.Y - point1.Y) * (-1);
                            float koef_Bi = (point3.X - point1.X) * (-1) - (point2.X - point1.X) * (-1);

                            float koef_Aj = (point2.Y - point1.Y) * (0) - (point3.Y - point1.Y) * (1);
                            float koef_Bj = (point3.X - point1.X) * (1) - (point2.X - point1.X) * (0);

                            value +=boardPotential[j].value*SquareTriangle(triangle) *
                                (koef_Ai * koef_Aj + koef_Bi * koef_Bj);
                        }
                    }

                }
                B.Add(value*(-1));
            }
        }
        public List<float> MethodKachmarzh(float[] a, float[] b, int nn, int ny)
        {
            /* матрица А, столбец свободных членов, массив неизвестных,
            nn - количество неизвестных;  ny - количество уравнений*/
            float[] x = new float[nn];
            float eps = 1e-6f;
            //float s;
            int i, j, k;
            float s1, s2, fa1, t;
            float[] x1 = new float[nn];

            x[0] = 0.5f;
            for (i = 1; i < nn; i++) x[i] = 0f;

            s1 = s2 = 1f;
            while (s1 > eps * s2)
            {
                for (i = 0; i < nn; i++) x1[i] = x[i];

                for (i = 0; i < ny; i++)
                {
                    s1 = 0f;
                    s2 = 0f;
                    for (j = 0; j < nn; j++)
                    {
                        fa1 = a[i * nn + j];
                        s1 += fa1 * x[j];
                        s2 += fa1 * fa1;
                    }
                    t = (b[i] - s1) / s2;
                    for (k = 0; k < nn; k++) x[k] += a[i * nn + k] * t;
                }

                s1 = 0f;
                s2 = 0f;
                for (i = 0; i < nn; i++)
                {
                    s1 += (x[i] - x1[i]) * (x[i] - x1[i]);
                    s2 += x[i] * x[i];
                }
                s1 = (float)Math.Sqrt(s1);
                s2 = (float)Math.Sqrt(s2);
            }
            return x.ToList();
        }
        public void CalculatePotential()
        {
            StreamWriter streamA = new StreamWriter("A.txt");
            StreamWriter streamB = new StreamWriter("B.txt");
            StreamWriter streamC = new StreamWriter("C.txt");
            CreateMatrixA();
            CreateB();
            C.Clear();
            List<float> buf = new List<float>();
            float[] a = new float[A.Count * A.Count];
            float[] b = new float[B.Count];
            for (int i = 0; i < A.Count; i++)
            {
                buf.AddRange(A[i]);
                for(int j=0; j<A[i].Count; j++)
                {
                    streamA.Write(A[i][j]+"  ");

                }
                streamA.WriteLine();

            }
            a = buf.ToArray();
            for (int i = 0; i < B.Count; i++) streamB.WriteLine(B[i]);
            b = B.ToArray();
            C.AddRange(MethodKachmarzh(a, b, B.Count, B.Count));
            for (int i = 0; i < C.Count; i++) streamC.WriteLine(C[i]);

            streamA.Close();
            streamB.Close();
            streamC.Close();
            List<Potential> newPotential = new List<Potential>();
            for (int i = 0; i < unknownPotential.Count; i++)
            {
                Potential pot = new Potential { point = unknownPotential[i].point, value = C[i] };
                newPotential.Add(pot);
            }
            unknownPotential.Clear();
            unknownPotential.AddRange(newPotential);

        }

    }
}
