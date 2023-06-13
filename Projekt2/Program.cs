using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Globalization;
using Projekt2;

namespace Projekt2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Podaj ilość procesorów: ");
            int k = int.Parse(Console.ReadLine());
            //int k = 3;
            try
            {
                // wczytanie danych wejściowych z pliku harm.in
                StreamReader sr = new StreamReader("harm.in");

                List<Zadanie> ListaZadań = Zadanie.WczytajZadania(sr);

                // wyświetlanie wczytanych danych
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Wczytałem następujące dane: ");
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine($"Ilość prac: {ListaZadań.Count}, ilość procesorów: {k}.");
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Dane prac: ");
                for (int j = 0; j < ListaZadań.Count; j++)
                {
                    Console.WriteLine($"Waga pracy nr {j}:                             {ListaZadań[j].Waga}");
                    if (ListaZadań[j].Poprzednie != null)
                    {
                        Console.WriteLine($"   => Zadanie nr {ListaZadań[j].Index} posiada poprzednie zadanie(a)");
                    }
                }
                Console.WriteLine("-----------------------------------------------------------------------");

                Genetic ga = new Genetic(ListaZadań, k);
                Harmonogram najlepszyharmonogram = ga.FindBestSchedule();

                Console.WriteLine("Najbardziej optymalny harmonogram: ");
                najlepszyharmonogram.Wyświetl();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}



