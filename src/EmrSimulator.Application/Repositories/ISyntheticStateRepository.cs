namespace EmrSimulator.Application.Repositories;

public interface ISyntheticStateRepository
{
    int ResetGeneration { get; }
    int ResetGeneratedState();
}