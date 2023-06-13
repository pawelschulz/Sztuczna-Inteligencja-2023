// Ten kod implementuje prosty model probabilistyczny, który estymuje
//umiejętności graczy na podstawie wyników gier.

using System;
using System.Linq;
using Microsoft.ML.Probabilistic;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic.Distributions;

namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tworzone są dane dotyczące zwycięzcy i przegranego w każdej z 6 próbek gier
            var winnerData = new[] { 0, 0, 0, 1, 3, 4 };
            var loserData = new[] { 1, 3, 4, 2, 1, 2 };

            // Definiowany jest model statystyczny jako program probabilistyczny.
            // Tworzone są zakresy (Ranges) dla gier i graczy
            var game = new Microsoft.ML.Probabilistic.Models.Range(winnerData.Length);
            var player = new Microsoft.ML.Probabilistic.Models.Range(winnerData.Concat(loserData).Max() + 1);

            // Tworzony jest tablica zmiennych reprezentujących umiejętności graczy (playerSkills),
            var playerSkills = Variable.Array<double>(player);

            // generowane są różne rozkłady a priori dla każdego gracza
            for (int i = 0; i < playerSkills.SizeAsInt; i++)
            {
                double mean = i * 2;
                double variance = i * 3;
                playerSkills[i] = Variable.GaussianFromMeanAndVariance(mean, variance);
            }
            // Każdy gracz ma przypisaną zmienną playerSkills[i], której rozkład a priori jest gaussowski
            // z różnymi wartościami oczekiwanymi (mean) i wariancjami (variance) w zależności od indeksu gracza.

            // Tworzone są tablice zmiennych winners i losers, które przechowują informacje o zwycięzcach i przegranych w poszczególnych grach
            var winners = Variable.Array<int>(game);
            var losers = Variable.Array<int>(game);

            // W obrębie pętli dla każdej gry definiowane są zmienne reprezentujące wyniki graczy (winnerPerformance i loserPerformance) jako wersje ich umiejętności
            using (Variable.ForEach(game))
            {
                var winnerPerformance = Variable.GaussianFromMeanAndVariance(playerSkills[winners[game]], 1.0);
                var loserPerformance = Variable.GaussianFromMeanAndVariance(playerSkills[losers[game]], 1.0);

                // zwycięzcy nie chcemy karać za wygraną, nie można obiżać rankingu dobrego gracza przez to, że wygrał ze słabym
                // Dodawane jest ograniczenie, które mówi, że zwycięzca ma wyższą wydajność niż przegrany w każdej grze
                Variable.ConstrainTrue(winnerPerformance > loserPerformance);
            }

            // Dane są przypisywane do modelu
            winners.ObservedValue = winnerData;
            losers.ObservedValue = loserData;

            // Uruchamiana jest inferencja, która estymuje umiejętności graczy na podstawie dostępnych danych
            var inferenceEngine = new InferenceEngine();
            var inferredSkills = inferenceEngine.Infer<Gaussian[]>(playerSkills);
            // estymacja - funkcja służąca do oszacowania nieznanego parametru badanej populacji

            // Wyniki inferencji są niepewne i zawierają informacje o wariancjach
            // Umiejętności graczy są sortowane według wartości średnich rozkładów a posteriori, a następnie są wyświetlane
            var orderedPlayerSkills = inferredSkills.Select((s, i) => new { Player = i, Skill = s }).OrderByDescending(ps => ps.Skill.GetMean());

            foreach (var playerSkill in orderedPlayerSkills)
            {
                Console.WriteLine($"Player {playerSkill.Player} skill: {playerSkill.Skill}");
            }
        }
    }
}

// Wyświetlane są indeksy graczy oraz ich umiejętności oszacowane na podstawie dostępnych danych

/*
Player 4 skill: N(8, 12)
Player 3 skill: N(6, 9)
Player 2 skill: N(4, 6)
Player 1 skill: N(2, 3)
Player 0 skill: N(0, 0)
Player 5 skill: N(10, 15)
*/

// Wyniki wyświetlane są w kolejności malejącej na podstawie wartości średnich rozkładów a posteriori
// umiejętności graczy. Każda umiejętność jest reprezentowana przez rozkład normalny (Gaussian),
// gdzie N(x, y) oznacza rozkład normalny o średniej x i wariancji y.
