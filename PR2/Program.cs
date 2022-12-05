using System;
using System.Collections.Generic;

namespace PR2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double eps = 0.001;
            double a = -1;
            double b = 1;

            Console.WriteLine($"Метод средней точки: {Midpoint(a, b, eps).Item1}\nИтераций: {Midpoint(a, b, eps).Item2}");
            Console.WriteLine($"Метод хорд: {Secant(a, b, eps).Item1}\nИтераций: {Secant(a, b, eps).Item2}");
            Console.WriteLine($"Метод Ньютона: {Newton(a, b, eps).Item1}\nИтераций: {Newton(a, b, eps).Item2}");
            Console.WriteLine($"Метод кубической аппроксимации: {cubicApproximation(a, b, eps)}");
        }

        static double function(double x)
        {
            return Math.Pow(Math.Sin(x), 2) + Math.Pow(Math.E, x); //вар22
            //return (1 / Math.Log(x, Math.E)) + Math.Pow(x, 2);
            //return Math.Pow((x + 1), (x / 2)) + Math.Pow((10 - x), 2); //Функция из примера (для проверки)
        }
        static double functionDerivative(double x) //Производная функции
        {
            return Math.Sin(2 * x) + Math.Pow(Math.E, x); //вар22
        }
        static double functionDoubleDerivative(double x) //Вторая производная функции
        {
            return 2 * Math.Cos(2 * x) + Math.Pow(Math.E, x); //вар22
        }
        static (double, int) Midpoint(double a, double b, double eps)
        {
            double x = (a + b) / 2;
            double a0 = a;
            double b0 = b;
            int k = 0;
            while (Math.Abs(functionDerivative(x)) > eps)
            {
                k++;
                if (functionDerivative(x) > 0)
                    b0 = x;
                else
                    a0 = x;
                x = (a0 + b0) / 2;
            }
            return (x, k);
        }
        static (double, int) Secant(double a, double b, double eps)
        {
            List<double> x = new List<double>() { a, b };
            int k = x.Count - 1;
            double tmp;
            while (Math.Abs(x[k - 1] - x[k]) > eps)
            {
                double test = Math.Abs(x[k - 1] - x[k]);
                tmp = x[k] - functionDerivative(x[k]) * (x[k - 1] - x[k]) / (functionDerivative(x[k - 1]) - functionDerivative(x[k]));
                x.Add(tmp);
                k++;
            }
            return (x[k], k);
        }
        static (double, int) Newton(double a, double b, double eps)
        {
            //гайд: https://math.stackexchange.com/questions/1900664/using-newton-raphson-method-to-approximate-the-minimum-value-of-the-function

            List<double> x = new List<double>() { 0, a };
            int k = 1;
            double tmp = 0;
            while (Math.Abs(x[k - 1] - x[k]) > eps)
            {
                tmp = x[k] - (functionDerivative(x[k])) / (functionDoubleDerivative(x[k]));
                k++;
                x.Add(tmp);
            }
            return (x[k], k);
        }
        static double cubicApproximation(double a, double b, double eps)
        {
            //https://life-prog.ru/2_60886_algoritm-metoda-sredney-tochki.html
            //https://studfile.net/preview/1467149/page:17/


            {
                //double x0 = a;
                //double x1 = 0;
                //double h = a / 100;
                //double k = 0;
                //int count = 0;
                //while (true)
                //{
                //    if (functionDerivative(x0) > 0)
                //        x1 = x0 + Math.Pow(2, count) * h;
                //    else
                //        x1 = x0 - Math.Pow(2, count) * h;
                //    if (functionDerivative(x1) * functionDerivative(x0) > 0)
                //        break;             //точка min-ма функции пройдена
                //    else
                //        x0 = x1;
                //    count++;
                //}

                //if (functionDerivative(x0) > 0)
                //{
                //    a = x0;
                //    b = x1;
                //}
                //else
                //{
                //    a = x1;
                //    b = x0;
                //}

                //while (true)
                //{
                //    double z = (3 * function(a) - function(b)) / (b - a) + functionDerivative(a) + functionDerivative(b);
                //    double w = Math.Sqrt(Math.Pow(z, 2) - functionDerivative(a) * functionDerivative(b));
                //    double y = (z + w - functionDerivative(a)) / (functionDerivative(b) - functionDerivative(a) + 2 * w);

                //    double polynom = a + y * (b - a);

                //    if (functionDerivative(polynom) < 0)
                //        a = polynom;
                //    else
                //        b = polynom;
                //    if (Math.Abs(b - a) < eps)
                //        break;
                //}
                //double x = (b + a) / 2;
            }
            double x1 = a;
            double x2 = b;

            double x = 0;

            double a1 = (function(x2)- function(x1))/(x2-x1);
            double a2 = (function(x2) - function(x1)) / (Math.Pow((x2 - x1), 2)) - function(x1)/(x2-x1);
            double a3 = (function(x2) + function(x1)) / (Math.Pow((x2 - x1), 2)) - (function(x2) - function(x1)) / (Math.Pow((x2 - x1), 3));

            double H(double x)
            {
                return function(x1)+a1*(x-x1)+a2*(x-x1)*(x-x2)+a3*Math.Pow(x-x1,2)*(x-x2);
            }

            bool x1f1 = H(x1) == function(x1);
            bool x2f2 = H(x2) == function(x2);

            while (Math.Abs(b-a)>eps)
            {
                double z = functionDerivative(x1) + functionDerivative(x2) - 3 * (function(x2) - function(x1)) / (x2 - x1);
                double w = Math.Sqrt(Math.Pow(z, 2) - functionDerivative(x1) * functionDerivative(x2));
                double u = (w + z - functionDerivative(x1)) / (2 * w - functionDerivative(x1) + functionDerivative(x2));
                x = x1 + u * (x2 - x1);
                if (functionDerivative(x1) * functionDerivative(x) < 0)
                {
                    a = x1;
                    b = x;
                }
                else
                {
                    a = x;
                    b = x2;
                }
            }

            return x;
        }

    }
}
