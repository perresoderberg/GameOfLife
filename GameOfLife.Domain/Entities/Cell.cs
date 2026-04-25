using GameOfLife.Domain.Enums;
using GameOfLife.Domain.ValueObjects;

namespace GameOfLife.Domain.Entities;
public record struct Cell(Position Position, CellState State);