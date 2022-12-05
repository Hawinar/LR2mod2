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
            Console.WriteLine($"Метод кубической аппроксимации: {cubicApproximation(a, b, eps).Item1}\nИтераций: {cubicApproximation(a, b, eps).Item2}");
        }

        static double function(double x)
        {
            return Math.Pow(Math.Sin(x), 2) + Math.Pow(Math.E, x); //вар22
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
        static (double,int) cubicApproximation(double a, double b, double eps)
        {
            //гайд: https://life-prog.ru/2_60886_algoritm-metoda-sredney-tochki.html
            double pol = 0;
            int k = 0;
            double tmp = 0;
            while (true)
            {
                k++;
                //Шаг 1
                double z = functionDerivative(a) + functionDerivative(b) - 3 * (function(b) - function(a)) / (b - a);
                double w = Math.Sqrt(Math.Pow(z, 2) - functionDerivative(a) * functionDerivative(b));
                double u = (w + z - functionDerivative(a)) / (2 * w - functionDerivative(a) + functionDerivative(b));
                pol = a + u * (b - a);
                
                //Шаг 2
                if (Math.Abs(tmp - functionDerivative(a) * functionDerivative(pol)) < eps)
                    break;
                else
                {
                    //Шаг 3
                    if ((functionDerivative(a) * functionDerivative(pol)) < 0)
                        b = pol;
                    else
                        a = pol;
                }
                tmp = (functionDerivative(a) * functionDerivative(pol));                
            }
            return (pol,k);
        }

    }
}
