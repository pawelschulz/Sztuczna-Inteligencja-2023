using Projekt1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1
{
    class Harmonogram
    {
        public List<Procesor> ListaProcesorów { get; set; }
        Random random = new Random();                   // wylosuj zmienną random

        // tworzy k-procesorów w danym harmonogramie
        public void Inicjalizuj(int k, List<Zadanie> listaZadań = null)
        {
            ListaProcesorów = new List<Procesor>();         // tworzę ListęProcesorów
            for (int i = 0; i < k; i++)
            {
                ListaProcesorów.Add(new Procesor());        // dodaję do listy k-procesorów
            }
            if (listaZadań != null)
            {
                foreach (var zadanie in listaZadań)         // każde zadanie przydzielane jest do procesora
                {
                    if (!zadanie.JużPrzydzielone)
                        PrzydzielZadanie(zadanie);          // przydziel zadanie do procesora
                }
            } 
        }

        // metoda wymienia zadania pomiędzy dwoma losowo wybranymi procesorami
        internal Harmonogram Wymień(Harmonogram harmonogramPoczątkowy, Random random)
        {
            // Tworzenie głębokiej kopii obecnego harmonogramu
            Harmonogram nowyHarmonogram = Kopiowanie();

            Zadanie zadanieprocesora1 = null;
            Zadanie zadanieprocesora2 = null;
            int indexzadaniaprocesora1 = 0;
            int indexzadaniaprocesora2 = 0;
            Procesor procesor1 = null;
            Procesor procesor2 = null;

            while (procesor1 == procesor2 || zadanieprocesora1 == zadanieprocesora2)
            {
                zadanieprocesora1 = null;
                zadanieprocesora2 = null;

                // losowy wybór 2 procesorów z danego harmonogramu
                procesor1 = nowyHarmonogram.ListaProcesorów[random.Next(nowyHarmonogram.ListaProcesorów.Count)];
                procesor2 = nowyHarmonogram.ListaProcesorów[random.Next(nowyHarmonogram.ListaProcesorów.Count)];

                // losowy wybór 2 zadań z wcześniej wybranych procesorów
                indexzadaniaprocesora1 = random.Next(procesor1.ListaZadań.Count + 1);
                indexzadaniaprocesora2 = random.Next(procesor2.ListaZadań.Count + 1);   // argument 0 jest niedopuszczalny

                // przypisanie wylosowanych Zadań do zmiennych
                if (procesor1.ListaZadań.Count != 0 && indexzadaniaprocesora1 != procesor1.ListaZadań.Count)
                    zadanieprocesora1 = procesor1.ListaZadań[indexzadaniaprocesora1];
                if (procesor2.ListaZadań.Count != 0 && indexzadaniaprocesora2 != procesor2.ListaZadań.Count)
                    zadanieprocesora2 = procesor2.ListaZadań[indexzadaniaprocesora2];
            }

            // pobranie perwszego zadania z cyklu zadań w relacji, dla każdego procesora
            if (zadanieprocesora1 != null)
            {
                zadanieprocesora1 = PodajPierwsze(zadanieprocesora1); // rekurencyjne pobranie ostatniego poprzednika
                indexzadaniaprocesora1 = zadanieprocesora1.IndexOnProcessor;
            }

            if (zadanieprocesora2 != null)
            {
                zadanieprocesora2 = PodajPierwsze(zadanieprocesora2); // rekurencyjne pobranie ostatniego poprzednika
                indexzadaniaprocesora2 = zadanieprocesora2.IndexOnProcessor;
            }

            // stworzenie list zadań do przeniesienia między procesorami
            List<Zadanie> listazadańwrelacji1 = new List<Zadanie>();
            List<Zadanie> listazadańwrelacji2 = new List<Zadanie>();

            // dodanie do listy poprzedników wylosowanego zadania (jeśli istnieją) i tegoż zadania
            // każde dodane do listy zadanie jest usuwane z procesora
            if (zadanieprocesora1 != null)
            {
                while (zadanieprocesora1.Następne != null)
                {
                    listazadańwrelacji1.Add(zadanieprocesora1);
                    procesor1.ListaZadań.Remove(zadanieprocesora1);
                    zadanieprocesora1 = zadanieprocesora1.Następne;
                }
                listazadańwrelacji1.Add(zadanieprocesora1);
                procesor1.ListaZadań.Remove(zadanieprocesora1);

            }

            if (zadanieprocesora2 != null)
            {
                while (zadanieprocesora2.Następne != null)
                {
                    listazadańwrelacji2.Add(zadanieprocesora2);
                    procesor2.ListaZadań.Remove(zadanieprocesora2);
                    zadanieprocesora2 = zadanieprocesora2.Następne;

                }
                listazadańwrelacji2.Add(zadanieprocesora2);
                procesor2.ListaZadań.Remove(zadanieprocesora2);

            }
            // wypisanie listy zadań do wymiany dla każdego z procesorów
            Console.WriteLine($"Lista zadań do wymiany z procesora1: ");
            foreach (Zadanie item in listazadańwrelacji1)
                Console.WriteLine(item.Index);
            Console.WriteLine($"Lista zadań do wymiany z procesora2: ");
            foreach (Zadanie item in listazadańwrelacji2)
                Console.WriteLine(item.Index);

            // dodanie list zadań do wymiany na początku list zadań procesorów
            procesor1.ListaZadań.InsertRange(0, listazadańwrelacji2);
            procesor2.ListaZadań.InsertRange(0, listazadańwrelacji1);

            // Aktualizacja danych dla zadań na procesorach po podmianie
            for (int i = 0; i < procesor1.ListaZadań.Count; i++)
            {
                procesor1.ListaZadań[i].Rozpoczęcie = i == 0 ? 0 : procesor1.ListaZadań[i - 1].Koniec();
                procesor1.ListaZadań[i].IndexOnProcessor = i;
            }
            for (int i = 0; i < procesor2.ListaZadań.Count; i++)
            {
                procesor2.ListaZadań[i].Rozpoczęcie = i == 0 ? 0 : procesor2.ListaZadań[i - 1].Koniec();
                procesor2.ListaZadań[i].IndexOnProcessor = i;
            }
            return nowyHarmonogram;
        }

        // metoda tworząca kopię harmonogramu
        private Harmonogram Kopiowanie()
        {
            Harmonogram schedule = new Harmonogram();
            schedule.Inicjalizuj(this.ListaProcesorów.Count);       // inicjalizacja harmonogramu o odpowiedniej liczbie procesorów

            for (int i = 0; i < this.ListaProcesorów.Count; i++)
            {
                for (int j = 0; j < this.ListaProcesorów[i].ListaZadań.Count; j++)
                {
                    Zadanie newJob = new Zadanie(ListaProcesorów[i].ListaZadań[j]);
                    schedule.ListaProcesorów[i].ListaZadań.Add(newJob);
                    newJob.Procesor = schedule.ListaProcesorów[i];              // aktualizacja wskaźnika na procesor
                }
            }
            for (int i = 0; i < schedule.ListaProcesorów.Count; i++)
            {
                for (int j = 0; j < schedule.ListaProcesorów[i].ListaZadań.Count; j++)
                {
                    // Jeśli w orginalnym harmonogramie był poprzednik, przypisz odpowiednie zadania jako poprzednik i następnik w skopiowanym harmonogramie
                    if (this.ListaProcesorów[i].ListaZadań[j].Poprzednie != null)
                    {
                        schedule.ListaProcesorów[i].ListaZadań[j].Poprzednie = schedule.ListaProcesorów[i].ListaZadań[j - 1];
                        schedule.ListaProcesorów[i].ListaZadań[j - 1].Następne = schedule.ListaProcesorów[i].ListaZadań[j];

                    }
                }
            }
            return schedule;
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
