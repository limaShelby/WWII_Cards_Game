using MainLib;

namespace ConsoleUI
{
    public static class Extensions
    {
        // This method builds a unit from an initializer
        public static Unit CreateUnit(this UnitInitializer initializer)
        {
            return new Unit(initializer.Name, initializer.Cost, initializer.Damage, initializer.Defense, initializer.Order, initializer.Description);
        }
        // This method builds an order from an initializer
        public static Order CreateOrder(this OrderInitializer initializer)
        {
            return new Order(initializer.Name, initializer.Cost, initializer.Order, initializer.Description);
        }
        // This method returns a shuffled generic list
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> result = new List<T>();
            foreach (var randomIndex in Randomness(list.Count))
            {
                result.Add(list[randomIndex]);
            }
            return result;
        }
        // This method returns a randomly sorted set of nth numbers, starting from 0
        private static IEnumerable<int> Randomness(int n)
        {
            List<int> numbers = new List<int>();
            Random random = new Random();

            while (numbers.Count != n)
            {
                int number = (int)random.NextInt64(0, n);

                if (numbers.Contains(number)) continue;
                numbers.Add(number);
                yield return number;
            }
        }
    }
}