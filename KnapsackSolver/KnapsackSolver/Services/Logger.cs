using KnapsackSolver.Models;

namespace KnapsackSolver.Services
{
    public static class Logger
    {
        private static readonly List<LogPoint> points = [];

        public static void Clear() => points.Clear();
        public static void Add(int fes, int best) => points.Add(new LogPoint(fes, best));
        public static IReadOnlyList<LogPoint> Points => points;

        public static void SaveRun(string file)
        {
            var lines = Points.Select(p => $"{p.Fes},{p.Best}");
            File.WriteAllLines(file, lines);
        }
    }
}
