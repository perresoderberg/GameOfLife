
namespace GameOfLife.Presentation.Services;
public interface ITimerService
{
    void Start(Action tick);
    void Stop();
}