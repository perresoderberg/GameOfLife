using FluentAssertions;
using GameOfLife.Application.Features;
using GameOfLife.Application.Results;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using GameOfLife.Domain.ValueObjects;
using Xunit;

public class NextGenerationTests
{
    private readonly IGameRule _rule = new GameOfLifeRule();

    [Fact]
    public void Execute_WithNullGrid_ShouldReturnFailure()
    {
        var useCase = new NextGeneration(_rule);

        var result = useCase.Execute(null!);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NullError");
        result.Error.Message.Should().Contain("Grid");
    }

    [Fact]
    public void Execute_WithZeroWidth_ShouldReturnValidationError()
    {
        var useCase = new NextGeneration(_rule);
        var grid = new Grid(0, 5, new Cell[0, 5]);

        var result = useCase.Execute(grid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ValidationError");
    }

    [Fact]
    public void Execute_WithZeroHeight_ShouldReturnValidationError()
    {
        var useCase = new NextGeneration(_rule);
        var grid = new Grid(5, 0, new Cell[5, 0]);

        var result = useCase.Execute(grid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ValidationError");
    }

    [Fact]
    public void Execute_WithValidGrid_ShouldReturnSuccess()
    {
        var useCase = new NextGeneration(_rule);
        var grid = Grid.CreateEmpty(5, 5);

        var result = useCase.Execute(grid);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Execute_ShouldReturnNewGridInstance()
    {
        var useCase = new NextGeneration(_rule);
        var grid = Grid.CreateEmpty(5, 5);

        var result = useCase.Execute(grid);

        result.Value.Should().NotBeSameAs(grid);
    }

    [Fact]
    public void Execute_ShouldApplyGameOfLifeRules()
    {
        var useCase = new NextGeneration(_rule);

        var initial = new[]
        {
            new Position(1,2),
            new Position(2,2),
            new Position(3,2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, initial);

        var result = useCase.Execute(grid);

        var alive = result.Value.GetAllCells()
            .Where(c => c.State == CellState.Alive)
            .Select(c => c.Position);

        alive.Should().BeEquivalentTo(new[]
        {
            new Position(2,1),
            new Position(2,2),
            new Position(2,3)
        });
    }

    [Fact]
    public void Execute_ShouldNotModifyOriginalGrid()
    {
        var useCase = new NextGeneration(_rule);

        var initial = new[]
        {
            new Position(1,2),
            new Position(2,2),
            new Position(3,2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, initial);

        var _ = useCase.Execute(grid);

        var originalAlive = grid.GetAllCells()
            .Where(c => c.State == CellState.Alive)
            .Select(c => c.Position);

        originalAlive.Should().BeEquivalentTo(initial);
    }

    [Fact]
    public void Execute_CalledMultipleTimes_ShouldBeDeterministic()
    {
        var useCase = new NextGeneration(_rule);

        var grid = Grid.CreateEmpty(5, 5);

        var result1 = useCase.Execute(grid);
        var result2 = useCase.Execute(grid);

        result1.Value.GetAllCells()
            .Should().BeEquivalentTo(result2.Value.GetAllCells());
    }
}