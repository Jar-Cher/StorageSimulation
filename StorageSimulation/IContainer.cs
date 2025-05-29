

namespace StorageSimulation
{
    public interface IContainer
    {
        int Id { get; }
        
        double Width { get; }

        double Length { get; }

        double Height { get; }

        double Weight { get; }

        DateOnly ExpirationDate { get; }

        public double GetCapacity();
    }
}
