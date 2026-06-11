using EmrSimulator.Application;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Contracts;

namespace EmrSimulator.Infrastructure.Scenarios;

public sealed class SyntheticScenarioStateService(ISyntheticStateRepository repository) : ISyntheticScenarioStateService
{
    public int ResetGeneration => repository.ResetGeneration;

    public SimulatorResetResult Reset()
    {
        var generation = repository.ResetGeneratedState();
        return new SimulatorResetResult(generation, $"Synthetic simulator state reset to generation {generation}.");
    }
}