using FluentAssertions;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using GameOfLife.Domain.ValueObjects;
using Xunit;

namespace GameOfLife.Tests.Domain;

public class GridTests
{
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
    public void GetCell_OutsideGrid_ShouldReturnDead()
    {
        var grid = Grid.CreateEmpty(5, 5);

        var cell = grid.GetCell(new Position(-1, -1));

        cell.State.Should().Be(CellState.Dead);
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
            .Select(c => c.Position)
            .ToList();

        alive.Should().BeEquivalentTo(new[]
        {
            new Position(2,1),
            new Position(2,2),
            new Position(2,3)
        });
    }

    [Fact]
    public void CreateEmpty_ShouldHaveAllDeadCells()
    {
        var grid = Grid.CreateEmpty(3, 3);

        grid.GetAllCells()
            .All(c => c.State == CellState.Dead)
            .Should().BeTrue();
    }
}