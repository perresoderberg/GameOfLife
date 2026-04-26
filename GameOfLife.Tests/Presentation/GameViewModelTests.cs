using FluentAssertions;
using GameOfLife.Application.Features;
using GameOfLife.Domain.Rules;
using GameOfLife.Presentation.SeedProvider;
using GameOfLife.Presentation.ViewModels;
using GameOfLife.Tests.Presentation.Fakes;
using Xunit;

public class GameViewModelTests
{
    private static GameViewModel CreateViewModel(FakeTimerService timer)
    {
        var rule = new GameOfLifeRule();
        var nextGeneration = new NextGeneration(rule);

        return new GameViewModel(
            width: 5,
            height: 5,
            timer: timer,
            seed: new RandomSeed(new Random(0)),
            nextGeneration: nextGeneration
        );
    }

    [Fact]
    public void Constructor_ShouldInitializeCells()
    {
        var timer = new FakeTimerService();

        var vm = CreateViewModel(timer);

        vm.Cells.Should().NotBeNull();
        vm.Cells.Count.Should().Be(vm.Width * vm.Height);
    }

    [Fact]
    public void StepCommand_ShouldAdvanceSimulation()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        var before = vm.Cells.Select(c => c.IsAlive).ToList();

        vm.StepCommand.Execute(null);

        var after = vm.Cells.Select(c => c.IsAlive).ToList();

        after.Should().NotEqual(before);
    }

    [Fact]
    public void StepCommand_ShouldUpdateCells_NotReplaceCollection()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        var originalReferences = vm.Cells.ToList();

        vm.StepCommand.Execute(null);

        // Same objects (important for WPF binding)
        vm.Cells.Should().BeEquivalentTo(originalReferences, opt => opt.WithStrictOrdering());
    }

    [Fact]
    public void StartCommand_ShouldStartTimer()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        vm.StartCommand.Execute(null);

        timer.IsRunning.Should().BeTrue();
        timer.TickAction.Should().NotBeNull();
    }

    [Fact]
    public void StopCommand_ShouldStopTimer()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        vm.StartCommand.Execute(null);
        vm.StopCommand.Execute(null);

        timer.IsRunning.Should().BeFalse();
    }

    [Fact]
    public void TimerTick_ShouldCallStep_AndUpdateState()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        vm.StartCommand.Execute(null);

        var before = vm.Cells.Select(c => c.IsAlive).ToList();

        timer.TriggerTick();

        var after = vm.Cells.Select(c => c.IsAlive).ToList();

        after.Should().NotEqual(before);
    }

    [Fact]
    public void MultipleTicks_ShouldAdvanceSimulationMultipleSteps()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        vm.StartCommand.Execute(null);

        var before = vm.Cells.Select(c => c.IsAlive).ToList();

        timer.TriggerTick();
        var afterFirst = vm.Cells.Select(c => c.IsAlive).ToList();

        timer.TriggerTick();
        var afterSecond = vm.Cells.Select(c => c.IsAlive).ToList();

        afterFirst.Should().NotEqual(before);
        afterSecond.Should().NotEqual(afterFirst);
    }

    [Fact]
    public void StopCommand_ShouldPreventFurtherTicks()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        vm.StartCommand.Execute(null);
        timer.TriggerTick();

        vm.StopCommand.Execute(null);

        var before = vm.Cells.Select(c => c.IsAlive).ToList();

        timer.TriggerTick(); // should NOT run Step()

        var after = vm.Cells.Select(c => c.IsAlive).ToList();

        after.Should().Equal(before);
    }

    [Fact]
    public void StepCommand_ShouldWorkWithoutTimer()
    {
        var timer = new FakeTimerService();
        var vm = CreateViewModel(timer);

        var before = vm.Cells.Select(c => c.IsAlive).ToList();

        vm.StepCommand.Execute(null);

        var after = vm.Cells.Select(c => c.IsAlive).ToList();

        after.Should().NotEqual(before);
    }
}