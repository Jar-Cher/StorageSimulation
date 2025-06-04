using Npgsql;
using System.Collections.Immutable;
using System.Xml.Linq;


namespace StorageSimulation
{
    class Program
    {
        static Random rand = new Random();

        static List<Pallet> generateData(
            int palletsToGenerate,
            int boxesPerPallet
            )
        {
            Console.WriteLine("===RANDOM DATA GENERATION===");
            List<Pallet> pallets = new List<Pallet>();
            for (var i = 0; i < palletsToGenerate; i++)
            {
                Pallet newPallet = new Pallet(
                    randParam(),
                    randParam(),
                    randParam()
                    );
                Console.WriteLine("=BOXES=");
                for (var j = 0; j < boxesPerPallet; j++)
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
            return pallets;
        }

        static List<Pallet> getDataFromConsole()
        {
            Console.WriteLine("Enter number of Pallets");
            int palletsAmount = int.Parse(
                Console.ReadLine() ?? "0"
                );
            List<Pallet> pallets = new List<Pallet>();
            for (var i = 0; i < palletsAmount; i++)
            {
                Console.WriteLine(
                    "Enter new pallet's width, length, height"
                    );
                int width =
                    int.Parse(Console.ReadLine() ?? "0");
                int length =
                    int.Parse(Console.ReadLine() ?? "0");
                int height =
                    int.Parse(Console.ReadLine() ?? "0");
                Pallet pallet = new Pallet(
                    width,
                    length,
                    height
                    );
                Console.WriteLine(
                    "Enter new pallet's number of boxes"
                    );
                int boxesAmount =
                    int.Parse(Console.ReadLine() ?? "0");
                for (var j = 0; j < boxesAmount; j++)
                {
                    Console.WriteLine(
                        """
                        Enter new box's width, length,
                        height and weight
                        """
                        );
                    int boxWidth =
                        int.Parse(Console.ReadLine() ?? "0");
                    int boxLength =
                        int.Parse(Console.ReadLine() ?? "0");
                    int boxHeight =
                        int.Parse(Console.ReadLine() ?? "0");
                    int boxWeight =
                        int.Parse(Console.ReadLine() ?? "0");
                    Console.WriteLine(
                        "Enter new box's expiration date"
                        );
                    DateOnly expirationDate =
                        DateOnly.Parse(
                            Console.ReadLine() ?? "31.12.9999"
                            );
                    Box box = Box.GetNewBoxWithExpirationDate(
                        boxWidth,
                        boxLength,
                        boxHeight,
                        boxWeight,
                        expirationDate
                        );
                    pallet.AddBox(box);
                }
                Console.WriteLine(pallet);
                pallets.Add(pallet);
            }
            return pallets;
        }

        static List<Pallet> getDataFromDb()
        {
            List<Pallet> pallets = new List<Pallet>();
            string connString =
                $"""
                Server={"localhost"}; User Id={"postgres"};
                 Database={"MonopolyStorage"}; Port={"5432"};
                 Password={"2348815"}; SSLMode=Prefer
                """;
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(
                        "SELECT * FROM pallet", conn)
                    )
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pallet pallet = new Pallet(
                            reader.GetDouble(1),
                            reader.GetDouble(2),
                            reader.GetDouble(3)
                            );
                        Console.WriteLine(pallet);
                        pallets.Add(pallet);
                    }
                    reader.Close();
                }
                using (var command = new NpgsqlCommand(
                    "SELECT * FROM box", conn)
                    )
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Box box = Box
                            .GetNewBoxWithExpirationDate(
                            reader.GetDouble(1),
                            reader.GetDouble(2),
                            reader.GetDouble(3),
                            reader.GetDouble(4),
                            DateOnly.FromDateTime(
                                reader.GetDateTime(5))
                            );
                        Console.WriteLine(box);
                        pallets[reader.GetInt16(6) - 1]
                            .AddBox(box);
                    }
                    reader.Close();
                }
            }
            return pallets;
        }

        static void firstTask(List<Pallet> pallets)
        {
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
        }

        static void secondTask(List<Pallet> pallets)
        {
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

        static void Main(string[] args)
        {
            const int PALLETS_TO_GENERATE = 10;
            const int BOXES_PER_PALLET = 3;
            List<Pallet> pallets = new List<Pallet>();
            string typeGen = Console.ReadLine() ?? "";
            pallets = typeGen switch
            {
                "1" => generateData(
                PALLETS_TO_GENERATE,
                BOXES_PER_PALLET
                ),
                "2" => getDataFromConsole(),
                "3" => getDataFromDb(),
                _ => generateData(
                PALLETS_TO_GENERATE,
                BOXES_PER_PALLET
                )
            };
            
            firstTask(pallets);

            secondTask(pallets);
        }
    }
}
