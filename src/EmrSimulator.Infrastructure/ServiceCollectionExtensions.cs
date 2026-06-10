using EmrSimulator.Application;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmrSimulatorInfrastructure(
        this IServiceCollection services,
        string? connectionString = null)
    {
        var cs = connectionString ?? "Data Source=emrsimulator.db";
        services.AddDbContext<EmrSimulatorDbContext>(options =>
            options.UseSqlite(cs));

        services.AddSingleton<InMemoryEmrSimulatorStore>();
        services.AddScoped<IPatientRepository, EfPatientRepository>();
        services.AddScoped<IAppointmentRepository, EfAppointmentRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IResultRepository, EfResultRepository>();
        services.AddScoped<IEmrSimulatorFacade, EmrSimulatorFacade>();
        return services;
    }
}
