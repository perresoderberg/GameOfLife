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
    private readonly INextGeneration _nextGeneration;
    private readonly ITimerService _timer;
    private readonly IGridSeed _seed;

    private bool _isRunning;

    public ObservableCollection<CellViewModel> Cells { get; } = new();

    public ICommand StepCommand { get; }
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    public int Width { get; }
    public int Height { get; }

    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            OnPropertyChanged();

            // Update button states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();
        }
    }

    public GameViewModel(
        int width,
        int height,
        ITimerService timer,
        IGridSeed seed,
        INextGeneration nextGeneration)
    {
        Width = width;
        Height = height;

        _timer = timer;
        _seed = seed;

        _grid = Grid.CreateFromAliveCells(
            Width,
            Height,
            _seed.CreateAlivePositions(Width, Height));

        InitializeCells();

        StepCommand = new RelayCommand(Step);
        StartCommand = new RelayCommand(Start, () => !IsRunning);
        StopCommand = new RelayCommand(Stop, () => IsRunning);
        _nextGeneration = nextGeneration;
    }

    // Initialize UI once
    private void InitializeCells()
    {
        Cells.Clear();

        foreach (var cell in _grid.GetAllCells())
        {
            Cells.Add(new CellViewModel(cell));
        }
    }

    // One simulation step
    private void Step()
    {
        var result = _nextGeneration.Execute(_grid);

        if (result.IsFailure)
            return;

        _grid = result.Value;

        UpdateCells();
    }

    // Update UI without recreating objects
    private void UpdateCells()
    {
        var domainCells = _grid.GetAllCells().ToList();

        for (int i = 0; i < domainCells.Count; i++)
        {
            Cells[i].Update(domainCells[i]);
        }
    }

    // Start simulation
    private void Start()
    {
        if (IsRunning)
            return;

        _timer.Start(Step);
        IsRunning = true;
    }

    // Stop simulation
    private void Stop()
    {
        if (!IsRunning)
            return;

        _timer.Stop();
        IsRunning = false;
    }
}