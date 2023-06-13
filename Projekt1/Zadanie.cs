using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1
{
    public class Zadanie
    {
        // dane pojedynczego zadania
        public int Index;                   // numer początkowy zadania
        public Procesor? Procesor = null;   // na którym procesorze się wykona (przypisany obiekt procesora)
        public int Rozpoczęcie;             // czas rozpoczęcia zadania
        public int Waga;                    // czas potrzebny na wykonanie zadania
        public Zadanie? Poprzednie = null;  // poprzednie zadanie (przypisany obiekt zadania)
        public Zadanie? Następne = null;    // następne zadanie (przypisany obiekt zadania)
        public int PoprzednieIndex = -1;    // indeks początkowy poprzedniego zadania
        public bool JużPrzydzielone = false;// flaga, czy zadanie przydzielone do procesora
        public int IndexOnProcessor { get; set; } // indeks zadania w liście zadań procesora

        // f-cja wczytująca zadania z pliku
        internal static List<Zadanie> WczytajZadania(StreamReader sr)
        {
            int n, no;
            n = int.Parse(sr.ReadLine());
            no = int.Parse(sr.ReadLine());
            string tekst1 = sr.ReadLine();
            string[] tablica1 = tekst1.Split(' ');

            List<Zadanie> listazadań = new List<Zadanie>();
            for (int i = 0; i < n; i++)
            {
                Zadanie nowe = new Zadanie();
                nowe.Waga = int.Parse(tablica1[i]);
                nowe.Index = i;
                listazadań.Add(nowe);
            }
            for (int i = 0; i < no; i++)
            {
                string tekst2 = sr.ReadLine();
                string[] tablica2 = tekst2.Split(' ');
                int przed = int.Parse(tablica2[0]);
                int po = int.Parse(tablica2[1]);

                //listazadań[poprzednie].NastępneIstnieje = true;
                listazadań[po].Poprzednie = listazadań[przed];
                listazadań[po].PoprzednieIndex = przed;
                listazadań[przed].Następne = listazadań[po];
            }
            return listazadań;
        }

        public Zadanie(Zadanie zadanie)         // konstruktor kopiujący dla klasy Zadanie
        {
            Waga = zadanie.Waga;                // przypisanie do zadania wagi
            Index = zadanie.Index;
            Procesor = zadanie.Procesor;        // na którym procesorze się wykona
            Rozpoczęcie = zadanie.Rozpoczęcie;  // czas rozpoczęcia zadania
            Poprzednie = zadanie.Poprzednie;    // poprzednie zadanie
            Następne = zadanie.Następne;
            PoprzednieIndex = zadanie.PoprzednieIndex;     // numer początkowy poprzedniego zadania
            JużPrzydzielone = zadanie.JużPrzydzielone;
            IndexOnProcessor = zadanie.IndexOnProcessor;
        }

        public Zadanie() {}

        // f-cja zwraca czas zakończenia zadania
        public int Koniec()
        {
            return Rozpoczęcie + Waga;
        }

    }
}
