using KnapsackSolver.Models;
using KnapsackSolver.Services;
using System.Threading.Tasks;
using static KnapsackSolver.Services.Evaluator;

namespace KnapsackSolver.Solvers
{
    public static class SimulatedAnnealingSolver
    {
        private static readonly Random r = new();

        public static KnapsackResult Solve( IReadOnlyList<Item> items, int capacity,
                double initialTemp = 1_000.0, 
                double finalTemp = 0.01,
                double alpha = 0.98,
                int iterationsPerTemp = 100)
        {
            int n = items.Count;
            bool[] current;
            EvalResult currentResult;

            do
            {
                current = [.. Enumerable.Range(0, n).Select(_ => r.NextDouble() < 0.5)];
                currentResult = Evaluator.Evaluate(current, items, capacity); 
            }
            while (currentResult.Weight > capacity);

            var best = (bool[])current.Clone();
            var bestVal = currentResult.Value;
            var bestWeight = currentResult.Weight;

            for (double T = initialTemp; T > finalTemp && Experiment.FesCounter < Experiment.MaxFes; T *= alpha)
            {
                for (int it = 0; it < iterationsPerTemp && Experiment.FesCounter < Experiment.MaxFes; it++)
                {
                    var neighbor = (bool[])current.Clone();
                    int idx = r.Next(n);
                    neighbor[idx] = !neighbor[idx];

                    var neighborResult = Evaluator.Evaluate(neighbor, items, capacity);
                    double delta = (double)neighborResult.Value - currentResult.Value;

                    if (delta > 0.0 || r.NextDouble() < Math.Exp(delta / T))
                    {
                        current = neighbor;
                        currentResult = neighborResult; 

                        if (currentResult.Weight <= capacity &&
                            currentResult.Value > bestVal)
                        {
                            best = (bool[])current.Clone();
                            bestVal = currentResult.Value;
                            bestWeight = currentResult.Weight;
                        }
                    }
                }
            }

            var selected = best.Select((inc, i) => (inc, i))
                               .Where(x => x.inc)
                               .Select(x => x.i)
                               .ToList();

            return new KnapsackResult(bestVal, bestWeight, selected);
        }
    }
}
