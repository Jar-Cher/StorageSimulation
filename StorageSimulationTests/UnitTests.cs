using StorageSimulation;


namespace StorageSimulationTests
{
    public class UnitTests
    {
        [Theory]
        [InlineData(-42, 42, 42, 42)]
        [InlineData(0, 42, 42, 42)]
        [InlineData(42, -42, 42, 42)]
        [InlineData(42, 0, 42, 42)]
        [InlineData(42, 42, -42, 42)]
        [InlineData(42, 42, 0, 42)]
        [InlineData(42, 42, 42, -42)]
        [InlineData(42, 42, 42, 0)]
        [InlineData(-42, -42, -42, -42)]
        [InlineData(0, 0, 0, 0)]
        public void TestOutOfRangeBoxExpirationConstructor(
            double width,
            double length,
            double depth,
            double weight)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Box.GetNewBoxWithExpirationDate(
                    width,
                    length,
                    depth,
                    weight,
                    DateOnly.FromDayNumber(42))
                );
        }

        [Theory]
        [InlineData(-42, 42, 42, 42)]
        [InlineData(0, 42, 42, 42)]
        [InlineData(42, -42, 42, 42)]
        [InlineData(42, 0, 42, 42)]
        [InlineData(42, 42, -42, 42)]
        [InlineData(42, 42, 0, 42)]
        [InlineData(42, 42, 42, -42)]
        [InlineData(42, 42, 42, 0)]
        [InlineData(-42, -42, -42, -42)]
        [InlineData(0, 0, 0, 0)]
        public void TestOutOfRangeBoxProductionConstructor(
            double width,
            double length,
            double depth,
            double weight)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => Box.GetNewBoxWithProductionDate(
                    width,
                    length,
                    depth,
                    weight,
                    DateOnly.FromDayNumber(42))
            );
        }

        [Theory]
        [InlineData(-42, 42, 42)]
        [InlineData(0, 42, 42)]
        [InlineData(42, -42, 42)]
        [InlineData(42, 0, 42)]
        [InlineData(42, 42, -42)]
        [InlineData(42, 42, 0)]
        [InlineData(-42, -42, -42)]
        [InlineData(0, 0, 0)]
        public void TestOutOfRangePalletConstructor(
            double width,
            double length,
            double depth)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 
            new Pallet(width, length, depth));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(42, 1, 1, 42)]
        [InlineData(1, 42, 1, 42)]
        [InlineData(1, 1, 42, 42)]
        [InlineData(3, 3, 3, 27)]
        public void TestBoxGetCapacity(
            double width,
            double length,
            double depth,
            double expectedCapacity)
        {
            Assert.Equal(
                expectedCapacity,
                Box.GetNewBoxWithProductionDate(
                    width,
                    length,
                    depth,
                    42,
                    DateOnly.FromDayNumber(42))
                .GetCapacity());
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 1, 1, 0, 1)]
        [InlineData(42, 1, 1, 1, 1, 1, 0, 42)]
        [InlineData(1, 42, 1, 1, 1, 1, 0, 42)]
        [InlineData(1, 1, 42, 1, 1, 1, 0, 42)]
        [InlineData(3, 3, 3, 1, 1, 1, 0, 27)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 2)]
        [InlineData(1, 1, 1, 1, 1, 1, 41, 42)]
        [InlineData(42, 2, 1, 3, 2, 1, 1, 90)]
        [InlineData(42, 2, 1, 3, 2, 1, 2, 96)]
        [InlineData(0.5, 42, 2, 0.5, 1, 2, 1, 43)]
        [InlineData(0.5, 42, 2, 0.5, 1, 2, 3, 45)]
        [InlineData(1, 3, 42, 1, 2, 2.5, 1, 131)]
        [InlineData(1, 3, 42, 1, 2, 2.5, 5, 151)]
        [InlineData(23, 5, 5, 2, 2, 2, 1, 583)]
        public void TestPalletGetCapacity(
            double width,
            double length,
            double depth,
            double boxWidth,
            double boxLength,
            double boxDepth,
            int boxCount,
            double expectedCapacity)
        {
            Pallet pallet = new Pallet(width, length, depth);
            for (var i = 0; i < boxCount; i++)
            {
                pallet.AddBox(Box.GetNewBoxWithExpirationDate(
                    boxWidth,
                    boxLength,
                    boxDepth,
                    42,
                    DateOnly.FromDayNumber(42)));
            }
            Assert.Equal(
                expectedCapacity,
                pallet.GetCapacity());
        }

        [Theory]
        [InlineData(1, 0, 30)]
        [InlineData(1, 1, 31)]
        [InlineData(42, 1, 72)]
        [InlineData(30, 29, 900)]
        public void TestPalletGetWeight(
            double boxWeight,
            int boxCount,
            double expectedWeight)
        {
            Pallet pallet = new Pallet(42, 42, 42);
            for (var i = 0; i < boxCount; i++)
            {
                pallet.AddBox(Box.GetNewBoxWithExpirationDate(
                    42,
                    42,
                    42,
                    boxWeight,
                    DateOnly.FromDayNumber(42)));
            }
            Assert.Equal(expectedWeight, pallet.Weight);
        }

        [Theory]
        [InlineData(1, 101)]
        [InlineData(0, 100)]
        [InlineData(17, 117)]
        public void TestBoxGetExpirationDateFromProductionDate(
            int productionDateInDaysFromEra,
            int expectedExpirationDateInDaysFromEra)
        {
            Box box = Box.GetNewBoxWithProductionDate(
                42,
                42,
                42,
                42,
                DateOnly.FromDayNumber(
                    productionDateInDaysFromEra));
            Assert.Equal(
                DateOnly.FromDayNumber(
                    expectedExpirationDateInDaysFromEra),
                box.ExpirationDate);
        }

        [Theory]
        [InlineData(1, 0, 3652058)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 1)]
        [InlineData(42, 1, 42)]
        [InlineData(15, 3, 15)]
        public void TestPalletGetExpirationDate(
            int boxExpirationDateDelta,
            int boxCount,
            int expectedExpirationDateInDaysFromEra)
        {
            Pallet pallet = new Pallet(42, 42, 42);
            for (var i = 0; i < boxCount; i++)
            {
                pallet.AddBox(Box.GetNewBoxWithExpirationDate(
                    42,
                    42,
                    42,
                    42,
                    DateOnly.FromDayNumber(
                        boxExpirationDateDelta++)));
            }
            Assert.Equal(
                DateOnly.FromDayNumber(
                    expectedExpirationDateInDaysFromEra),
                pallet.ExpirationDate);
        }

        [Theory]
        [InlineData(42, 42, 100500, 42)]
        [InlineData(42, 42, 42, 100500)]
        [InlineData(42, 42, 100500, 100500)]
        public void TestOutOfRangeBoxAdditionToPallet(
            double palletWidth,
            double palletLength,
            double boxWidth,
            double boxLength)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Pallet(
                    palletWidth,
                    palletLength,
                    42)
                .AddBox(Box.GetNewBoxWithExpirationDate(
                    boxWidth,
                    boxLength,
                    42,
                    42,
                    DateOnly.FromDayNumber(42)))
            );
        }

        [Theory]
        [InlineData(
            42,
            1,
            2,
            """
            Width: 42
            Length: 1
            Height: 2
            Weight: 30
            ExpirationDate: 31.12.9999
            """)
        ]
        [InlineData(
            12.74, 
            42.33, 
            10.5, 
            """
            Width: 12,74
            Length: 42,33
            Height: 10,5
            Weight: 30
            ExpirationDate: 31.12.9999
            """)
        ]
        public void TestPalletToString(
            double width,
            double length,
            double depth,
            string expectedString)
        {
            Assert.Contains(
                expectedString,
                new Pallet(width, length, depth).ToString());
        }

        [Theory]
        [InlineData(
            42, 
            1, 
            2, 
            3, 
            4,
            """
            Width: 42
            Length: 1
            Height: 2
            Weight: 3
            ExpirationDate: 05.01.0001
            """)
        ]
        [InlineData(
            42, 
            1,
            2, 
            3,
            0, 
            """
            Width: 42
            Length: 1
            Height: 2
            Weight: 3
            ExpirationDate: 01.01.0001
            """)
        ]
        [InlineData(
            12.74, 
            42.33, 
            10.5,
            100.5, 
            42, 
            """
            Width: 12,74
            Length: 42,33
            Height: 10,5
            Weight: 100,5
            ExpirationDate: 12.02.0001
            """)
        ]
        public void TestBoxToString(
            double width,
            double length,
            double depth,
            double weight,
            int daysFromEraStart,
            string expectedString)
        {
            Assert.Contains(
                expectedString,
                Box.GetNewBoxWithExpirationDate(
                    width,
                    length,
                    depth,
                    weight,
                    DateOnly.FromDayNumber(daysFromEraStart))
                .ToString());
        }
    }
}