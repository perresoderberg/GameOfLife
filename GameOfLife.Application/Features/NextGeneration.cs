using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Rules;
using GameOfLife.Application.Results;

namespace GameOfLife.Application.Features;

public class NextGeneration : INextGeneration
{
    private readonly IGameRule _rule;

    public NextGeneration(IGameRule rule)
    {
        _rule = rule;
    }

    public Result<Grid> Execute(Grid currentGrid)
    {
        if (currentGrid is null)
            return Result<Grid>.Fail(Error.Null("Grid cannot be null"));

        if (currentGrid.Width <= 0 || currentGrid.Height <= 0)
            return Result<Grid>.Fail(Error.Validation("Grid must have positive dimensions"));

        var nextGrid = currentGrid.NextGeneration(_rule);

        return Result<Grid>.Success(nextGrid);
    }
}