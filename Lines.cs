using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public struct LevelLines { public PointF point1; public PointF point2; }
    public class Lines
    {
        private List<TrianglePotential> triangles;
        public List<LevelLines> levelLines;
        private List<float> levels;

        public Lines(List<TrianglePotential> p_triangles, float Umax, float Umin, float stepU)
        {
            levelLines = new List<LevelLines>();
            triangles = new List<TrianglePotential>();
            triangles.Clear();
            triangles.AddRange(p_triangles);

            levels = new List<float> { Umin };
            do
            {
                levels.Add(levels.Last() + stepU);
            } while (levels.Last() <= Umax);

        }

        public List<LevelLines> GetLevelLines()
        {
            levelLines.Clear();
            foreach (float level in levels)
            {
                foreach (TrianglePotential triangle in triangles)
                {
                    Potential pot1 = new Potential();
                    Potential pot2 = new Potential();
                    Potential pot3 = new Potential();

                    /*Располагаю точки по возрастанию от 1й к 3й*/
                    if (triangle.point1.value >= triangle.point2.value && triangle.point2.value > triangle.point3.value)
                    {
                        pot1 = triangle.point1;
                        pot2 = triangle.point2;
                        pot3 = triangle.point3;
                    }
                    else if (triangle.point1.value >= triangle.point3.value && triangle.point3.value > triangle.point2.value)
                    {
                        pot1 = triangle.point1;
                        pot2 = triangle.point3;
                        pot3 = triangle.point2;
                    }
                    else if (triangle.point2.value >= triangle.point1.value && triangle.point1.value > triangle.point3.value)
                    {
                        pot1 = triangle.point2;
                        pot2 = triangle.point1;
                        pot3 = triangle.point3;
                    }
                    else if (triangle.point2.value >= triangle.point3.value && triangle.point3.value > triangle.point1.value)
                    {
                        pot1 = triangle.point2;
                        pot2 = triangle.point3;
                        pot3 = triangle.point1;
                    }
                    else if (triangle.point3.value >= triangle.point2.value && triangle.point2.value > triangle.point1.value)
                    {
                        pot1 = triangle.point3;
                        pot2 = triangle.point2;
                        pot3 = triangle.point1;
                    }
                    else if (triangle.point3.value >= triangle.point1.value && triangle.point1.value > triangle.point2.value)
                    {
                        pot1 = triangle.point3;
                        pot2 = triangle.point1;
                        pot3 = triangle.point2;
                    }

                    if (level <= pot1.value && level >= pot3.value)
                    {
                        PointF point1 = new PointF();
                        PointF point2 = new PointF();
                        if (level > pot2.value)
                        {
                            point1.X = pot1.point.X -
                                (pot1.value - level) / (pot1.value - pot2.value) * (pot1.point.X - pot2.point.X);
                            point1.Y = pot1.point.Y -
                                (pot1.value - level) / (pot1.value - pot2.value) * (pot1.point.Y - pot2.point.Y);
                        }
                        else
                        {
                            point1.X = pot2.point.X -
                                (pot2.value - level) / (pot2.value - pot3.value) * (pot2.point.X - pot3.point.X);
                            point1.Y = pot2.point.Y -
                                (pot2.value - level) / (pot2.value - pot3.value) * (pot2.point.Y - pot3.point.Y);
                        }
                        point2.X = pot1.point.X -
                                (pot1.value - level) / (pot1.value - pot3.value) * (pot1.point.X - pot3.point.X);
                        point2.Y = pot1.point.Y -
                            (pot1.value - level) / (pot1.value - pot3.value) * (pot1.point.Y - pot3.point.Y);
                        LevelLines ll = new LevelLines { point1 = point1, point2 = point2 };
                        levelLines.Add(ll);
                    }

                }


            }
            return levelLines;
        }



    }
}
