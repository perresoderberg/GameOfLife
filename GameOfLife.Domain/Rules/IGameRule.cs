using GameOfLife.Domain.Enums;

namespace GameOfLife.Domain.Rules;
public interface IGameRule
{
    CellState GetNextState(CellState current, int aliveNeighbors);
}
