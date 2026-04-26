using GameOfLife.Presentation.Services;

namespace GameOfLife.Tests.Presentation.Fakes
{
    public class FakeTimerService : ITimerService
    {
        public Action? TickAction { get; private set; }
        public bool IsRunning { get; private set; }

        public void Start(Action tick)
        {
            TickAction = tick;
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void TriggerTick()
        {
            if (IsRunning)
                TickAction?.Invoke();
        }
    }
}
