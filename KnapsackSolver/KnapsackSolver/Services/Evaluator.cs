using KnapsackSolver.Models;

namespace KnapsackSolver.Services
{
    public static class Evaluator
    {
        public sealed record EvalResult(int Value, int Weight);

        public static EvalResult Evaluate(bool[] mask, IReadOnlyList<Item> items, int capacity)
        {
            Experiment.FesCounter++;

            int v = 0, w = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                if (!mask[i]) continue;

                w += items[i].Weight;
                v += items[i].Value;

                if (w > capacity)
                    return new EvalResult(-1_000_000, w);     // penalizace
            }
            return new EvalResult(v, w);
        }
    }
}
