using System.Windows.Threading;

namespace GameOfLife.Presentation.Services;
public class TimerService : ITimerService
{
    private readonly DispatcherTimer _timer;
    private Action? _tick;

    public TimerService()
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(300)
        };

        _timer.Tick += OnTick;
    }

    public void Start(Action tick)
    {
        _tick = tick;
        _timer.Start();
    }
    public void Stop() => _timer.Stop();
    private void OnTick(object? sender, EventArgs e) => _tick?.Invoke();
}