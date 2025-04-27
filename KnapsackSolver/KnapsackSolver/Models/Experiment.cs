using System.Runtime.CompilerServices;

namespace KnapsackSolver.Models
{
    public static class Experiment
    {
        public static int MaxFes = 10_000;         
        public static int FesCounter = 0;

        public static void ResetFes() => FesCounter = 0;
    }
}
