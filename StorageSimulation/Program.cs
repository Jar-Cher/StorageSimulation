using System.Collections.Immutable;


namespace StorageSimulation
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            const int PALLETS_TO_GENERATE = 10;
            const int BOXES_PER_PALLET = 3;
            Console.WriteLine("===DATA GENERATION===");
            List<Pallet> pallets = new List<Pallet>();
            for (var i = 0; i < PALLETS_TO_GENERATE; i++)
            {
                Pallet newPallet = new Pallet(
                    randParam(),
                    randParam(),
                    randParam()
                    );
                Console.WriteLine("=BOXES=");
                for (var j = 0; j < BOXES_PER_PALLET; j++)
                {
                    Box newBox =
                        Box.GetNewBoxWithExpirationDate(
                            randParam((int)newPallet.Width),
                            randParam((int)newPallet.Length),
                            randParam(),
                            randParam(),
                            DateOnly.FromDayNumber(randParam())
                            );
                    newPallet.AddBox(newBox);
                    Console.WriteLine(newBox);
                }
                pallets.Add(newPallet);
                Console.WriteLine("=PALLET=");
                Console.WriteLine(newPallet);
            }

            Console.WriteLine("===FIRST TASK===");
            ImmutableSortedDictionary
                <DateOnly, List<Pallet>> task1 = pallets
                .GroupBy(x => x.ExpirationDate)
                .ToImmutableSortedDictionary(
                    x => x.Key,
                    x => x.OrderBy(y => y.Weight).ToList()
                    );
            foreach (
                KeyValuePair<DateOnly, List<Pallet>> i in task1
                )
            {
                Console.WriteLine(i.Key);
                i.Value.ForEach(Console.WriteLine);
            }

            Console.WriteLine("===SECOND TASK===");
            IOrderedEnumerable<Pallet> task2 = pallets
                .Where(x => x.GetBoxes().Count > 0)
                .OrderBy(x =>
                    x.GetBoxes().Max(y => y.ExpirationDate))
                .TakeLast(3)
                .OrderBy(x => x.GetCapacity());
            foreach (Pallet i in task2)
            {
                Console.WriteLine(i);
            }
        }

        static int randParam(int max = 9)
        {
            return rand.Next(max) + 1;
        }
    }
}
