using GameOfLife.Domain.Enums;

namespace GameOfLife.Domain.Rules;
public class GameOfLifeRule : IGameRule
{
    public CellState GetNextState(CellState current, int neighbors)
    {
        return current switch
        {
            CellState.Alive when neighbors < 2 => CellState.Dead,
            CellState.Alive when neighbors <= 3 => CellState.Alive,
            CellState.Alive => CellState.Dead,

            CellState.Dead when neighbors == 3 => CellState.Alive,

            _ => current
        };
    }
}