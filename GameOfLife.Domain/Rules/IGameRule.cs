using GameOfLife.Domain.Enums;

namespace GameOfLife.Domain.Rules;
public interface IGameRule
{
    CellState DetermineNextState(CellState currentState, int aliveNeighbors);
}
