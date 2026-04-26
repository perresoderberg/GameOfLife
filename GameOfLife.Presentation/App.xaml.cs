using GameOfLife.Application.Features;
using GameOfLife.Domain.Rules;
using GameOfLife.Presentation.SeedProvider;
using GameOfLife.Presentation.Services;
using GameOfLife.Presentation.ViewModels;
using GameOfLife.Presentation.Views;
using System.Windows;

namespace GameOfLife;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var rule = new GameOfLifeRule();
        var nextGeneration = new NextGeneration(rule);

        var vm = new GameViewModel(
            width: 20,
            height: 20,
            timer: new TimerService(),
            seed: new RandomSeed(new Random()),
            nextGeneration: nextGeneration
        );

        var window = new MainWindow
        {
            DataContext = vm
        };

        window.Show();
    }
}