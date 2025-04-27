using KnapsackSolver.Models;
using KnapsackSolver.Services;
using System.Text.RegularExpressions;

namespace KnapsackSolver.Solvers
{
    public static class RandomSearchSolver
    {
        private static readonly Random r = new();

        public static KnapsackResult Solve(IReadOnlyList<Item> items, int capacity)
        {
            int n = items.Count;

            int bestVal = int.MinValue;
            bool[] bestMask = new bool[n];
            Logger.Clear();

            while (Experiment.FesCounter < Experiment.MaxFes)
            {
                bool[] mask = [.. Enumerable.Range(0, n).Select(_ => r.NextDouble() < 0.5)];

                var (val, w) = Evaluator.Evaluate(mask, items, capacity); 
                if (w > capacity) continue;                     

                if (val > bestVal)
                {
                    bestVal = val;
                    bestMask = mask;
                    Logger.Add(Experiment.FesCounter, bestVal);
                }
            }

            var sel = bestMask.Select((inc, i) => (inc, i)).Where(x => x.inc).Select(x => x.i).ToList();
            int bestWeight = sel.Sum(i => items[i].Weight);

            Logger.SaveRun($"rs_run.csv");
            return new KnapsackResult(bestVal, bestWeight, sel);
        }
    }
}
