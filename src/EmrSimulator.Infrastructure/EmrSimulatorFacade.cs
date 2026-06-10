using EmrSimulator.Application;
using EmrSimulator.Application.Repositories;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure;

public sealed class EmrSimulatorFacade(
    InMemoryEmrSimulatorStore store,
    IPatientRepository? patientRepository = null,
    IAppointmentRepository? appointmentRepository = null,
    IOrderRepository? orderRepository = null,
    IResultRepository? resultRepository = null) : IEmrSimulatorFacade
{
    public IReadOnlyList<ProviderSelectionDto> GetProviders()
        => store.Profiles.Select(profile => new ProviderSelectionDto(profile.Name, profile.Enabled ? "Available" : "Disabled")).ToList();

    public ProviderSelectionDto GetActiveProvider()
        => new(store.ActiveProfile.Name, $"{store.ActiveProfile.Name} is active");

    public ProviderSelectionDto SetActiveProvider(EmrProviderType provider)
    {
        store.SetActiveProfile(provider);
        return GetActiveProvider();
    }

    public IReadOnlyList<PatientDto> GetPatients()
    {
        var patients = patientRepository?.GetAll() ?? store.Patients.ToList();
        return patients.Select(ToDto).ToList();
    }

    public PatientDto? GetPatient(Guid id)
    {
        var patient = patientRepository?.GetById(id) ?? store.Patients.FirstOrDefault(p => p.Id == id);
        return patient is null ? null : ToDto(patient);
    }

    public IReadOnlyList<AppointmentDto> GetAppointments()
    {
        var appointments = appointmentRepository?.GetAll() ?? store.Appointments.ToList();
        return appointments.Select(a => new AppointmentDto(a.Id, a.PatientId, a.StartTimeUtc, a.EndTimeUtc, a.ProviderName, a.Status)).ToList();
    }

    public IReadOnlyList<OrderDto> GetOrders()
    {
        var orders = orderRepository?.GetAll() ?? store.Orders.ToList();
        return orders.Select(o => new OrderDto(o.Id, o.PatientId, o.OrderType, o.Status, o.PlacedAtUtc)).ToList();
    }

    public IReadOnlyList<ResultDto> GetResults()
    {
        var results = resultRepository?.GetAll() ?? store.Results.ToList();
        return results.Select(r => new ResultDto(r.Id, r.PatientId, r.OrderId, r.ResultType, r.Value, r.ResultedAtUtc)).ToList();
    }

    public IReadOnlyList<ScenarioDto> GetScenarios()
        => store.Scenarios.Select(s => new ScenarioDto(s.Id, s.Name, s.ScenarioType, s.IsActive, s.Seed)).ToList();

    public ScenarioDto SetActiveScenario(ScenarioType scenarioType)
    {
        store.SetActiveScenario(scenarioType);
        var scenario = store.ActiveScenario;
        return new ScenarioDto(scenario.Id, scenario.Name, scenario.ScenarioType, scenario.IsActive, scenario.Seed);
    }

    public ProviderRouteResult ExecuteProviderRoute(string provider, string route, string method, string? patientId = null, string? requestBody = null)
    {
        var activeScenario = store.ActiveScenario;
        var providerName = NormalizeProvider(provider);

        var result = activeScenario.ScenarioType switch
        {
            ScenarioType.PatientNotFound => new ProviderRouteResult(404, providerName, route, null, "Patient not found"),
            ScenarioType.InvalidCredentials => new ProviderRouteResult(401, providerName, route, null, "Invalid credentials"),
            ScenarioType.Unauthorized => new ProviderRouteResult(403, providerName, route, null, "Unauthorized"),
            ScenarioType.Timeout => new ProviderRouteResult(504, providerName, route, null, "Timed out"),
            ScenarioType.ServerError => new ProviderRouteResult(500, providerName, route, null, "Server error"),
            ScenarioType.RateLimited => new ProviderRouteResult(429, providerName, route, null, "Rate limited"),
            ScenarioType.MalformedResponse => new ProviderRouteResult(200, providerName, route, new { provider = providerName, malformed = true }, null),
            _ => ExecuteHappyPath(providerName, route, patientId)
        };

        store.AddLog(new RequestLog
        {
            Provider = providerName,
            Route = route,
            Method = method,
            RequestHeadersJson = "{}",
            RequestBody = requestBody,
            ResponseBody = store.Serialize(result.Payload ?? new { error = result.Error }),
            ResponseCode = result.StatusCode,
            DurationMs = 15,
            ScenarioId = activeScenario.Id
        });

        return result;
    }

    public ImportReport ImportPatients(string sourceFormat, string content)
    {
        var rows = new List<ImportRowResult>();
        var accepted = 0;
        var rejected = 0;

        foreach (var line in content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select((text, index) => (text, RowNumber: index + 1)))
        {
            var parts = line.text.Split(',', StringSplitOptions.TrimEntries);
            if (parts.Length < 5)
            {
                rows.Add(new ImportRowResult(line.RowNumber, false, "Required fields missing", null));
                rejected++;
                continue;
            }

            var externalPatientId = parts[0];
            var mrn = parts[1];
            var firstName = parts[2];
            var lastName = parts[3];
            var dobText = parts[4];

            var existsInRepository = (patientRepository?.ExistsByExternalId(externalPatientId) ?? false)
                || (patientRepository?.ExistsByMrn(mrn) ?? false);

            if (existsInRepository || store.HasPatientExternalIdOrMrn(externalPatientId, mrn))
            {
                rows.Add(new ImportRowResult(line.RowNumber, false, "Duplicate record", null));
                rejected++;
                continue;
            }

            if (!DateOnly.TryParse(dobText, out var dob))
            {
                rows.Add(new ImportRowResult(line.RowNumber, false, "Invalid date of birth", null));
                rejected++;
                continue;
            }

            var patient = new Patient
            {
                ExternalPatientId = externalPatientId,
                Mrn = mrn,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dob,
                Gender = parts.Length > 5 ? parts[5] : "Unknown"
            };

            if (patientRepository is not null)
            {
                patientRepository.Add(patient);
            }
            else
            {
                store.AddPatient(patient);
            }

            rows.Add(new ImportRowResult(line.RowNumber, true, null, ToDto(patient)));
            accepted++;
        }

        store.AddLog(new RequestLog
        {
            Provider = store.ActiveProfile.Name,
            Route = "/api/v1/import/patients",
            Method = "POST",
            RequestHeadersJson = "{}",
            RequestBody = content,
            ResponseBody = store.Serialize(rows),
            ResponseCode = 200,
            DurationMs = 25,
            ScenarioId = store.ActiveScenario.Id
        });

        return new ImportReport(sourceFormat, accepted, rejected, rows);
    }

    public IReadOnlyList<RequestLogDto> GetRequestLogs()
        => store.Logs.Select(log => new RequestLogDto(log.Id, log.Provider, log.Route, log.Method, log.RequestHeadersJson, log.RequestBody, log.ResponseBody, log.ResponseCode, log.DurationMs, log.ScenarioId, log.CreatedAtUtc)).ToList();

    private static PatientDto ToDto(Patient patient)
        => new(patient.Id, patient.ExternalPatientId, patient.Mrn, patient.FirstName, patient.LastName, patient.DateOfBirth, patient.Gender, patient.Phone, patient.Email);

    private ProviderRouteResult ExecuteHappyPath(string provider, string route, string? patientId)
    {
        if (route.Contains("patients/search", StringComparison.OrdinalIgnoreCase))
        {
            return new ProviderRouteResult(200, provider, route, new { provider, patients = GetPatients() });
        }

        if (Guid.TryParse(patientId, out var id))
        {
            var patient = GetPatient(id);
            return patient is null
                ? new ProviderRouteResult(404, provider, route, null, "Patient not found")
                : new ProviderRouteResult(200, provider, route, patient);
        }

        return new ProviderRouteResult(200, provider, route, new { provider, status = "ok" });
    }

    private static string NormalizeProvider(string provider)
        => provider.Replace("-", string.Empty, StringComparison.OrdinalIgnoreCase).Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase).ToLowerInvariant();
}
