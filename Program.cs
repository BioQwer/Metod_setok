using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Решение_уравнений_с_Частными_производными_Методом_Сеток
{
    class Program
    {
        //Метод isOutOfBorder работает правильно код для проверки 
        //Console.WriteLine(isOutOfBorder(0.5,1.2));
        //Console.WriteLine(isOutOfBorder(-100, 1.2));
        //Проверка границ
        //Console.WriteLine(isOutOfBorder(0, 2 * Math.Log(2)));
        //Console.WriteLine(isOutOfBorder(0,4));
        private static bool isOutOfBorder(double x, double y)   //если вышел за границу false 
        {
            if ((y > 4 * Math.Pow(Math.E, -x)) || (y < 1.0) || (x < 0.0))  //условие моего варианта
                return true;
            else
                return false;
        }

        private static bool isNeedBorderNode(double x, double y, double h)   //если нужен граничный узел для заполнения сетки
        {
            if ((y > 4 * Math.Pow(Math.E, -x) + h) || (y < 1.0 - h) || (x < 0.0 - h))
                return false;
            else
                return true;
        }


        public static double f(double x, double y)    //заданая граничная функция 
        {
            return x * x + y * y;
        }

        public static void metod_setok_dirixle(double h, int k)
        {
            double eps = 0.01;
            //double h = (2 * Math.Log(2)) / n;    //мой шаг

            double P = 2 * h;   //ширина прямоугольника 
            //задание контура прямоугольника в котором будем строить сетку
            double a  ;   // 0 - h
            if (k == 1)
                a = -0.1;
            else
                a = 0;
            double b = 2 * Math.Log(2) + P;
            double c = 1 - P;
            double d = 4 + P;
            //Console.WriteLine(" a={0,9:F4}  b={1,10:F4}  c={2,10:F4}  d={3,10:F4}", a, b, c, d);

            int Nx = (int)((b - a) / h);   // n задает колличество итераций и тем самым разбиение сетки по оси X 
            int Ny = (int)((d - c) / h);   // так как равный шаг, то надо узнать размер сетки оси Y
            double[,] U = new double[Ny, Nx];  //создание массива
            //Console.WriteLine("Nx={0,10} Ny={1,10}", Nx, Ny);

            //обнуление массива + заполнение граничных точек
            double x = a;
            double y = c;
            for (int i = 0; i < Nx; i++)
            {
                x = a + i * h;
                for (int j = 0; j < Ny; j++)
                {
                    y = c + j * h;
                    if (isNeedBorderNode(x, y, h))
                        U[j, i] = f(x, y);
                    else
                        U[j, i] = 0.0;
                }
            }

            //заполнение сетки конечными разностями
            x = a;
            y = c;
            for (int i = 1; i < Nx - 1; i++)
            {
                x = a + i * h;
                for (int j = 1; j < Ny - 1; j++)
                {
                    y = c + j * h;
                    if (!isOutOfBorder(x, y))
                        U[j, i] = (U[j, i - 1] + U[j, i + 1] + U[j - 1, i] + U[j + 1, i]) / 4;
                }
            }

            x = a;
            y = c;
            for (int i = 0; i < Ny - 1; i+=k)
                Console.Write("{0,6:F2}",c+i*h);
            Console.WriteLine();
            for (int i = 1; i < Nx - 1; i+=k)
            {
                Console.Write("{0,6:F2}", a + i * h);
                for (int j = 1; j < Ny - 1; j+=k)
                {
                    Console.Write("{0,6:F2}", U[j,i]); 
                }
                Console.Write(Environment.NewLine);
            }

            string path = "F:\\Dropbox\\Visual Studio\\Projects\\Metod_setok\\";
            StreamWriter file = new System.IO.StreamWriter(@"" + path + "file"+Nx+".txt");

            for (int i = 0; i < Nx; i++)
            {
                for (int j = 0; j < Ny; j++)
                {
                    file.Write(U[j, i].ToString(CultureInfo.InvariantCulture) + "   ");
                }
                file.WriteLine();
            }
            file.Close();


        }

        static void Main(string[] args)
        {
            Console.WriteLine("шаг 0.1");
            metod_setok_dirixle(0.01,2);
            Console.WriteLine("шаг 0.2");
            metod_setok_dirixle(0.2, 1);

            Console.WriteLine("Finish!");
            Console.ReadLine();

        }
    }
}
