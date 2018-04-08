using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleDeloneWithMagnetic
{
    public class ForceLines
    {
        private List<TrianglePotential> triangles;
        private List<LevelLines> forceLines;

        List<Potential> magnet1;
        List<Potential> magnet2;

        public ForceLines(List<TrianglePotential> p_triangles, List<Potential> p_magnet1, List<Potential> p_magnet2)
        {
            triangles = new List<TrianglePotential>();
            forceLines = new List<LevelLines>();

            triangles.AddRange(p_triangles);

            magnet1 = new List<Potential>();
            magnet1.AddRange(p_magnet1);

            magnet2 = new List<Potential>();
            magnet2.AddRange(p_magnet2);

        }
        #region Производные для градиента
        float DpDy(Potential pot1, Potential pot2, Potential pot3)
        {
            float B = -((pot2.point.X - pot1.point.X) * (pot3.value - pot1.value) -
                (pot3.point.X - pot1.point.X) * (pot2.value - pot1.value));
            return B;
        }

        float DpDx(Potential pot1, Potential pot2, Potential pot3)
        {
            float A = (pot2.point.Y - pot1.point.Y) * (pot3.value - pot1.value) -
                (pot3.point.Y - pot1.point.Y) * (pot2.value - pot1.value);
            return A;
        }

        float DpDz(Potential pot1, Potential pot2, Potential pot3)
        {
            float C = (pot2.point.X - pot1.point.X) * (pot3.point.Y - pot1.point.Y) -
                (pot3.point.X - pot1.point.X) * (pot2.point.Y - pot1.point.Y);
            return C;
        }
        #endregion

        public List<LevelLines> GetForceLines()
        {
            int size_triangles = triangles.Count;
            int size_magnet1 = magnet1.Count;
            int size_magnet2 = magnet2.Count;
            forceLines.Clear();
            for (int i = 0; i < size_magnet1; ++i)
            {
                Potential pot = magnet1[i];
                PointF reducentPoint = new PointF();
                reducentPoint = pot.point;

               
                    int num_triangle = 0;
                    TrianglePotential triangle = new TrianglePotential();
                    PointF passPoint1 = new PointF();
                    PointF passPoint2 = new PointF();
                    PointF mainPoint = new PointF();
                    PointF pointIntersect = new PointF(); //точка пересечения
                    /*Поиск любого треугольника на границе магнита содержащего точку pot*/
                    for (int j = 0; j < size_triangles; j++)
                    {
                        if (pot.point == triangles[j].point1.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point2.point;
                            mainPoint = triangles[j].point3.point;
                            break;
                        }
                        if (pot.point == triangles[j].point2.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point1.point;
                            mainPoint = triangles[j].point3.point;
                            break;
                        }
                        if (pot.point == triangles[j].point3.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point2.point;
                            mainPoint = triangles[j].point1.point;
                            break;
                        }
                    }
                    triangle = triangles[num_triangle];

                    float A = -DpDx(triangle.point1, triangle.point2, triangle.point3);
                    float B = -DpDy(triangle.point1, triangle.point2, triangle.point3);

                    //крайняя точка градиента
                    pointIntersect.X = (passPoint1.X + passPoint2.X) / 2;
                    pointIntersect.Y = (passPoint1.Y + passPoint2.Y) / 2;
                    pot.point = pointIntersect;
                    PointF pot2 = new PointF { X = pot.point.X + A, Y = pot.point.Y + B };


                    LevelLines line = new LevelLines();

                    PointF futureBoardPoint1 = new PointF(); //точки стороны, с которой будет пересечение градиента
                    PointF futureBoardPoint2 = new PointF();

                   

                    //Пробую найти пересечение с однйо из сторон треугольника
                    float ka = 0, kb = 0;
                    ka = Ua(mainPoint, passPoint2, pot.point, pot2);
                    kb = Ub(mainPoint, passPoint2, pot.point, pot2);
                    PointF intersectBuf = new PointF();
                    if (ka <= 1 && ka >= 0)
                    {
                        intersectBuf.X = mainPoint.X + ka * (passPoint2.X - mainPoint.X);
                        intersectBuf.Y = mainPoint.Y + ka * (passPoint2.Y - mainPoint.Y);
                        futureBoardPoint1 = mainPoint;
                        futureBoardPoint2 = passPoint2;
                        reducentPoint = passPoint1;

                        pointIntersect = intersectBuf;
                        line.point1 = pot.point;
                        line.point2 = pointIntersect;

                        forceLines.Add(line);
                    }
                    else
                    { 
                        ka = Ua(mainPoint, passPoint1, pot.point, pot2);
                        kb = Ub(mainPoint, passPoint1, pot.point, pot2);

                        if (ka <= 1 && ka >= 0)
                        {
                            intersectBuf.X = mainPoint.X + ka * (passPoint1.X - mainPoint.X);
                            intersectBuf.Y = mainPoint.Y + ka * (passPoint1.Y - mainPoint.Y);
                            futureBoardPoint1 = mainPoint;
                            futureBoardPoint2 = passPoint1;
                            reducentPoint = passPoint2;

                                pointIntersect = intersectBuf;
                                line.point1 = pot.point;
                                line.point2 = pointIntersect;

                                forceLines.Add(line);
                            
                        }
                    }
                  

                    for (int z = 0; z < 200; z++)
                    {
                        triangle = new TrianglePotential();
                        passPoint1 = new PointF();
                        passPoint2 = new PointF();
                        mainPoint = new PointF();
                        /*Поиск любого треугольника содержащего точки futureBoardPoints*/
                        for (int j = 0; j < size_triangles; j++)
                        {
                            TrianglePotential tr = new TrianglePotential();
                            tr = triangles[j];
                            if (futureBoardPoint1 == tr.point1.point && futureBoardPoint2 == tr.point2.point && reducentPoint != tr.point3.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point2.point;
                                mainPoint = tr.point3.point;
                            }
                            if (futureBoardPoint1 == tr.point1.point && futureBoardPoint2 == tr.point3.point && reducentPoint != tr.point2.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point2.point;
                            }
                            if (futureBoardPoint1 == tr.point2.point && futureBoardPoint2 == tr.point1.point && reducentPoint != tr.point3.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point2.point;
                                passPoint2 = tr.point1.point;
                                mainPoint = tr.point3.point;
                            }
                            if (futureBoardPoint1 == tr.point2.point && futureBoardPoint2 == tr.point3.point && reducentPoint != tr.point1.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point2.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point1.point;
                            }
                            if (futureBoardPoint1 == tr.point3.point && futureBoardPoint2 == tr.point2.point && reducentPoint != tr.point1.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point3.point;
                                passPoint2 = tr.point2.point;
                                mainPoint = tr.point1.point;
                            }
                            if (futureBoardPoint1 == tr.point3.point && futureBoardPoint2 == tr.point1.point && reducentPoint != tr.point2.point)
                            {

                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point2.point;

                            }
                        }

                        triangle = triangles[num_triangle];

                        A = -DpDx(triangle.point1, triangle.point2, triangle.point3);
                        B = -DpDy(triangle.point1, triangle.point2, triangle.point3);

                        pot.point = pointIntersect;
                        pot2.X = pot.point.X + A;
                        pot2.Y = pot.point.Y + B;



                        //Пробую найти пересечение с однйо из сторон треугольника
                        ka = 0; kb = 0;
                        ka = Ua(mainPoint, passPoint2, pot.point, pot2);
                        kb = Ub(mainPoint, passPoint2, pot.point, pot2);
                        intersectBuf = new PointF();
                        if (ka <= 1 && ka >= 0)
                        {
                            intersectBuf.X = mainPoint.X + ka * (passPoint2.X - mainPoint.X);
                            intersectBuf.Y = mainPoint.Y + ka * (passPoint2.Y - mainPoint.Y);
                            futureBoardPoint1 = mainPoint;
                            futureBoardPoint2 = passPoint2;
                            reducentPoint = passPoint1;

                            pointIntersect = intersectBuf;
                            line.point1 = pot.point;
                            line.point2 = pointIntersect;

                            forceLines.Add(line);

                        }
                        else 
                        {
                            ka = Ua(mainPoint, passPoint1, pot.point, pot2);
                            kb = Ub(mainPoint, passPoint1, pot.point, pot2);

                            if (ka <= 1 && ka >= 0)
                            {
                                intersectBuf.X = mainPoint.X + ka * (passPoint1.X - mainPoint.X);
                                intersectBuf.Y = mainPoint.Y + ka * (passPoint1.Y - mainPoint.Y);
                                futureBoardPoint1 = mainPoint;
                                futureBoardPoint2 = passPoint1;
                                reducentPoint = passPoint2;


                                
                                    pointIntersect = intersectBuf;
                                    line.point1 = pot.point;
                                    line.point2 = pointIntersect;

                                    forceLines.Add(line);
                                
                            }
                        }
                        


                    }

                   
                

            }
            for (int i = 0; i < size_magnet2; ++i)
            {
                Potential pot = magnet2[i];
                PointF reducentPoint = new PointF();
                reducentPoint = pot.point;

               // if (pot.value > 0)
               // {
                    int num_triangle = 0;
                    TrianglePotential triangle = new TrianglePotential();
                    PointF passPoint1 = new PointF();
                    PointF passPoint2 = new PointF();
                    PointF mainPoint = new PointF();
                    PointF pointIntersect = new PointF(); //точка пересечения
                    /*Поиск любого треугольника на границе магнита содержащего точку pot*/
                    for (int j = 0; j < size_triangles; j++)
                    {
                        if (pot.point == triangles[j].point1.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point2.point;
                            mainPoint = triangles[j].point3.point;
                            break;
                        }
                        if (pot.point == triangles[j].point2.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point1.point;
                            mainPoint = triangles[j].point3.point;
                            break;
                        }
                        if (pot.point == triangles[j].point3.point)
                        {
                            num_triangle = j;
                            passPoint1 = pot.point;
                            passPoint2 = triangles[j].point2.point;
                            mainPoint = triangles[j].point1.point;
                            break;
                        }
                    }
                    triangle = triangles[num_triangle];

                    float A = -DpDx(triangle.point1, triangle.point2, triangle.point3);
                    float B = -DpDy(triangle.point1, triangle.point2, triangle.point3);

                    //крайняя точка градиента
                    pointIntersect.X = (passPoint1.X + passPoint2.X) / 2;
                    pointIntersect.Y = (passPoint1.Y + passPoint2.Y) / 2;
                    pot.point = pointIntersect;
                    PointF pot2 = new PointF { X = pot.point.X + A, Y = pot.point.Y + B };


                    LevelLines line = new LevelLines();

                    PointF futureBoardPoint1 = new PointF(); //точки стороны, с которой будет пересечение градиента
                    PointF futureBoardPoint2 = new PointF();

                    

                    //Пробую найти пересечение с однйо из сторон треугольника
                    float ka = 0, kb = 0;
                    ka = Ua(mainPoint, passPoint2, pot.point, pot2);
                    kb = Ub(mainPoint, passPoint2, pot.point, pot2);
                    PointF intersectBuf = new PointF();
                    if (ka <= 1 && ka >= 0)
                    {
                        intersectBuf.X = mainPoint.X + ka * (passPoint2.X - mainPoint.X);
                        intersectBuf.Y = mainPoint.Y + ka * (passPoint2.Y - mainPoint.Y);
                        futureBoardPoint1 = mainPoint;
                        futureBoardPoint2 = passPoint2;
                        reducentPoint = passPoint1;

                        pointIntersect = intersectBuf;
                        line.point1 = pot.point;
                        line.point2 = pointIntersect;

                        forceLines.Add(line);
                    }
                    else
                    {
                        ka = Ua(mainPoint, passPoint1, pot.point, pot2);
                        kb = Ub(mainPoint, passPoint1, pot.point, pot2);

                        if (ka <= 1 && ka >= 0)
                        {
                            intersectBuf.X = mainPoint.X + ka * (passPoint1.X - mainPoint.X);
                            intersectBuf.Y = mainPoint.Y + ka * (passPoint1.Y - mainPoint.Y);
                            futureBoardPoint1 = mainPoint;
                            futureBoardPoint2 = passPoint1;
                            reducentPoint = passPoint2;

                            pointIntersect = intersectBuf;
                            line.point1 = pot.point;
                            line.point2 = pointIntersect;

                            forceLines.Add(line);

                        }
                    }


                    for (int z = 0; z < 200; z++)
                    {
                        triangle = new TrianglePotential();
                        passPoint1 = new PointF();
                        passPoint2 = new PointF();
                        mainPoint = new PointF();
                        /*Поиск любого треугольника содержащего точки futureBoardPoints*/
                        for (int j = 0; j < size_triangles; j++)
                        {
                            TrianglePotential tr = new TrianglePotential();
                            tr = triangles[j];
                            if (futureBoardPoint1 == tr.point1.point && futureBoardPoint2 == tr.point2.point && reducentPoint != tr.point3.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point2.point;
                                mainPoint = tr.point3.point;
                            }
                            if (futureBoardPoint1 == tr.point1.point && futureBoardPoint2 == tr.point3.point && reducentPoint != tr.point2.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point2.point;
                            }
                            if (futureBoardPoint1 == tr.point2.point && futureBoardPoint2 == tr.point1.point && reducentPoint != tr.point3.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point2.point;
                                passPoint2 = tr.point1.point;
                                mainPoint = tr.point3.point;
                            }
                            if (futureBoardPoint1 == tr.point2.point && futureBoardPoint2 == tr.point3.point && reducentPoint != tr.point1.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point2.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point1.point;
                            }
                            if (futureBoardPoint1 == tr.point3.point && futureBoardPoint2 == tr.point2.point && reducentPoint != tr.point1.point)
                            {
                                num_triangle = j;
                                passPoint1 = tr.point3.point;
                                passPoint2 = tr.point2.point;
                                mainPoint = tr.point1.point;
                            }
                            if (futureBoardPoint1 == tr.point3.point && futureBoardPoint2 == tr.point1.point && reducentPoint != tr.point2.point)
                            {

                                num_triangle = j;
                                passPoint1 = tr.point1.point;
                                passPoint2 = tr.point3.point;
                                mainPoint = tr.point2.point;

                            }
                        }

                        triangle = triangles[num_triangle];

                        A = -DpDx(triangle.point1, triangle.point2, triangle.point3);
                        B = -DpDy(triangle.point1, triangle.point2, triangle.point3);

                        pot.point = pointIntersect;
                        pot2.X = pot.point.X + A;
                        pot2.Y = pot.point.Y + B;



                        //Пробую найти пересечение с однйо из сторон треугольника
                        ka = 0; kb = 0;
                        ka = Ua(mainPoint, passPoint2, pot.point, pot2);
                        kb = Ub(mainPoint, passPoint2, pot.point, pot2);
                        intersectBuf = new PointF();
                        if (ka <= 1 && ka >= 0)
                        {
                            intersectBuf.X = mainPoint.X + ka * (passPoint2.X - mainPoint.X);
                            intersectBuf.Y = mainPoint.Y + ka * (passPoint2.Y - mainPoint.Y);
                            futureBoardPoint1 = mainPoint;
                            futureBoardPoint2 = passPoint2;
                            reducentPoint = passPoint1;

                            pointIntersect = intersectBuf;
                            line.point1 = pot.point;
                            line.point2 = pointIntersect;

                            forceLines.Add(line);

                        }
                        else
                        {
                            ka = Ua(mainPoint, passPoint1, pot.point, pot2);
                            kb = Ub(mainPoint, passPoint1, pot.point, pot2);

                            if (ka <= 1 && ka >= 0)
                            {
                                intersectBuf.X = mainPoint.X + ka * (passPoint1.X - mainPoint.X);
                                intersectBuf.Y = mainPoint.Y + ka * (passPoint1.Y - mainPoint.Y);
                                futureBoardPoint1 = mainPoint;
                                futureBoardPoint2 = passPoint1;
                                reducentPoint = passPoint2;



                                pointIntersect = intersectBuf;
                                line.point1 = pot.point;
                                line.point2 = pointIntersect;

                                forceLines.Add(line);

                            }
                        }



                    }


              //  }

            }
            return forceLines;
        }

        private bool IsPointOnLine(PointF p1, PointF p2, PointF point_test)
        {
            float scalar = (p2.X - p1.X) * (point_test.X - p1.X) + (p2.Y - p1.Y) * (point_test.Y - p1.Y);
            if (scalar > 1e-7) return true;
            else return false;
        }

        /*Угловой коэффициент прямой одной из прямых*/
        private float Ua(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float result = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) /
                            ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));
            return result;
        }
        private float Ub(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float result = -((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) /
                            ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));
            return result;
        }
    }
}
