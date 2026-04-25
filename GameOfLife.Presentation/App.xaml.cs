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

        var timer = new TimerService();
        var vm = new GameViewModel(new Random(), timer);

        var window = new MainWindow
        {
            DataContext = vm
        };

        window.Show();
    }
}