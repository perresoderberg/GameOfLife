using GameOfLife.Domain.ValueObjects;

namespace GameOfLife.Presentation.SeedProvider;
public interface IGridSeed
{
    IEnumerable<Position> CreateAlivePositions(int width, int height);
}
