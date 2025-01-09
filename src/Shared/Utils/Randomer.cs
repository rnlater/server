
namespace Shared.Utils
{
    public static class Randomer
    {
        private static readonly Random random = new Random();

        public static List<T> GetRandomElementAsList<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentOutOfRangeException("The list cannot be null or empty.", nameof(list));
            }

            int index = random.Next(list.Count);
            return [list[index]];
        }

        public static List<T> GetRandomElementsAsList<T>(List<T> list, int quantity)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentOutOfRangeException("The list cannot be null or empty.", nameof(list));
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException("The quantity must be greater than 0.", nameof(quantity));
            }

            if (quantity > list.Count)
            {
                throw new ArgumentOutOfRangeException("The quantity must be less than or equal to the list count.", nameof(quantity));
            }

            var shuffledList = list.OrderBy(_ => random.Next()).ToList();
            return shuffledList.Take(quantity).ToList();
        }

        public static List<List<T>> GetRandomGroups<T>(List<T> List, int maxGroupSize = 4, int minGroupSize = 2)
        {
            var random = new Random();
            var Groups = new List<List<T>>();
            var shuffledList = List.OrderBy(_ => random.Next()).ToList();

            if (shuffledList.Count < minGroupSize)
            {
                Groups.Add(shuffledList);
                return Groups;
            }

            int i = 0;
            while (i < shuffledList.Count)
            {
                int remaining = shuffledList.Count - i;

                int groupSize = random.Next(minGroupSize, Math.Min(maxGroupSize, remaining) + 1);

                var group = shuffledList.Skip(i).Take(groupSize).ToList();
                Groups.Add(group);

                i += groupSize;
            }

            if (Groups.Count > 1 && Groups.Last().Count < minGroupSize)
            {
                var lastGroup = Groups.Last();
                Groups.RemoveAt(Groups.Count - 1);

                foreach (T theT in lastGroup)
                {
                    Groups[^1].Add(theT);
                }
            }

            return Groups;
        }

    }
}
