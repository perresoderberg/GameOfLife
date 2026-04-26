using FluentAssertions;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using Xunit;

public class GameOfLifeRuleTests
{
    private readonly GameOfLifeRule _rule = new();

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void AliveCell_WithLessThanTwoNeighbors_ShouldDie(int neighbors)
    {
        var result = _rule.DetermineNextState(CellState.Alive, neighbors);

        result.Should().Be(CellState.Dead);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void AliveCell_WithTwoOrThreeNeighbors_ShouldSurvive(int neighbors)
    {
        var result = _rule.DetermineNextState(CellState.Alive, neighbors);

        result.Should().Be(CellState.Alive);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void AliveCell_WithMoreThanThreeNeighbors_ShouldDie(int neighbors)
    {
        var result = _rule.DetermineNextState(CellState.Alive, neighbors);

        result.Should().Be(CellState.Dead);
    }

    [Fact]
    public void DeadCell_WithExactlyThreeNeighbors_ShouldBecomeAlive()
    {
        var result = _rule.DetermineNextState(CellState.Dead, 3);

        result.Should().Be(CellState.Alive);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public void DeadCell_WithNonThreeNeighbors_ShouldStayDead(int neighbors)
    {
        var result = _rule.DetermineNextState(CellState.Dead, neighbors);

        result.Should().Be(CellState.Dead);
    }
}