using System;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;

namespace Projekt3
{

    class Program
    {

        static void Main(string[] args)
        {
            // liczba rzutów
            int[] data = new int[100];

            // losowe generowanie rzutów - 0 lub 1
            for (int i = 0; i < data.Length; i++)
                data[i] = new Random().Next(2);

            // gęstość prawdopodobieństwa a priori
            Variable<double> apriori = Variable.Beta(1, 1);

            for (int i = 0; i < data.Length; i++)
            {
                // obserwowaną wartość zmiennej x ustawiona na 1 - true, 0 - false
                Variable<bool> x = Variable.Bernoulli(apriori);
                x.ObservedValue = (data[i] == 1) ? true : false;
            }

            // silnik Infer.NET
            InferenceEngine engine = new InferenceEngine();

            // gęstość pradopodobieństwa a posteriori
            Console.WriteLine("apriori=" + engine.Infer(apriori));
        }
    }
}
