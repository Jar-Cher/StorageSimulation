using System.Text;


namespace StorageSimulation
{
    public class Pallet : IContainer
    {
        private HashSet<Box> boxes = new HashSet<Box>();
        private int id;
        private double width;
        private double length;
        private double height;
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

        public double Weight =>
            boxes.Select(x => x.Weight).Sum() + 30;

        public DateOnly ExpirationDate =>
            boxes.Select(x => x.ExpirationDate)
            .DefaultIfEmpty(DateOnly.MaxValue)
            .Min();

        public double GetCapacity()
        {
            return Width * Length * Height + 
                boxes.Select(x => x.GetCapacity()).Sum();
        }

        public void AddBox(Box newBox)
        {
            if (Width >= newBox.Width && 
                Length >= newBox.Length)
            {
                boxes.Add(newBox);
            }
            else
            { 
                throw new ArgumentOutOfRangeException();
            }
        }

        public HashSet<Box> GetBoxes()
        {
            return boxes;
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

        public Pallet(double width, double length, double depth)
        {
            Id = serialId++;
            Width = width;
            Length = length;
            Height = depth;
        }
    }
}
