using KnapsackSolver.Models;

namespace KnapsackSolver.Services
{
    internal class KnapsackFactory
    {
        private static readonly Random r = new();

        public static List<Item> GenerateItems(int count, int maxWeight, int maxValue)
        {
            var items = new List<Item>(count);
            for (int i = 0; i < count; i++)
            {
                int w = r.Next(1, maxWeight + 1);
                int v = r.Next(1, maxValue + 1);
                items.Add(new Item(w, v));
            }
            return items;
        }

        public static int GetCapacity(int itemCount)
        {
            if (itemCount <= 15)
                return 100;
            if (itemCount <= 30)
                return 200;
            return 300;
        }
    }
}
