using KnapsackSolver.Models;
using KnapsackSolver.Services;

public static class BruteForceSolver
{
    public static KnapsackResult SolveBrute(IReadOnlyList<Item> items, int capacity)
    {

        int bestValue = 0;
        int bestWeight = 0;
        List<int> bestIndices = [];

        int n = items.Count;
        int combinations = 1 << n;

        for (int mask = 0; mask < combinations && Experiment.FesCounter < Experiment.MaxFes; mask++)
        {
            int totalW = 0, totalV = 0;
            var current = new List<int>();

            for (int i = 0; i < n; i++)
            {
                if ((mask & (1 << i)) == 0) continue;

                totalW += items[i].Weight;
                totalV += items[i].Value;
                current.Add(i);
            }

            if (totalW <= capacity && totalV > bestValue)
            {
                bestValue = totalV;
                bestWeight = totalW;
                bestIndices = [.. current];
                Logger.Add(Experiment.FesCounter, bestValue);
            }

            Experiment.FesCounter++; 
        }

        return new KnapsackResult(bestValue, bestWeight, bestIndices);
    }
}
