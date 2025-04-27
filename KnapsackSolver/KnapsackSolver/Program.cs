using KnapsackSolver.Models;
using KnapsackSolver.Services;
using KnapsackSolver.Solvers;

class Program
{
    static void Main(string[] args)
    {
        int itemCount = 15;
        int maxWeight = 50;
        int maxValue = 50;
        int capacity = 100;

        var items = KnapsackFactory.GenerateItems(itemCount, maxWeight, maxValue);

        Console.WriteLine("Settings:");
        Console.WriteLine($"  Item count:        {itemCount}");
        Console.WriteLine($"  Max weight/item:   {maxWeight}");
        Console.WriteLine($"  Max value/item:    {maxValue}");
        Console.WriteLine($"  Knapsack capacity: {capacity}");

        Console.WriteLine("\nVygenerované položky (Weight, Value):");
        for (int i = 0; i < items.Count; i++)
            Console.WriteLine($"  [{i,2}]:  W={items[i].Weight,2},  V={items[i].Value,2}");

        // --- Brute Force -----------
        Experiment.ResetFes();
        Experiment.MaxFes = int.MaxValue;       
        Logger.Clear();

        var bfResult = BruteForceSolver.SolveBrute(items, capacity);
        Logger.SaveRun("bf.csv");

        Console.WriteLine("\nBrute-force result:");
        Console.WriteLine($"  Best value : {bfResult.BestValue}");
        Console.WriteLine($"  Total weight: {bfResult.BestWeight}");
        Console.WriteLine($"  FES         : {Experiment.FesCounter}\n");

        int fesLimit = Experiment.FesCounter;
        Experiment.MaxFes = fesLimit;
        Console.WriteLine($"Limiting heuristics: {fesLimit} FES\n");

        // --- Random Search 30× ------------------------------------
        var rsRuns = BatchRunner.RunMany(() => RandomSearchSolver.Solve(items, capacity), "rs");
        BatchRunner.PrintStats(rsRuns, "Random Search");

        // --- Simulated Annealing 30× -------------------------------
        var saStats = BatchRunner.RunMany(() => SimulatedAnnealingSolver.Solve(items, capacity), "sa");
        BatchRunner.PrintStats(saStats, "Simulated Annealing");
    }
}
