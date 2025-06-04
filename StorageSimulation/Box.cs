using System.Text;


namespace StorageSimulation
{
    public class Box : IContainer
    {
        private int id;
        private double width;
        private double length;
        private double height;
        private double weight;
        private DateOnly expirationDate;
        private static int serialId = 0;

        public int Id 
        { 
            get => id; 
            private set => id = value; 
        }

        public double Width 
        { 
            get => width; 
            private set
            { 
                if (value > 0) width = value; 
                else throw new ArgumentOutOfRangeException(); 
            } 
        }

        public double Length
        {
            get => length; 
            private set
            { 
                if (value > 0) length = value; 
                else throw new ArgumentOutOfRangeException(); 
            } 
        }

        public double Height 
        {
            get => height;
            private set
            {
                if (value > 0) height = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public double Weight
        {
            get => weight;
            private set
            {
                if (value > 0) weight = value;
                else throw new ArgumentOutOfRangeException();
            }
        }

        public DateOnly ExpirationDate 
        {
            get => expirationDate;
            private set { expirationDate = value; }
        }

        public double GetCapacity()
        {
            return Width * Length * Height;
        }

        public override string? ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (
                System.Reflection.PropertyInfo property
                    in GetType().GetProperties()
                )
            {
                sb.Append(property.Name);
                sb.Append(": ");
                sb.Append(property.GetValue(this, null));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static Box GetNewBoxWithExpirationDate(
            double width,
            double length,
            double depth,
            double weight,
            DateOnly expirationDate)
        {
            return new Box(
                width,
                length,
                depth,
                weight,
                expirationDate);
        }

        public static Box GetNewBoxWithProductionDate(
            double width,
            double length,
            double depth,
            double weight,
            DateOnly productionDate)
        {
            return new Box(
                width,
                length,
                depth,
                weight,
                productionDate.AddDays(100));
        }

        private Box(
            double width,
            double length,
            double depth,
            double weight,
            DateOnly expirationDate)
        {
            Id = serialId++;
            Width = width;
            Length = length;
            Height = depth;
            Weight = weight;
            ExpirationDate = expirationDate;
        }
    }
}
