using FluentAssertions;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using Xunit;

namespace GameOfLife.Tests.Domain;
public class GameOfLifeRuleTests
{
    private readonly GameOfLifeRule _rule = new();

    [Fact]
    public void AliveCell_WithLessThan2Neighbors_ShouldDie()
    {
        var result = _rule.GetNextState(CellState.Alive, 1);

        result.Should().Be(CellState.Dead);
    }

    [Fact]
    public void AliveCell_With2Or3Neighbors_ShouldLive()
    {
        _rule.GetNextState(CellState.Alive, 2)
            .Should().Be(CellState.Alive);

        _rule.GetNextState(CellState.Alive, 3)
            .Should().Be(CellState.Alive);
    }

    [Fact]
    public void AliveCell_WithMoreThan3Neighbors_ShouldDie()
    {
        var result = _rule.GetNextState(CellState.Alive, 4);

        result.Should().Be(CellState.Dead);
    }

    [Fact]
    public void DeadCell_WithExactly3Neighbors_ShouldBecomeAlive()
    {
        var result = _rule.GetNextState(CellState.Dead, 3);

        result.Should().Be(CellState.Alive);
    }
}