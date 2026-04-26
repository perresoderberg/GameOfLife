using GameOfLife.Domain.Enums;

namespace GameOfLife.Domain.Rules;
public class GameOfLifeRule : IGameRule
{
    public CellState DetermineNextState(CellState currentState, int aliveNeighbors)
    {
        return currentState switch
        {
            CellState.Alive when aliveNeighbors < 2 => CellState.Dead,
            CellState.Alive when aliveNeighbors <= 3 => CellState.Alive,
            CellState.Alive => CellState.Dead,

            CellState.Dead when aliveNeighbors == 3 => CellState.Alive,

            _ => currentState
        };
    }
}