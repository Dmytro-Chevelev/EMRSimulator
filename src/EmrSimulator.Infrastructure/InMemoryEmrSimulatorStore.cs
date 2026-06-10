using System.Collections.Concurrent;
using System.Text.Json;
using EmrSimulator.Contracts;
using EmrSimulator.Domain;

namespace EmrSimulator.Infrastructure;

public sealed class InMemoryEmrSimulatorStore
{
    private readonly ConcurrentDictionary<Guid, EmrProfile> _profiles = new();
    private readonly ConcurrentDictionary<Guid, Scenario> _scenarios = new();
    private readonly ConcurrentDictionary<Guid, Patient> _patients = new();
    private readonly ConcurrentDictionary<Guid, Appointment> _appointments = new();
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();
    private readonly ConcurrentDictionary<Guid, Result> _results = new();
    private readonly ConcurrentDictionary<Guid, RequestLog> _logs = new();
    private readonly object _sync = new();

    public Guid ActiveProfileId { get; private set; }
    public Guid ActiveScenarioId { get; private set; }

    public InMemoryEmrSimulatorStore()
    {
        Seed();
    }

    public IEnumerable<EmrProfile> Profiles => _profiles.Values.OrderBy(p => p.Name);
    public IEnumerable<Scenario> Scenarios => _scenarios.Values.OrderBy(s => s.Name);
    public IEnumerable<Patient> Patients => _patients.Values.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    public IEnumerable<Appointment> Appointments => _appointments.Values.OrderBy(a => a.StartTimeUtc);
    public IEnumerable<Order> Orders => _orders.Values.OrderBy(o => o.PlacedAtUtc);
    public IEnumerable<Result> Results => _results.Values.OrderByDescending(r => r.ResultedAtUtc);
    public IEnumerable<RequestLog> Logs => _logs.Values.OrderByDescending(l => l.CreatedAtUtc);

    public EmrProfile ActiveProfile => _profiles[ActiveProfileId];
    public Scenario ActiveScenario => _scenarios[ActiveScenarioId];

    public void SetActiveProfile(EmrProviderType provider)
    {
        var profile = _profiles.Values.First(p => p.Provider == provider);
        ActiveProfileId = profile.Id;
    }

    public void SetActiveScenario(ScenarioType scenarioType)
    {
        var scenario = _scenarios.Values.First(s => s.ScenarioType == scenarioType);
        foreach (var entry in _scenarios.Values)
        {
            entry.IsActive = entry.Id == scenario.Id;
            entry.UpdatedAtUtc = DateTime.UtcNow;
        }

        ActiveScenarioId = scenario.Id;
    }

    public void AddPatient(Patient patient)
    {
        _patients[patient.Id] = patient;
    }

    public void AddLog(RequestLog log)
    {
        _logs[log.Id] = log;
    }

    public bool HasPatientExternalIdOrMrn(string externalPatientId, string mrn)
        => _patients.Values.Any(p => p.ExternalPatientId.Equals(externalPatientId, StringComparison.OrdinalIgnoreCase)
            || p.Mrn.Equals(mrn, StringComparison.OrdinalIgnoreCase));

    public string Serialize(object value) => JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false });

    private void Seed()
    {
        lock (_sync)
        {
            if (_profiles.Count > 0)
            {
                return;
            }

            var providers = new[]
            {
                new EmrProfile { Name = "Epic", Provider = EmrProviderType.Epic, BaseUrl = "/api/v1/emr/epic" },
                new EmrProfile { Name = "Cerner", Provider = EmrProviderType.Cerner, BaseUrl = "/api/v1/emr/cerner" },
                new EmrProfile { Name = "Altera", Provider = EmrProviderType.Altera, BaseUrl = "/api/v1/emr/altera" },
                new EmrProfile { Name = "Athena Flow", Provider = EmrProviderType.AthenaFlow, BaseUrl = "/api/v1/emr/athena-flow" },
                new EmrProfile { Name = "Athena Server", Provider = EmrProviderType.AthenaServer, BaseUrl = "/api/v1/emr/athena-server" },
            };

            foreach (var profile in providers)
            {
                _profiles[profile.Id] = profile;
            }

            ActiveProfileId = providers[0].Id;

            var scenarios = new[]
            {
                new Scenario { Name = "Happy Path", ScenarioType = ScenarioType.HappyPath, IsActive = true, Seed = "happy-path" },
                new Scenario { Name = "Patient Not Found", ScenarioType = ScenarioType.PatientNotFound, Seed = "patient-not-found" },
                new Scenario { Name = "Invalid Credentials", ScenarioType = ScenarioType.InvalidCredentials, Seed = "invalid-credentials" },
                new Scenario { Name = "Unauthorized", ScenarioType = ScenarioType.Unauthorized, Seed = "unauthorized" },
                new Scenario { Name = "Timeout", ScenarioType = ScenarioType.Timeout, Seed = "timeout" },
                new Scenario { Name = "Server Error", ScenarioType = ScenarioType.ServerError, Seed = "server-error" },
                new Scenario { Name = "Rate Limited", ScenarioType = ScenarioType.RateLimited, Seed = "rate-limited" },
                new Scenario { Name = "Malformed Response", ScenarioType = ScenarioType.MalformedResponse, Seed = "malformed-response" },
            };

            foreach (var scenario in scenarios)
            {
                _scenarios[scenario.Id] = scenario;
            }

            ActiveScenarioId = scenarios[0].Id;

            var patient = new Patient
            {
                ExternalPatientId = "EP-1001",
                Mrn = "MRN-1001",
                FirstName = "Jordan",
                LastName = "Casey",
                DateOfBirth = new DateOnly(1980, 4, 20),
                Gender = "Unknown",
                Phone = "555-0101",
                Email = "jordan.casey@example.invalid"
            };

            _patients[patient.Id] = patient;

            _appointments[new Appointment
            {
                PatientId = patient.Id,
                StartTimeUtc = DateTime.UtcNow.Date.AddHours(13),
                EndTimeUtc = DateTime.UtcNow.Date.AddHours(13).AddMinutes(30),
                ProviderName = "Dr. Avery",
                Status = "Scheduled"
            }.Id] = new Appointment
            {
                PatientId = patient.Id,
                StartTimeUtc = DateTime.UtcNow.Date.AddHours(13),
                EndTimeUtc = DateTime.UtcNow.Date.AddHours(13).AddMinutes(30),
                ProviderName = "Dr. Avery",
                Status = "Scheduled"
            };

            var order = new Order { PatientId = patient.Id, OrderType = "ECG", Status = "Open", PlacedAtUtc = DateTime.UtcNow.AddMinutes(-15) };
            _orders[order.Id] = order;

            var result = new Result { PatientId = patient.Id, OrderId = order.Id, ResultType = "ECG", Value = "Normal", ResultedAtUtc = DateTime.UtcNow.AddMinutes(-5) };
            _results[result.Id] = result;
        }
    }
}
