using KnapsackSolver.Models;
using System.Diagnostics;

namespace KnapsackSolver.Services
{
    public static class BatchRunner
    {
        public record RunInfo(int BestValue, long Ms);

        public static List<RunInfo> RunMany(Func<KnapsackResult> solver, int runs = 30)
        {
            var list = new List<RunInfo>(runs);
            var sw = new Stopwatch();

            for (int k = 0; k < runs; k++)
            {
                Experiment.ResetFes();
                sw.Restart();
                var res = solver();
                sw.Stop();

                list.Add(new RunInfo(res.BestValue, sw.ElapsedMilliseconds));
            }
            return list;
        }

        public static void PrintStats(IEnumerable<RunInfo> data, string label)
        {
            var bestArr = data.Select(d => d.BestValue).OrderBy(x => x).ToArray();
            var msArr = data.Select(d => d.Ms).OrderBy(x => x).ToArray();

            static double Std(double[] ar, double mean) =>
                Math.Sqrt(ar.Select(x => (x - mean) * (x - mean)).Average());

            double meanVal = bestArr.Average();
            double meanMs = msArr.Average();

            Console.WriteLine($"\n{label}  (n = {data.Count()})");
            Console.WriteLine($"  VALUE     =>  min: {bestArr.First()};  max: {bestArr.Last()};  " +
                              $"mean: {meanVal:F2};  median: {bestArr[bestArr.Length / 2]};  " +
                              $"std: {Std([.. bestArr.Select(x => (double)x)], meanVal):F2};");
            Console.WriteLine($"  TIME [ms] =>  min: {msArr.First()};  max: {msArr.Last()};  " +
                              $"mean: {meanMs:F1};  median: {msArr[msArr.Length / 2]};  " +
                              $"std: {Std([.. msArr.Select(x => (double)x)], meanMs):F1};");
        }
    }
}
