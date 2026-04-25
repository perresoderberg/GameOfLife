using GameOfLife.Application.Features;
using GameOfLife.Domain.Entities;
using GameOfLife.Domain.Rules;
using GameOfLife.Presentation.SeedProvider;
using GameOfLife.Presentation.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GameOfLife.Presentation.ViewModels;

public class GameViewModel : ViewModelBase
{
    private Grid _grid;
    private readonly IGameRule _rule;
    private readonly NextGeneration _nextGeneration;
    private readonly ITimerService _timer;

    public ObservableCollection<CellViewModel> Cells { get; } = new();

    public ICommand StepCommand { get; }
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    public int Width { get; } = 20;
    public int Height { get; } = 20;

    public GameViewModel(Random random, ITimerService timer)
    {
        _rule = new GameOfLifeRule();
        _nextGeneration = new NextGeneration(_rule);
        _timer = timer;

        var seed = new RandomSeed(random);

        _grid = Grid.CreateFromAliveCells(
            Width,
            Height,
            seed.Create(Width, Height));

        InitializeCells();

        StepCommand = new RelayCommand(Step);
        StartCommand = new RelayCommand(Start);
        StopCommand = new RelayCommand(Stop);
    }

    private void InitializeCells()
    {
        Cells.Clear();

        foreach (var cell in _grid.GetAllCells())
        {
            Cells.Add(new CellViewModel(cell));
        }
    }
    private void Step()
    {
        var result = _nextGeneration.Execute(_grid);

        if (result.IsFailure)
            return;

        _grid = result.Value;

        UpdateCells();
    }

    private void UpdateCells()
    {
        var domainCells = _grid.GetAllCells().ToList();

        for (int i = 0; i < domainCells.Count; i++)
        {
            Cells[i].Update(domainCells[i]);
        }
    }

    private void Start()
    {
        _timer.Start(Step);
    }

    private void Stop()
    {
        _timer.Stop();
    }
}