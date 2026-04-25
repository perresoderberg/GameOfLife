using FluentAssertions;
using GameOfLife.Application.Features;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Rules;
using Xunit;

namespace GameOfLife.Tests.Application;
public class NextGenerationTests
{
    [Fact]
    public void Execute_WithNullGrid_ShouldFail()
    {
        var useCase = new NextGeneration(new GameOfLifeRule());

        var result = useCase.Execute(null!);

        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Contain("null");
    }

    [Fact]
    public void Execute_WithInvalidGrid_ShouldFail()
    {
        var rule = new GameOfLifeRule();
        var useCase = new NextGeneration(rule);

        var invalidGrid = new Grid(0, 0, new Cell[0, 0]);

        var result = useCase.Execute(invalidGrid);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Execute_WithValidGrid_ShouldReturnNewGrid()
    {
        var rule = new GameOfLifeRule();
        var useCase = new NextGeneration(rule);

        var grid = Grid.CreateEmpty(5, 5);

        var result = useCase.Execute(grid);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Should().NotBeSameAs(grid);
    }
}