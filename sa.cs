using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace std
{
    class Program
    {
        public static void Main(string[] args)
        {
            int x_min = -10;
            int x_max = 10;
            double T_min = 0.001;
            double alpha = 0.9;
            double num_it = 100;
            int i;

            int x_init = 10;
            double x_current = x_init;
            double x_best = x_init;
            double T = f(x_init);

            while (T > T_min)
            {
                for (i = 0; i < num_it; i++)
                {
                    double x_new =  random(x_min, x_max);     // chosing new x
                    double ac = acc(x_new, x_current, f(x_new));
                    if (ac == 1)
                    {
                        x_current = x_new;
                        if (ac > random(x_min, x_max))
                            x_best = x_current;
                    }
                    T *= alpha; // Cooling Scheme
                }
            }

            System.Console.WriteLine("Y Min: {0} w x = {1}", f(x_best), x_best);
            Console.ReadLine();

            static double acc(double sa, double sb, double T)      //acceptance criterium
            {
                double delta = f(sa) - f(sb);
                if (delta <= 0)
                    return 1;
                else
                    return Math.Exp(-delta / T);         // Math.Exp zwraca double
            }

            static double random(double min, double max)        // generate random double numbers between min and max
            {
                System.Random rand = new System.Random();
                return rand.NextDouble() * (max - min) + min;
            }

            static double f(double s)
            {
                return s * s;
                //return s * s + 1;
                //return (s+2) * (s+2) + 1;
            }
        }
    }
}
