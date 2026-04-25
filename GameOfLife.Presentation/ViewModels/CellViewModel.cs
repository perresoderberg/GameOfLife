using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Enums;

namespace GameOfLife.Presentation.ViewModels;
public class CellViewModel : ViewModelBase
{
    private Cell _cell;

    public int X => _cell.Position.X;
    public int Y => _cell.Position.Y;

    public bool IsAlive => _cell.State == CellState.Alive;

    public CellViewModel(Cell cell)
    {
        _cell = cell;
    }

    public void Update(Cell newCell)
    {
        _cell = newCell;
        OnPropertyChanged(nameof(IsAlive));
    }
}