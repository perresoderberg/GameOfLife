using GameOfLife.Domain.ValueObjects;

namespace GameOfLife.Presentation.SeedProvider;

public class RandomSeed : IGridSeed
{
    private readonly Random _random;

    public RandomSeed(Random random)
    {
        _random = random;
    }
    public IEnumerable<Position> CreateAlivePositions(int width, int height)
    {
        return Enumerable.Range(0, (width * height) / 5)
            .Select(_ => new Position(
                _random.Next(width),
                _random.Next(height)));
    }
}