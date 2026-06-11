using EmrSimulator.Application;
using EmrSimulator.Application.Providers.Cerner;
using EmrSimulator.Application.Providers.Epic;
using EmrSimulator.Application.Providers.Unity;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Infrastructure.Hl7;
using EmrSimulator.Infrastructure.Auth;
using EmrSimulator.Infrastructure.ExternalEmr;
using EmrSimulator.Infrastructure.Logging;
using EmrSimulator.Infrastructure.Persistence;
using EmrSimulator.Infrastructure.Providers.Altera;
using EmrSimulator.Infrastructure.Providers.Athena;
using EmrSimulator.Infrastructure.Providers.Cerner;
using EmrSimulator.Infrastructure.Providers.Epic;
using EmrSimulator.Infrastructure.Providers.Unity;
using EmrSimulator.Infrastructure.Scenarios;
using EmrSimulator.Infrastructure.Soap;
using EmrSimulator.Infrastructure.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmrSimulator.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmrSimulatorInfrastructure(
        this IServiceCollection services,
        string? connectionString = null,
        string? contentRootPath = null)
    {
        var cs = SqliteConnectionStringResolver.Resolve(connectionString, contentRootPath ?? Directory.GetCurrentDirectory());
        services.AddDbContext<EmrSimulatorDbContext>(options =>
            options.UseSqlite(cs));

        services.AddSingleton<InMemoryEmrSimulatorStore>();
        services.AddScoped<IPatientRepository, EfPatientRepository>();
        services.AddScoped<IAppointmentRepository, EfAppointmentRepository>();
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IResultRepository, EfResultRepository>();
        services.AddScoped<IEndpointContractRepository, EfEndpointContractRepository>();
        services.AddScoped<IVerificationEvidenceRepository, EfVerificationEvidenceRepository>();
        services.AddScoped<ISyntheticStateRepository, EfSyntheticStateRepository>();
        services.AddScoped<IEndpointCatalogService, EndpointCatalogService>();
        services.AddScoped<IContractValidationService, ContractValidationService>();
        services.AddScoped<ISyntheticAuthenticationService, SyntheticAuthenticationService>();
        services.AddScoped<IVerificationEvidenceService, VerificationEvidenceService>();
        services.AddScoped<ISyntheticScenarioStateService, SyntheticScenarioStateService>();
        services.AddScoped<ExternalEmrRequestLogger>();
        services.AddScoped<EpicLaunchOAuthService>();
        services.AddScoped<IEpicSimulatorService>(provider => provider.GetRequiredService<EpicLaunchOAuthService>());
        services.AddScoped<EpicFhirService>();
        services.AddScoped<EpicPdfService>();
        services.AddScoped<EpicReportsService>();
        services.AddScoped<EpicDeviceWorkflowService>();
        services.AddScoped<EpicVerificationRecorder>();
        services.AddScoped<VitalsLinkAuthService>();
        services.AddScoped<ICernerSimulatorService>(provider => provider.GetRequiredService<VitalsLinkAuthService>());
        services.AddScoped<VitalsLinkClinicalService>();
        services.AddScoped<VitalsLinkDeviceService>();
        services.AddScoped<CernerMidmarkService>();
        services.AddScoped<CernerVerificationRecorder>();
        services.AddSingleton<Hl7MllpService>();
        services.AddHostedService<Hl7MllpHostedService>();
        services.AddScoped<SoapEnvelopeService>();
        services.AddScoped<IUnitySimulatorService>(provider => provider.GetRequiredService<SoapEnvelopeService>());
        services.AddScoped<AthenaUnityService>();
        services.AddScoped<AlteraUnityService>();
        services.AddScoped<AlteraFrameworkService>();
        services.AddScoped<AlteraBrowserRouteService>();
        services.AddScoped<UnityVerificationRecorder>();
        services.AddScoped<IEmrSimulatorFacade, EmrSimulatorFacade>();
        return services;
    }
}
