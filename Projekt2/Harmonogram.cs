using Projekt2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    class Harmonogram
    {
        public List<Procesor> ListaProcesorów { get; set; }
        Random random = new Random();                   // wylosuj zmienną random

        public Harmonogram(int k)
        {
            ListaProcesorów = new List<Procesor>();
            for (int i = 0; i < k; i++)
            {
                ListaProcesorów.Add(new Procesor());
            }
        }

        // rekurencyjna metoda pomocnicza, zwraca pierwsze zadanie w sekwencji
        private Zadanie PodajPierwsze(Zadanie job)
        {
            if (job.Poprzednie == null) return job;

            return PodajPierwsze(job.Poprzednie); // rekurencyjne pobieranie ostatniego poprzednika
        }

        // metda przydzielająca zadania do procesora
        private void PrzydzielZadanie(Zadanie zadanie)
        {
            if (!zadanie.JużPrzydzielone)                       // jeśli już przydzielone, nie wykonuj działań
            {
                if (zadanie.Poprzednie != null)                 // jeśli zadanie posiada poprzednika, przydziel pierw poprzednika
                {
                    PrzydzielZadanie(zadanie.Poprzednie);
                }
                var random = new Random();
                if (zadanie.Poprzednie != null)
                {
                    zadanie.Poprzednie.Procesor.DodajZadanie(zadanie);      // jeśli zadanie ma poprzednika, dodaj zadanie do listy zadań procesora, do którego należy poprzednie zadanie
                    zadanie.Poprzednie.Następne = zadanie;                  // ustaw zadanie jako następnik poprzedniego zadania
                }
                else
                {
                    ListaProcesorów[random.Next(0, ListaProcesorów.Count)].DodajZadanie(zadanie);       // jeśli zadanie nie ma poprzednika, losowo wybierz jeden z procesorów i dodaj zadanie
                }
            }
        }

        // prywatna metoda usuwająca zadanie z określonego procesora i wszystkich jego poprzedników
        private void UsuńZadanie(Zadanie zadanie, int nrProcesora)
        {
            if (zadanie.PoprzednieIndex != -1)                // jeśli zadanie posiada poprzednika, przydziel pierw poprzednika
            {
                this.UsuńZadanie(zadanie.Poprzednie, nrProcesora);
            }
            Random random = new Random();
            this.ListaProcesorów[nrProcesora].UsuńZadanie(zadanie);
        }

        // obliczenie maksymalnego czasu zakończenia pracy w danym harmonogramie
        internal int MaxCzas()
        {
            int max = int.MinValue;
            foreach (var procesor in ListaProcesorów)
            {
                foreach (var job in procesor.ListaZadań)
                {
                    if (job.Koniec() > max)
                    {
                        max = job.Koniec();
                    }
                }
            }
            return max;
        }

        // metda ta przechodzi po wszystkich procesorach i zadaniach, wyświetla indeksy zadań oraz ich wagi
        internal void Wyświetl()
        {
            for (int i = 0; i < ListaProcesorów.Count; i++)
            {
                Console.Write($"Procesor nr {i}:|");
                for (int j = 0; j < ListaProcesorów[i].ListaZadań.Count; j++)
                {
                    Console.Write($"J{ListaProcesorów[i].ListaZadań[j].Index}:");
                    for (int m = 0; m < ListaProcesorów[i].ListaZadań[j].Waga; m++)
                    {
                        Console.Write("*");
                    }
                    Console.Write("|");
                }
                Console.WriteLine("");
            }
            Console.WriteLine($"Czas zakończenia pracy: {this.MaxCzas()}");
        }
    }
}