# Tasks: External EMR Endpoint Simulator

**Input**: Design documents from `specs/004-external-emr-endpoints/`
**Prerequisites**: [plan.md](plan.md), [spec.md](spec.md), [research.md](research.md), [data-model.md](data-model.md), [contracts/external-emr-compatibility-contract.md](contracts/external-emr-compatibility-contract.md), [quickstart.md](quickstart.md)
**Tests**: Required by specification success criteria and plan verification requirements.
**Organization**: Tasks are grouped by user story to enable independent implementation and testing.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase
- **[Story]**: Maps tasks to user stories from [spec.md](spec.md)
- Every task includes an exact file path

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare project configuration, seed files, and baseline references for native endpoint compatibility work.

- [ ] T001 Add external EMR feature marker and source-doc references in `specs/004-external-emr-endpoints/verification/coverage-source-map.md`
- [ ] T002 [P] Add native protocol configuration defaults for HTTP/SOAP/HL7 listeners in `src/EmrSimulator.Api/appsettings.Development.json`
- [ ] T003 [P] Add endpoint-catalog seed JSON skeleton in `src/EmrSimulator.Infrastructure/SeedData/external-emr-endpoint-contracts.json`
- [ ] T004 [P] Add synthetic credential seed JSON skeleton in `src/EmrSimulator.Infrastructure/SeedData/synthetic-provider-credentials.json`
- [ ] T005 [P] Add feature verification checklist scaffold in `specs/004-external-emr-endpoints/verification/implementation-verification.md`
- [ ] T006 [P] Add external EMR smoke sample placeholders in `specs/004-external-emr-endpoints/verification/protocol-smoke-samples.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core catalog, persistence, validation, auth, scenario-state, logging, and reset infrastructure required by all user stories.

**CRITICAL**: No user story work can begin until this phase is complete.

- [ ] T007 Create endpoint contract domain enums and entities in `src/EmrSimulator.Domain/EndpointContracts.cs`
- [ ] T008 [P] Create provider profile and synthetic credential domain entities in `src/EmrSimulator.Domain/ProviderProfiles.cs`
- [ ] T009 [P] Create synthetic patient/report/device/document state entities in `src/EmrSimulator.Domain/SyntheticWorkflowState.cs`
- [ ] T010 [P] Create HL7 message and verification evidence entities in `src/EmrSimulator.Domain/VerificationState.cs`
- [ ] T011 Add endpoint contract and provider profile DTOs in `src/EmrSimulator.Contracts/EndpointContractDtos.cs`
- [ ] T012 [P] Add synthetic auth, reset, request log, and verification DTOs in `src/EmrSimulator.Contracts/SimulatorOperationDtos.cs`
- [ ] T013 Add protocol-neutral Application interfaces for catalog, dispatch, validation, auth, state, reset, logging, and evidence in `src/EmrSimulator.Application/ExternalEmrContracts.cs`
- [ ] T014 [P] Add repository interfaces for endpoint contracts and verification evidence in `src/EmrSimulator.Application/Repositories/IEndpointContractRepository.cs`
- [ ] T015 [P] Add repository interfaces for synthetic workflow state in `src/EmrSimulator.Application/Repositories/ISyntheticStateRepository.cs`
- [ ] T016 Add EF Core DbSets for endpoint contracts, credentials, workflow state, and evidence in `src/EmrSimulator.Infrastructure/Persistence/EmrSimulatorDbContext.cs`
- [ ] T017 [P] Add Fluent API mappings for endpoint contracts in `src/EmrSimulator.Infrastructure/Persistence/Configurations/EndpointContractConfiguration.cs`
- [ ] T018 [P] Add Fluent API mappings for provider profiles and credentials in `src/EmrSimulator.Infrastructure/Persistence/Configurations/ProviderProfileConfiguration.cs`
- [ ] T019 [P] Add Fluent API mappings for synthetic workflow state in `src/EmrSimulator.Infrastructure/Persistence/Configurations/SyntheticWorkflowStateConfiguration.cs`
- [ ] T020 [P] Add Fluent API mappings for HL7 messages and verification evidence in `src/EmrSimulator.Infrastructure/Persistence/Configurations/VerificationStateConfiguration.cs`
- [ ] T021 Implement endpoint catalog repository and seed loader in `src/EmrSimulator.Infrastructure/Persistence/EfEndpointContractRepository.cs`
- [ ] T022 [P] Implement synthetic workflow state repository in `src/EmrSimulator.Infrastructure/Persistence/EfSyntheticStateRepository.cs`
- [ ] T023 [P] Implement verification evidence repository in `src/EmrSimulator.Infrastructure/Persistence/EfVerificationEvidenceRepository.cs`
- [ ] T024 Implement tolerant contract validation service in `src/EmrSimulator.Infrastructure/Validation/ContractValidationService.cs`
- [ ] T025 [P] Implement synthetic authentication service in `src/EmrSimulator.Infrastructure/Auth/SyntheticAuthenticationService.cs`
- [ ] T026 [P] Implement request logging service with endpoint contract metadata in `src/EmrSimulator.Infrastructure/Logging/ExternalEmrRequestLogger.cs`
- [ ] T027 Implement deterministic scenario state service and operator reset service in `src/EmrSimulator.Infrastructure/Scenarios/SyntheticScenarioStateService.cs`
- [ ] T028 Register foundational services and hosted protocol options in `src/EmrSimulator.Infrastructure/ServiceCollectionExtensions.cs`
- [ ] T029 Add admin/control endpoints for catalog, verification evidence, and reset in `src/EmrSimulator.Api/Program.cs`
- [ ] T030 [P] Add endpoint catalog schema tests in `tests/EmrSimulator.Tests.Unit/Persistence/EndpointContractConfigurationTests.cs`
- [ ] T031 [P] Add tolerant contract validation tests in `tests/EmrSimulator.Tests.Unit/ContractValidationTests.cs`
- [ ] T032 [P] Add synthetic authentication tests in `tests/EmrSimulator.Tests.Unit/SyntheticAuthenticationTests.cs`
- [ ] T033 Add persistence and reset integration tests in `tests/EmrSimulator.Tests.Integration/SyntheticStatePersistenceTests.cs`

**Checkpoint**: Foundation ready - user story implementation can now begin in priority order or in parallel by provider family.

---

## Phase 3: User Story 1 - Connect Through Epic Workflows (Priority: P1) MVP

**Goal**: Existing Epic connector workflows can use the simulator for launch, OAuth/FHIR, PDF conversion, reports, device operations, authentication, and launcher registration without connector or bridge changes.

**Independent Test**: Configure an Epic-style connector profile to use the simulator and complete launch, token, patient/report lookup, report save/retrieve, device start/abort, PDF conversion, and close workflows using synthetic data only.

### Tests for User Story 1

- [ ] T034 [P] [US1] Add Epic endpoint catalog coverage tests in `tests/EmrSimulator.Tests.Contracts/Epic/EpicEndpointCatalogTests.cs`
- [ ] T035 [P] [US1] Add Epic launch and OAuth integration tests in `tests/EmrSimulator.Tests.Integration/Epic/EpicLaunchOAuthTests.cs`
- [ ] T036 [P] [US1] Add Epic FHIR resource response contract tests in `tests/EmrSimulator.Tests.Contracts/Epic/EpicFhirContractTests.cs`
- [ ] T037 [P] [US1] Add Epic reports and devices integration tests in `tests/EmrSimulator.Tests.Integration/Epic/EpicReportDeviceWorkflowTests.cs`
- [ ] T038 [P] [US1] Add Epic protected-flow auth failure tests in `tests/EmrSimulator.Tests.Integration/Epic/EpicAuthFailureTests.cs`

### Implementation for User Story 1

- [ ] T039 [US1] Seed Epic launch, OAuth, FHIR, PDF, report, device, auth, and register catalog entries in `src/EmrSimulator.Infrastructure/SeedData/external-emr-endpoint-contracts.json`
- [ ] T040 [P] [US1] Add Epic contract response DTOs and sample builders in `src/EmrSimulator.Contracts/Epic/EpicContracts.cs`
- [ ] T041 [P] [US1] Add Epic Application handler interface in `src/EmrSimulator.Application/Providers/Epic/IEpicSimulatorService.cs`
- [ ] T042 [US1] Implement Epic session, OAuth, and token behavior in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicLaunchOAuthService.cs`
- [ ] T043 [US1] Implement Epic FHIR metadata, patient, observation, diagnostic report, and binary response behavior in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicFhirService.cs`
- [ ] T044 [US1] Implement Epic PDF conversion behavior in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicPdfService.cs`
- [ ] T045 [US1] Implement Epic report save/list/retrieve/data-file/review/compare/convert behavior in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicReportsService.cs`
- [ ] T046 [US1] Implement Epic device start/abort and launcher registration behavior in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicDeviceWorkflowService.cs`
- [ ] T047 [US1] Map native Epic compatibility routes in `src/EmrSimulator.Api/EndpointMapping/EpicEndpointMappings.cs`
- [ ] T048 [US1] Register Epic services and route mappings in `src/EmrSimulator.Api/Program.cs`
- [ ] T049 [US1] Record Epic request logs and verification evidence in `src/EmrSimulator.Infrastructure/Providers/Epic/EpicVerificationRecorder.cs`
- [ ] T050 [US1] Document Epic local connector configuration in `specs/004-external-emr-endpoints/verification/epic-smoke-test.md`

**Checkpoint**: Epic MVP is independently runnable and testable without Cerner, Athena, or Altera implementation.

---

## Phase 4: User Story 2 - Connect Through Cerner Workflows (Priority: P2)

**Goal**: Existing Cerner CareAware/VitalsLink, HL7, and Midmark-facing Cerner service workflows can target the simulator using native REST and TCP/MLLP behavior.

**Independent Test**: Configure a Cerner-style connector profile to authenticate, resolve barcode/personnel, retrieve locations/encounters/patients, register and heartbeat a device, post vitals, exchange HL7 messages, and use ADT/physician/HL7 service routes against the simulator.

### Tests for User Story 2

- [ ] T051 [P] [US2] Add Cerner endpoint catalog coverage tests in `tests/EmrSimulator.Tests.Contracts/Cerner/CernerEndpointCatalogTests.cs`
- [ ] T052 [P] [US2] Add VitalsLink REST contract tests in `tests/EmrSimulator.Tests.Contracts/Cerner/VitalsLinkContractTests.cs`
- [ ] T053 [P] [US2] Add VitalsLink workflow integration tests in `tests/EmrSimulator.Tests.Integration/Cerner/VitalsLinkWorkflowTests.cs`
- [ ] T054 [P] [US2] Add HL7 TCP/MLLP ACK/NAK smoke tests in `tests/EmrSimulator.Tests.Integration/Cerner/Hl7MllpListenerTests.cs`
- [ ] T055 [P] [US2] Add Cerner Midmark-facing service route tests in `tests/EmrSimulator.Tests.Contracts/Cerner/CernerMidmarkServiceTests.cs`

### Implementation for User Story 2

- [ ] T056 [US2] Seed Cerner VitalsLink, HL7, and Midmark-facing service catalog entries in `src/EmrSimulator.Infrastructure/SeedData/external-emr-endpoint-contracts.json`
- [ ] T057 [P] [US2] Add Cerner contract response DTOs and sample builders in `src/EmrSimulator.Contracts/Cerner/CernerContracts.cs`
- [ ] T058 [P] [US2] Add Cerner Application handler interfaces in `src/EmrSimulator.Application/Providers/Cerner/ICernerSimulatorService.cs`
- [ ] T059 [US2] Implement Cerner VitalsLink authentication and tenant header behavior in `src/EmrSimulator.Infrastructure/Providers/Cerner/VitalsLinkAuthService.cs`
- [ ] T060 [US2] Implement Cerner barcode, personnel, location, encounter, and patient behavior in `src/EmrSimulator.Infrastructure/Providers/Cerner/VitalsLinkClinicalService.cs`
- [ ] T061 [US2] Implement Cerner device registration, heartbeat, vitals posting, and device removal behavior in `src/EmrSimulator.Infrastructure/Providers/Cerner/VitalsLinkDeviceService.cs`
- [ ] T062 [US2] Implement HL7 MLLP framing, parsing, ACK/NAK, and message persistence in `src/EmrSimulator.Infrastructure/Hl7/Hl7MllpService.cs`
- [ ] T063 [US2] Add hosted HL7 TCP listener configuration and lifecycle in `src/EmrSimulator.Infrastructure/Hl7/Hl7MllpHostedService.cs`
- [ ] T064 [US2] Implement Cerner ADT patient, physician, HL7 message, and pending-test service behavior in `src/EmrSimulator.Infrastructure/Providers/Cerner/CernerMidmarkService.cs`
- [ ] T065 [US2] Map native Cerner VitalsLink and Midmark-facing HTTP routes in `src/EmrSimulator.Api/EndpointMapping/CernerEndpointMappings.cs`
- [ ] T066 [US2] Register Cerner services, routes, and HL7 hosted service in `src/EmrSimulator.Api/Program.cs`
- [ ] T067 [US2] Record Cerner REST and HL7 verification evidence in `src/EmrSimulator.Infrastructure/Providers/Cerner/CernerVerificationRecorder.cs`
- [ ] T068 [US2] Document Cerner REST and HL7 local connector configuration in `specs/004-external-emr-endpoints/verification/cerner-smoke-test.md`

**Checkpoint**: Cerner workflows are independently runnable and do not require Epic, Athena, or Altera implementation.

---

## Phase 5: User Story 3 - Connect Through Unity and Framework Workflows (Priority: P3)

**Goal**: Athena/Centricity and Altera/Allscripts Unity, framework, browser route, and data-source workflows can call SOAP/XML-compatible simulator surfaces and receive contract-valid synthetic responses.

**Independent Test**: Configure Athena/Centricity and Altera/Allscripts connector profiles to call Unity operations, framework ASMX-style operations, browser routes, and data-source simulations, then verify XML, JSON, URL, file-block, report, calibration, and settings responses.

### Tests for User Story 3

- [ ] T069 [P] [US3] Add Athena/Centricity endpoint catalog coverage tests in `tests/EmrSimulator.Tests.Contracts/Unity/AthenaEndpointCatalogTests.cs`
- [ ] T070 [P] [US3] Add Altera/Allscripts endpoint catalog coverage tests in `tests/EmrSimulator.Tests.Contracts/Unity/AlteraEndpointCatalogTests.cs`
- [ ] T071 [P] [US3] Add Unity SOAP/XML envelope integration tests in `tests/EmrSimulator.Tests.Integration/Unity/UnitySoapEnvelopeTests.cs`
- [ ] T072 [P] [US3] Add Altera ASMX framework method tests in `tests/EmrSimulator.Tests.Integration/Unity/AlteraFrameworkAsmxTests.cs`
- [ ] T073 [P] [US3] Add Unity token and auth failure tests in `tests/EmrSimulator.Tests.Integration/Unity/UnityAuthenticationTests.cs`

### Implementation for User Story 3

- [ ] T074 [US3] Seed Athena Unity, Athena data-source, Altera Unity, Altera framework, and Altera browser-route catalog entries in `src/EmrSimulator.Infrastructure/SeedData/external-emr-endpoint-contracts.json`
- [ ] T075 [P] [US3] Add Unity and ASMX contract DTOs/envelope builders in `src/EmrSimulator.Contracts/Unity/UnityContracts.cs`
- [ ] T076 [P] [US3] Add Unity and framework Application handler interfaces in `src/EmrSimulator.Application/Providers/Unity/IUnitySimulatorService.cs`
- [ ] T077 [US3] Implement SOAP/XML envelope parsing and response envelope generation in `src/EmrSimulator.Infrastructure/Soap/SoapEnvelopeService.cs`
- [ ] T078 [US3] Implement Athena Unity token, Magic, clinical summary, document, and data-source behavior in `src/EmrSimulator.Infrastructure/Providers/Athena/AthenaUnityService.cs`
- [ ] T079 [US3] Implement Altera Unity token, Magic, ReturnMagicJSON, validation, and retire-token behavior in `src/EmrSimulator.Infrastructure/Providers/Altera/AlteraUnityService.cs`
- [ ] T080 [US3] Implement Altera framework ASMX file, report, XBAP, calibration, settings, and notification behavior in `src/EmrSimulator.Infrastructure/Providers/Altera/AlteraFrameworkService.cs`
- [ ] T081 [US3] Implement Altera browser route deterministic URL outcomes in `src/EmrSimulator.Infrastructure/Providers/Altera/AlteraBrowserRouteService.cs`
- [ ] T082 [US3] Map Unity, ASMX, and browser compatibility routes in `src/EmrSimulator.Api/EndpointMapping/UnityEndpointMappings.cs`
- [ ] T083 [US3] Register Athena, Altera, SOAP/XML, and framework services in `src/EmrSimulator.Api/Program.cs`
- [ ] T084 [US3] Record Unity/framework verification evidence in `src/EmrSimulator.Infrastructure/Providers/Unity/UnityVerificationRecorder.cs`
- [ ] T085 [US3] Document Athena and Altera local connector configuration in `specs/004-external-emr-endpoints/verification/unity-framework-smoke-test.md`

**Checkpoint**: Athena/Centricity and Altera/Allscripts workflows are independently runnable with SOAP/XML-compatible contracts.

---

## Phase 6: User Story 4 - Configure and Verify Endpoint Coverage (Priority: P4)

**Goal**: Operators can configure provider scenarios, reset state, inspect endpoint coverage, review request logs, and view verification evidence for all documented endpoint groups.

**Independent Test**: Load the endpoint inventory, select provider scenarios, send representative requests for every endpoint group, and verify the UI/API show implemented, passing, failing, and untested coverage plus request log evidence.

### Tests for User Story 4

- [ ] T086 [P] [US4] Add endpoint coverage API integration tests in `tests/EmrSimulator.Tests.Integration/Admin/EndpointCoverageApiTests.cs`
- [ ] T087 [P] [US4] Add reset API integration tests in `tests/EmrSimulator.Tests.Integration/Admin/SimulatorResetApiTests.cs`
- [ ] T088 [P] [US4] Add verification evidence API tests in `tests/EmrSimulator.Tests.Integration/Admin/VerificationEvidenceApiTests.cs`
- [ ] T089 [P] [US4] Add Admin UI coverage component tests in `src/EmrSimulator.AdminUi/src/app/endpoint-coverage/endpoint-coverage.component.spec.ts`
- [ ] T090 [P] [US4] Add Admin UI reset/log component tests in `src/EmrSimulator.AdminUi/src/app/request-logs/request-logs.component.spec.ts`

### Implementation for User Story 4

- [ ] T091 [US4] Add endpoint coverage and verification methods to facade contract in `src/EmrSimulator.Application/ApplicationContracts.cs`
- [ ] T092 [US4] Implement endpoint coverage, verification evidence, and reset facade behavior in `src/EmrSimulator.Infrastructure/EmrSimulatorFacade.cs`
- [ ] T093 [US4] Add admin/control API DTO mappings for coverage, evidence, and reset in `src/EmrSimulator.Api/EndpointMapping/AdminEndpointMappings.cs`
- [ ] T094 [US4] Add endpoint coverage API client methods in `src/EmrSimulator.AdminUi/src/app/services/emr-simulator-api.service.ts`
- [ ] T095 [P] [US4] Add endpoint coverage TypeScript models in `src/EmrSimulator.AdminUi/src/app/models/endpoint-coverage.model.ts`
- [ ] T096 [US4] Implement endpoint coverage view in `src/EmrSimulator.AdminUi/src/app/endpoint-coverage/endpoint-coverage.component.ts`
- [ ] T097 [US4] Implement endpoint coverage template in `src/EmrSimulator.AdminUi/src/app/endpoint-coverage/endpoint-coverage.component.html`
- [ ] T098 [US4] Implement endpoint coverage styles in `src/EmrSimulator.AdminUi/src/app/endpoint-coverage/endpoint-coverage.component.scss`
- [ ] T099 [US4] Add reset controls and verification evidence to request logs view in `src/EmrSimulator.AdminUi/src/app/request-logs/request-logs.component.ts`
- [ ] T100 [US4] Add navigation route for endpoint coverage in `src/EmrSimulator.AdminUi/src/app/app.routes.ts`
- [ ] T101 [US4] Update Admin UI shell navigation for endpoint coverage in `src/EmrSimulator.AdminUi/src/app/app.html`
- [ ] T102 [US4] Document operator coverage and reset workflow in `specs/004-external-emr-endpoints/verification/operator-coverage-smoke-test.md`

**Checkpoint**: Operators can verify endpoint coverage and troubleshoot requests across all provider families.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Final hardening, docs, validation, and compatibility evidence across all stories.

- [ ] T103 [P] Update local run and connector configuration instructions in `README.md`
- [ ] T104 [P] Update source endpoint implementation status notes in `.docs/external-emr-endpoints.md`
- [ ] T105 [P] Update contract inventory implementation status notes in `.docs/external-emr-api-contracts.md`
- [ ] T106 Add Swagger/OpenAPI summaries for admin/control and HTTP compatibility routes in `src/EmrSimulator.Api/Program.cs`
- [ ] T107 Add complete endpoint coverage verification results in `specs/004-external-emr-endpoints/verification/coverage-results.md`
- [ ] T108 Run and record backend test results in `specs/004-external-emr-endpoints/verification/implementation-verification.md`
- [ ] T109 Run and record Admin UI build result in `specs/004-external-emr-endpoints/verification/implementation-verification.md`
- [ ] T110 Run and record native protocol smoke results in `specs/004-external-emr-endpoints/verification/protocol-smoke-samples.md`
- [ ] T111 Validate final constitution gates in `specs/004-external-emr-endpoints/verification/constitution-gates.md`
- [ ] T112 Run final Speckit consistency analysis and record outcome in `specs/004-external-emr-endpoints/verification/implementation-verification.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies; can start immediately.
- **Phase 2 Foundational**: Depends on Phase 1; blocks all user stories.
- **Phase 3 US1 Epic MVP**: Depends on Phase 2; first deliverable MVP.
- **Phase 4 US2 Cerner**: Depends on Phase 2; can run in parallel with US1 after foundation but should integrate shared state/logging carefully.
- **Phase 5 US3 Unity and Framework**: Depends on Phase 2; can run in parallel with US1/US2 after foundation.
- **Phase 6 US4 Operator Coverage**: Depends on Phase 2 for API foundations and benefits from US1-US3 for complete provider coverage.
- **Phase 7 Polish**: Depends on desired user stories being complete.

### User Story Dependencies

- **US1 Epic (P1)**: No dependency on other stories after foundation; MVP scope.
- **US2 Cerner (P2)**: No dependency on US1 after foundation; shares endpoint catalog, auth, state, logging, and evidence services.
- **US3 Unity and Framework (P3)**: No dependency on US1/US2 after foundation; shares endpoint catalog, auth, state, logging, and evidence services.
- **US4 Operator Coverage (P4)**: Can start after foundation for catalog/reset/log API, but full coverage evidence requires provider stories to be implemented.

### Within Each User Story

- Tests before implementation and should fail before story code lands.
- Catalog seed before provider services.
- Provider contracts and Application interfaces before Infrastructure handlers.
- Infrastructure behavior before Api route mappings.
- Route mappings before smoke documentation and evidence.
- Story checkpoint validation before moving to the next sequential priority.

## Parallel Opportunities

- Setup tasks T002-T006 can run in parallel.
- Foundational entity/config/test tasks marked [P] can run in parallel after T007 where relevant.
- US1 tests T034-T038 can run in parallel; Epic DTO/interface tasks T040-T041 can run in parallel before service implementation.
- US2 tests T051-T055 can run in parallel; Cerner DTO/interface tasks T057-T058 can run in parallel before service implementation.
- US3 tests T069-T073 can run in parallel; Unity DTO/interface tasks T075-T076 can run in parallel before service implementation.
- US4 tests T086-T090 can run in parallel; TypeScript model task T095 can run in parallel with API work after service contracts exist.
- Polish docs T103-T105 can run in parallel after provider story behavior is known.

## Parallel Example: User Story 1

```powershell
# Epic test work can be split across files:
Task T034: tests/EmrSimulator.Tests.Contracts/Epic/EpicEndpointCatalogTests.cs
Task T035: tests/EmrSimulator.Tests.Integration/Epic/EpicLaunchOAuthTests.cs
Task T036: tests/EmrSimulator.Tests.Contracts/Epic/EpicFhirContractTests.cs
Task T037: tests/EmrSimulator.Tests.Integration/Epic/EpicReportDeviceWorkflowTests.cs
Task T038: tests/EmrSimulator.Tests.Integration/Epic/EpicAuthFailureTests.cs

# Epic contracts/interfaces can begin in parallel:
Task T040: src/EmrSimulator.Contracts/Epic/EpicContracts.cs
Task T041: src/EmrSimulator.Application/Providers/Epic/IEpicSimulatorService.cs
```

## Parallel Example: User Story 2

```powershell
Task T052: tests/EmrSimulator.Tests.Contracts/Cerner/VitalsLinkContractTests.cs
Task T054: tests/EmrSimulator.Tests.Integration/Cerner/Hl7MllpListenerTests.cs
Task T057: src/EmrSimulator.Contracts/Cerner/CernerContracts.cs
Task T058: src/EmrSimulator.Application/Providers/Cerner/ICernerSimulatorService.cs
```

## Parallel Example: User Story 3

```powershell
Task T071: tests/EmrSimulator.Tests.Integration/Unity/UnitySoapEnvelopeTests.cs
Task T072: tests/EmrSimulator.Tests.Integration/Unity/AlteraFrameworkAsmxTests.cs
Task T075: src/EmrSimulator.Contracts/Unity/UnityContracts.cs
Task T076: src/EmrSimulator.Application/Providers/Unity/IUnitySimulatorService.cs
```

## Implementation Strategy

### MVP First (US1 Epic Only)

1. Complete Phase 1 setup.
2. Complete Phase 2 foundational catalog, persistence, validation, auth, scenario state, logging, reset, and evidence services.
3. Complete Phase 3 Epic MVP.
4. Stop and validate Epic launch, OAuth/FHIR, reports, device workflows, auth failures, request logs, and verification evidence independently.
5. Demo an Epic-style connector smoke workflow before adding other provider families.

### Incremental Delivery

1. Foundation ready: catalog, persistence, synthetic auth, scenario state, logs, reset, and evidence.
2. Add US1 Epic and validate MVP.
3. Add US2 Cerner REST/HL7 and validate independently.
4. Add US3 Athena/Altera Unity/framework and validate independently.
5. Add US4 operator coverage UI/API and validate complete coverage evidence.
6. Polish docs, Swagger, quickstart, and final Speckit analysis.

### Parallel Team Strategy

1. Team completes Setup and Foundational phases together.
2. After foundation, split by provider family:
   - Developer A: US1 Epic
   - Developer B: US2 Cerner REST/HL7
   - Developer C: US3 Unity/framework
   - Developer D: US4 coverage UI/API after catalog API stabilizes
3. Integrate through shared endpoint catalog, auth, state, logging, and evidence services.

## Notes

- Tasks preserve connector-facing contracts as-is; do not create connector bridges unless a later approved plan changes scope.
- Admin/control API tasks stay under `/api/v1`; native connector surfaces may use documented non-`/api/v1` paths because they are provider contract surfaces.
- All new EF Core mappings must use Fluent API.
- All default credentials and data must be synthetic.
- Every completed provider story must update request logging and verification evidence.
- Stop at checkpoints to validate each story independently.
