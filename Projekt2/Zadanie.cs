using Projekt2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt2
{
    public class Zadanie
    {
        public int Index;                   // numer początkowy zadania
        public Procesor? Procesor = null;           // na którym procesorze się wykona
        public int Rozpoczęcie;             // czas rozpoczęcia zadania
        public int Waga;                    // czas potrzebny na wykonanie zadania
        public Zadanie? Poprzednie = null;         // poprzednie zadanie
        public Zadanie? Następne = null;
        public int PoprzednieIndex = 0;     // numer początkowy poprzedniego zadania
        public bool JużPrzydzielone = false;
        public int IndexOnProcessor { get; set; } // indeks zadania w liście zadań procesora

        //public bool NastępneIstnieje = false; // czy zadanie ma się wykonać przed innym

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
                //listazadań.Add(new Zadanie() { Waga = int.Parse(tablica1[i]), Index = i});

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

        public Zadanie()
        {

        }

        public int Koniec()
        {
            return Rozpoczęcie + Waga;
        }

    }
}
