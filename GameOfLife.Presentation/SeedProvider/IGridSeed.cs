using GameOfLife.Domain.ValueObjects;

namespace GameOfLife.Presentation.SeedProvider;
public interface IGridSeed
{
    IEnumerable<Position> Create(int width, int height);
}
