using GameOfLife.Domain.Enums;
using GameOfLife.Domain.Rules;
using GameOfLife.Domain.ValueObjects;

namespace GameOfLife.Domain.Entities;
public class Grid
{
    private readonly Cell[,] _cells;

    public int Width { get; }
    public int Height { get; }

    public Grid(int width, int height, Cell[,] cells)
    {
        if (cells.GetLength(0) != width || cells.GetLength(1) != height)
            throw new ArgumentException("Invalid grid dimensions");

        Width = width;
        Height = height;
        _cells = cells;
    }
    private bool IsInside(Position pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;

    public Cell GetCell(Position pos)
    {
        if (!IsInside(pos))
            return new Cell(pos, CellState.Dead);

        return _cells[pos.X, pos.Y];
    }

    public int CountAliveNeighbors(Position position)
    {
        int countAliveNeighbors = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                var neighbor = new Position(position.X + x, position.Y + y);

                if (IsInside(neighbor) &&
                    _cells[neighbor.X, neighbor.Y].State == CellState.Alive)
                {
                    countAliveNeighbors++;
                }
            }
        }

        return countAliveNeighbors;
    }
    public Grid NextGeneration(IGameRule rule)
    {
        var newCells = new Cell[Width, Height];

        foreach (var position in AllPositions())
        {
            var currentCell = _cells[position.X, position.Y];
            var aliveNeighbors = CountAliveNeighbors(position);

            var nextState = rule.DetermineNextState(currentCell.State, aliveNeighbors);

            newCells[position.X, position.Y] = currentCell with { State = nextState };
        }

        return new Grid(Width, Height, newCells);
    }

    private IEnumerable<Position> AllPositions()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                yield return new Position(x, y);
    }
    public IEnumerable<Cell> GetAllCells()
    {
        foreach (var pos in AllPositions())
            yield return _cells[pos.X, pos.Y];
    }

    public static Grid CreateEmpty(int width, int height)
    {
        var cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var position = new Position(x, y);
                cells[x, y] = new Cell(position, CellState.Dead);
            }
        }

        return new Grid(width, height, cells);
    }

    public static Grid CreateFromAliveCells(int width, int height, IEnumerable<Position> aliveCells)
    {
        var cells = new Cell[width, height];

        InitializeAllAsDead(width, height, cells);

        SetAliveCells(width, height, aliveCells, cells);

        return new Grid(width, height, cells);
    }

    private static void SetAliveCells(int width, int height, IEnumerable<Position> aliveCells, Cell[,] cells)
    {
        foreach (var position in aliveCells)
        {
            if (position.X >= 0 && position.X < width &&
                position.Y >= 0 && position.Y < height)
            {
                cells[position.X, position.Y] = new Cell(position, CellState.Alive);
            }
        }
    }

    private static void InitializeAllAsDead(int width, int height, Cell[,] cells)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Position(x, y);
                cells[x, y] = new Cell(pos, CellState.Dead);
            }
        }
    }
}