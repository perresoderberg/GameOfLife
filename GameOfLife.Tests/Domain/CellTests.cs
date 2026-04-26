using FluentAssertions;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.ValueObjects;
using Xunit;

public class CellTests
{
    [Fact]
    public void Cell_ShouldHaveCorrectPositionAndState()
    {
        var pos = new Position(1, 2);

        var cell = new Cell(pos, CellState.Alive);

        cell.Position.Should().Be(pos);
        cell.State.Should().Be(CellState.Alive);
    }

    [Fact]
    public void Cell_WithExpression_ShouldCreateNewInstance()
    {
        var cell = new Cell(new Position(1, 1), CellState.Dead);

        var updated = cell with { State = CellState.Alive };

        updated.State.Should().Be(CellState.Alive);
        cell.State.Should().Be(CellState.Dead); // original unchanged
    }

    [Fact]
    public void Cells_WithSameValues_ShouldBeEqual()
    {
        var a = new Cell(new Position(1, 1), CellState.Alive);
        var b = new Cell(new Position(1, 1), CellState.Alive);

        a.Should().Be(b);
    }
}