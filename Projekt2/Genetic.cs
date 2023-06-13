using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace Projekt2
{
    class Genetic
    {
        private List<Zadanie> ListaZadań;
        private int k;

        // konstruktor
        public Genetic(List<Zadanie> jobs, int liczbaprocesorów)
        {
            k = liczbaprocesorów;
            ListaZadań = jobs;
        }

        // metoda znajdzie najlepszy harmonogram
        public Harmonogram FindBestSchedule()
        {
            var population = new Population();
            int PopulationSize = 100;
            List<Zadanie> populationJobs = new List<Zadanie>();

            // wczytanie zadań populacyjnych
            for(int i = 0; i < ListaZadań.Count; i++)
            {
                if (ListaZadań[i].Poprzednie == null)
                {
                    populationJobs.Add(ListaZadań[i]);
                }
            }

            // Tworzenie populacji początkowej, złożónej z chromosomów = harmonogramów
            // Chromosomy są tworzone przez geny = zadania
            for (var p = 0; p < PopulationSize; p++)
            {
                var chromosome = new Chromosome(populationJobs.Count);
                for(int i = 0; i < populationJobs.Count; i++)
                {
                    chromosome.Genes[i] = new Gene(populationJobs[i]);
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            // określenie parametrów algorytmu genetycznego
            var ga = new GeneticAlgorithm(population, CalculateFitness);
            var mutate = new SwapMutate(0.02);
            var elite = new Elite(5);
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //dodanie parametrów
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            // uruchomienie algorytmu 
            ga.Run(Terminate);

            var bestChromosome = ga.Population.GetTop(1)[0];
            Harmonogram bestHarmonogram = DecodeChromosome(bestChromosome);

            return bestHarmonogram;
        }

        // metoda dekoduje chromosom i tworzy najlepszy harmonogramu
        private Harmonogram DecodeChromosome(Chromosome bestChromosome)
        {
            List<Zadanie> jobs = new List<Zadanie>();

            foreach (var gene in bestChromosome.Genes)
            {
                jobs.Add((Zadanie)gene.ObjectValue);
            }

            Harmonogram schedule = NowyHarmonogram(jobs, k);

            return schedule;
        }

        // funckja przystosowania
        public double CalculateFitness(Chromosome chromosome)
        {
            var minTime = CalculateMinTime(chromosome);     // zwraca minimalny czas wykonania harmonogramu

            var fitness = 10 / minTime;
            return fitness > 1.0 ? 1.0 : fitness;
        }

        private double CalculateMinTime(Chromosome chromosome)
        {
            // utworzenie listy zadań na podstawie genów (zadań) chromosomu (harmonogramu)
            List<Zadanie> jobs = new List<Zadanie>();

            foreach (var gene in chromosome.Genes)
            {
                jobs.Add((Zadanie)gene.ObjectValue);
            }

            Harmonogram newSchedule = NowyHarmonogram(jobs, k);

            return newSchedule.MaxCzas();
        }

        // metoda generuje nowy harmonogram
        private Harmonogram NowyHarmonogram(List<Zadanie> jobs, int k)
        {
            var schedule = new Harmonogram(k);      // nowy obiekt harmonogramu

            foreach (var job in jobs)
            {
                Zadanie jobTemp = job;
                Procesor processor = schedule.ListaProcesorów.OrderBy(p => p.EndTime()).FirstOrDefault();
                // dodanie zadań do procesorów w kolejności zgodnej z ich czasami zakończenia

                while (jobTemp != null)
                {
                    processor.DodajZadanie(jobTemp);
                    jobTemp = jobTemp.Następne;
                }
            }
            schedule.Wyświetl();

            return schedule;
        }

        //  warunek zakończenia algorytmu genetycznego
        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 100;
        }
    }
}
