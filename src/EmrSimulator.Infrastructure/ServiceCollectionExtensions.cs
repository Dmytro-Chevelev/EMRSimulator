using EmrSimulator.Application;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmrSimulatorInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<InMemoryEmrSimulatorStore>();
        services.AddSingleton<IEmrSimulatorFacade, EmrSimulatorFacade>();
        return services;
    }
}
