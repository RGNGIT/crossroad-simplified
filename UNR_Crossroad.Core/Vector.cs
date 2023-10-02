using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNR_Crossroad.Core
{
    // Класс реализации двумерного вектора. Для определения координат или направлений
    public struct Vector
    {
        // Переменные вектора
        public double X;
        public double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
