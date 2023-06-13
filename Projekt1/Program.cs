using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Globalization;

namespace Projekt1
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

                // Inicjalizacja harmonogramu początkowego, tworzenie obiektu harmonogram
                Harmonogram harmonogram = new Harmonogram();
                harmonogram.Inicjalizuj(k, ListaZadań);

                // parametry algorytmu
                double T = 100;
                double T_end = 0.01;
                double alpha = 0.97;
                double i = 0;

                // Losowy wybór punktu startowego dzięki zmiennej losowej
                Random random = new Random();   

                // pętla będzie wykonywać się tak długo, aż warunek stopu spełniony
                while (T > T_end)          
                {
                    // Wygeneruj nową konfigurację rozwiązania, wykonując losowe operacje na obecnym rozwiązaniu.
                    Harmonogram nowyHarmonogram = new Harmonogram();
                    nowyHarmonogram = harmonogram.Wymień(harmonogram, random);
          
                    // zaakceptuj nowe rozwiązanie, jeśli czas nowego < czas obecnego
                    if (nowyHarmonogram.MaxCzas() < harmonogram.MaxCzas() )
                    {
                        harmonogram = nowyHarmonogram;
                    }
                    else
                    {
                        // Jeśli nowyHarmonogram jest gorszy, zaakceptuj go z pewnym prawdopodobieństwem
                        double deltaCzas = nowyHarmonogram.MaxCzas() - harmonogram.MaxCzas();
                        double akceptacja = Math.Exp(-deltaCzas / T);
                        if (akceptacja > random.NextDouble())
                        {
                            harmonogram = nowyHarmonogram;
                        }
                    }

                    if (i % 10 == 0)
                    {
                        T *= alpha; // studzenie
                    }
                    i++;
                    Console.WriteLine($"Czas: {harmonogram.MaxCzas()}");
                    harmonogram.Wyświetl();
                }
                Console.WriteLine("==========================================================");
                Console.WriteLine($"Najlepszy: {harmonogram.MaxCzas()}");
                harmonogram.Wyświetl();
            }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }
    }
}

