using Projekt2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt2
{
    public class Procesor
    {
        // lista zadań przypisanych do procesora
        public List<Zadanie> ListaZadań { get; set; }

        // Konstruktor, inicjalizuje psutą listę
        public Procesor()
        {
            ListaZadań = new List<Zadanie>();
        }

        // Metoda dodająca zadanie do procesora
        public void DodajZadanie(Zadanie zadanie)
        {
            // pierwsze zadanie na liście rozpoczyna sekwencję czasu - przypisane 0
            if (ListaZadań.Count == 0)
            {
                zadanie.Rozpoczęcie = 0;
            }
            else
            {
                zadanie.Rozpoczęcie = ListaZadań[ListaZadań.Count - 1].Koniec();
            }
            ListaZadań.Add(zadanie);
            zadanie.JużPrzydzielone = true;     // ustawienie odpowiednich pól dodanego zadania
            zadanie.Procesor = this;
            zadanie.IndexOnProcessor = ListaZadań.Count - 1;
        }

        // Metoda usuwająca zadanie z procesora
        internal void UsuńZadanie(Zadanie zadanie)
        {
            ListaZadań.Remove(zadanie);
            zadanie.JużPrzydzielone = false;
        }
  
        public int EndTime()
        {
            return ListaZadań.Count > 0 ? ListaZadań[ListaZadań.Count - 1].Koniec() : 0;
        }
    }
}