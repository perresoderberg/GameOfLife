using FluentAssertions;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using GameOfLife.Domain.ValueObjects;
using Xunit;

public class GridTests
{
    [Fact]
    public void CreateEmpty_ShouldHaveAllDeadCells()
    {
        var grid = Grid.CreateEmpty(3, 3);

        grid.GetAllCells()
            .All(c => c.State == CellState.Dead)
            .Should().BeTrue();
    }

    [Fact]
    public void CreateFromAliveCells_ShouldSetCorrectCellsAlive()
    {
        var alive = new[]
        {
            new Position(1, 1),
            new Position(2, 2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, alive);

        grid.GetCell(new Position(1, 1)).State.Should().Be(CellState.Alive);
        grid.GetCell(new Position(2, 2)).State.Should().Be(CellState.Alive);
    }

    [Fact]
    public void CreateFromAliveCells_ShouldIgnoreOutOfBoundsPositions()
    {
        var alive = new[]
        {
            new Position(1, 1),
            new Position(99, 99)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, alive);

        var aliveCount = grid.GetAllCells().Count(c => c.State == CellState.Alive);

        aliveCount.Should().Be(1);
    }

    [Fact]
    public void GetCell_OutsideGrid_ShouldReturnDead()
    {
        var grid = Grid.CreateEmpty(5, 5);

        var cell = grid.GetCell(new Position(-1, -1));

        cell.State.Should().Be(CellState.Dead);
    }

    [Fact]
    public void CountAliveNeighbors_ShouldReturnCorrectCount()
    {
        var alive = new[]
        {
            new Position(1,1),
            new Position(1,2),
            new Position(2,1)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, alive);

        var result = grid.CountAliveNeighbors(new Position(2, 2));

        result.Should().Be(3);
    }

    [Fact]
    public void CountAliveNeighbors_OnEdge_ShouldNotCrash()
    {
        var grid = Grid.CreateEmpty(3, 3);

        var result = grid.CountAliveNeighbors(new Position(0, 0));

        result.Should().Be(0);
    }

    [Fact]
    public void NextGeneration_ShouldNotModifyOriginalGrid()
    {
        var rule = new GameOfLifeRule();

        var initial = new[]
        {
            new Position(1,2),
            new Position(2,2),
            new Position(3,2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, initial);

        var _ = grid.NextGeneration(rule);

        var alive = grid.GetAllCells()
            .Where(c => c.State == CellState.Alive)
            .Select(c => c.Position);

        alive.Should().BeEquivalentTo(initial);
    }

    [Fact]
    public void NextGeneration_Blinker_ShouldOscillate()
    {
        var rule = new GameOfLifeRule();

        var initial = new[]
        {
            new Position(1,2),
            new Position(2,2),
            new Position(3,2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, initial);

        var next = grid.NextGeneration(rule);

        var alive = next.GetAllCells()
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
    public void Blinker_ShouldReturnToInitialState_AfterTwoSteps()
    {
        var rule = new GameOfLifeRule();

        var initial = new[]
        {
            new Position(1,2),
            new Position(2,2),
            new Position(3,2)
        };

        var grid = Grid.CreateFromAliveCells(5, 5, initial);

        var step1 = grid.NextGeneration(rule);
        var step2 = step1.NextGeneration(rule);

        var alive = step2.GetAllCells()
            .Where(c => c.State == CellState.Alive)
            .Select(c => c.Position);

        alive.Should().BeEquivalentTo(initial);
    }
}