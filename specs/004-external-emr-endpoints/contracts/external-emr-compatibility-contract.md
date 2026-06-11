# Contract: External EMR Compatibility Surface

## Purpose

This contract defines the planned simulator compatibility surface for existing EMR connectors. The implementation must preserve documented connector-facing paths, actions, payload families, auth behavior, and protocol boundaries from `.docs/external-emr-endpoints.md` and `.docs/external-emr-api-contracts.md`. Admin/control APIs may remain under `/api/v1`; connector-facing compatibility surfaces must not require connector or bridge changes.

## Global Contract Rules

- All default data and credentials are synthetic.
- Every protected flow validates provider-compatible synthetic credentials, tokens, or headers.
- Contract validation accepts documented shapes plus known serializer variants: PascalCase or camelCase property names, and string or numeric enum values.
- Provider-facing request and response payloads are represented by typed provider contract DTOs/records, not generic `object` or anonymous payload shapes.
- Every request is logged with provider, endpoint/operation, protocol, scenario, status, validation/auth outcome, correlation/session identifier when available, and redacted metadata.
- Generated reports, device registrations, documents, messages, imported/generated synthetic patients, request logs, and verification evidence persist until operator reset.
- The default SQLite database uses a stable repo-local data folder such as `.data/emrsimulator.db` unless an operator explicitly overrides the connection string.
- Native connector paths and protocol expectations are preserved. Do not rename documented connector endpoints to `/api/v1` equivalents unless the source contract already uses `/api/v1`.

## Admin and Control API Surface

These routes are simulator control APIs and remain versioned under `/api/v1`.

| Route | Purpose |
| --- | --- |
| `GET /api/v1/providers` | List provider profiles and availability |
| `GET /api/v1/providers/active` | Get active provider profile |
| `PUT /api/v1/providers/active/{provider}` | Set active provider profile |
| `GET /api/v1/scenarios` | List deterministic scenarios |
| `PUT /api/v1/scenarios/active/{scenario}` | Set active scenario |
| `GET /api/v1/request-logs` | View simulator request logs |
| `GET /api/v1/endpoint-contracts` | Planned: list coverage catalog from source docs |
| `GET /api/v1/endpoint-contracts/{id}/verification` | Planned: view verification evidence for a contract |
| `POST /api/v1/simulator/reset` | Reset generated synthetic state, imported/generated patients, logs, and evidence, then restore the 15 default seeded synthetic patients |

## Epic Compatibility Surface

### Launch and OAuth/FHIR

| Method | Native path or pattern | Required behavior |
| --- | --- | --- |
| `GET` | `/Midmark?launch={launchToken}&iss={fhirBaseUrl}` | Start synthetic SMART launch session and bind `iss` to simulated FHIR base URL |
| `GET` | `/Midmark/Redirect?code={authorizationCode}&state={sessionId}` | Complete synthetic authorization-code callback and return workflow context |
| `GET` | `/Midmark/Close` | Close/abort launched workflow, honoring `uniqueMDLId` header when present |
| `GET` | `{iss}/metadata` | Return synthetic FHIR capability statement and OAuth endpoint discovery |
| `POST` | `{AuthorizationURI}` | Simulate authorization request target when connector calls discovered URI |
| `POST` | `{TokenURI}` | Return documented token response for authorization-code or backend token flow |
| `POST` | `{BackendAuthUrl}` | Return backend OAuth token response using synthetic client assertion validation |
| `GET` | `{fhirBaseUrl}/{resource}` | Return documented FHIR patient, observation, diagnostic report, binary/PDF, service request, and related resources |
| `GET` | `{BackendFHIRUrl}/{resource}` | Return backend FHIR resources for PDF/report conversion flow |

### Epic PDF, Reports, Devices, Auth, Register

| Method | Native path | Required behavior |
| --- | --- | --- |
| `GET` | `/Pdf/convert?environmentId={environmentId}&userId={userId}&documentId={documentId}` | Return synthetic PDF conversion/retrieval response using documented Basic auth behavior |
| `GET` | `/api/v1/Reports/patientId/{patientId}` | Return reports for patient |
| `GET` | `/api/v1/Reports/ReportType/{reportType}` | Return reports by type using patient context header when supplied |
| `GET` | `/api/v1/Reports/deviceId/{deviceId}` | Return reports by device using patient context header when supplied |
| `GET` | `/api/v1/Reports/reportId/{reportId}` | Return report metadata/content by report ID |
| `POST` | `/api/v1/Reports/SaveReport` | Persist synthetic report and related EMR info |
| `GET` | `/api/v1/Reports/GetDataFile` | Return report data file or block metadata |
| `POST` | `/api/v1/Reports/ReviewReport/{reportId}` | Return review workflow result/context |
| `POST` | `/api/v1/Reports/CompareReports` | Return comparison workflow result/context |
| `POST` | `/api/v1/Reports/Convert/{reportType}/{reportId}` | Return converted report result/context |
| `POST` | `/api/v1/Devices/StartTest` | Start synthetic device workflow and persist device/report state |
| `POST` | `/api/v1/Devices/Abort` | Abort active synthetic device workflow |
| `POST` | `/api/v1/Authenticate/Auth` | Validate synthetic IQconnect authentication |
| `POST` | `/api/v1/Register/Launcher` | Persist synthetic launcher registration |

## Athena/Centricity Compatibility Surface

| Protocol | Native pattern | Required behavior |
| --- | --- | --- |
| SOAP/WCF over HTTP(S) | `{unityEndpoint}` | Accept Unity SOAP/XML envelope and BasicHttpBinding-compatible operation calls |
| Operation | `GetSecurityToken` | Return synthetic security token |
| Operation | `Magic` action `GetPatient` | Return synthetic patient XML |
| Operation | `Magic` action `GetProviders` | Return provider list XML |
| Operation | `Magic` action `GetClinicalSummary` with `Allergies` | Return allergies XML |
| Operation | `Magic` action `GetClinicalSummary` with `Medications` | Return medications XML |
| Operation | `Magic` action `GetClinicalSummary` with `Problems` | Return problems XML |
| Operation | `Magic` action `SaveDocumentImage` | Persist synthetic document image |
| Operation | `Magic` action `GetDocumentByAccession` | Return document by accession |
| Operation | `Magic` action `GetDocuments` | Return document list |
| Operation | `Magic` action `GetDocumentType` | Return document type metadata |
| Data source simulation | CPS SQL Server contract | Simulate configured CPS patient/search/report data without requiring real SQL Server |
| Data source simulation | CEMR Oracle contract | Simulate configured CEMR patient/search/report data without requiring real Oracle |

## Altera/Allscripts Compatibility Surface

### Unity WCF

| Protocol/action | Native value | Required behavior |
| --- | --- | --- |
| SOAP namespace | `http://www.allscripts.com/Unity` | Preserve namespace in SOAP-compatible responses |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/Magic` | Dispatch documented Magic actions |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/ReturnMagicJSON` | Return JSON payload in documented envelope |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/GetSecurityToken` | Return synthetic token |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/GetValidSecurityToken` | Validate or return active token |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/GetValidSecurityTokenPost` | POST token validation variant |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/GetTokenJsonPost` | JSON token retrieval variant |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/RetireSecurityToken` | Retire synthetic token |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/RetireSecurityTokenPost` | POST token retirement variant |
| SOAP action | `http://www.allscripts.com/Unity/IUnityService/RetireTokenJsonPost` | JSON token retirement variant |

### Midmark Framework ASMX

Native endpoint: `/IQFrameworkWebService/IQConnectIF.asmx`

The simulator must accept SOAP/XML-compatible calls for documented framework methods, including folder configuration, plugin lookup, file download, report audit, notifications, report lists, report retrieval/deletion/existence checks, XBAP preparation, data file block operations, calibration reports, settings operations, common settings, and provider-list retrieval.

### Browser/client routes

| Method | Native path | Required behavior |
| --- | --- | --- |
| `GET` | `/XbapLauncher.aspx` | Return deterministic launch workflow URL/outcome |
| `GET` | `/XbapTest.aspx` | Return deterministic test workflow URL/outcome |
| `GET` | `/XbapReview.aspx` | Return deterministic review workflow URL/outcome |
| `GET` | `/XbapCompare.aspx` | Return deterministic compare workflow URL/outcome |
| `GET` | `/XbapCalibrate.aspx` | Return deterministic calibration workflow URL/outcome |

## Cerner Compatibility Surface

### CareAware/VitalsLink REST

All paths are relative to configured `BASE_URL`.

| Method | Native path | Required behavior |
| --- | --- | --- |
| `POST` | `/security/auth/login` | Validate synthetic credentials and return session behavior |
| `GET` | `/cas/api/v1/barcode/formats?organizationId={organizationId}&formatTypes=...` | Return barcode format response |
| `GET` | `/cas/api/v1/barcode/organizations/{organizationId}/barcodes/{barcode}/personnel` | Resolve personnel barcode |
| `GET` | `/cas/api/v1/locations/getLocations?...` | Return location hierarchy |
| `GET` | `/cas/api/v1/encounters?...` | Search active encounters |
| `GET` | `/cas/api/v1/encounters/{encounterId}` | Return encounter detail |
| `GET` | `/cas/api/v1/patients?_id={patientId}` | Return patient detail |
| `POST` | `/gda/api/devices` | Register device |
| `POST` | `/gda/api/devices/heartbeat` | Record heartbeat and return statuses |
| `POST` | `/cas/api/v1/chartdoc/discrete` | Persist posted vitals/discrete chart documentation |
| `DELETE` | `/gda/api/devices/{deviceId}/{instanceId}` | Remove/unregister device instance |

### HL7 TCP/MLLP

| Boundary | Native pattern | Required behavior |
| --- | --- | --- |
| Inbound ADT listener | `{ADTPublisherIP}:{ADTPublisherPort}` | Accept MLLP-framed ADT messages, persist patient/message state, return ACK/NAK |
| Outbound ORU/result sender | `{HL7Settings:TCPIPHost}:{HL7Settings:IPPort}` | Send or simulate ORU/result messages with configured framing and record verification evidence |

### Midmark-facing Cerner web service

| Method | Native path | Required behavior |
| --- | --- | --- |
| `POST` | `/api/v1/ADTPatients/PatientSearchRequest` | Search synthetic ADT patients |
| `GET` | `/api/v1/ADTPatients/{id}` | Return ADT patient by ID |
| `GET` | `/api/v1/cerner/patients` | Return all current synthetic database patients, including the 15 default seeded records and later synthetic imports |
| `GET` | `/api/v1/cerner/patients/{id}` | Return a typed synthetic Cerner patient response by database ID, external patient ID, or MRN |
| `PUT` | `/api/v1/ADTPatients/UpdateLastAccessTime` | Update synthetic last access timestamp |
| `GET` | `/api/v1/Physicians?activeOnly={activeOnly}` | Return physician list |
| `POST` | `/api/v1/HL7Messages` | Submit HL7 message for outbound processing/simulation |
| `POST` | `/api/v1/HL7Messages/pendingtest/{id}` | Submit HL7 message associated with pending test |

### Patient Seed and Reset Contract

- Startup guarantees exactly 15 default synthetic patient seed records exist in the SQLite database.
- Startup seeding is non-destructive and does not remove later synthetic imports.
- Cerner patient search returns all current synthetic database patients.
- Operator reset removes imported/generated synthetic patients and restores the 15 default seeded patients.
- All Cerner patient, physician, and HL7 submission payloads use typed provider contract DTOs/records.

## Verification Requirements

- Contract tests must exercise at least one success and one protected/failure case per contract family.
- Coverage verification must prove every source-document entry has a catalog row and status.
- Native SOAP/XML and HL7 MLLP boundaries require integration tests or smoke harnesses that call the same protocol shape expected by connectors.
- Swagger/OpenAPI must document admin/control APIs and HTTP compatibility endpoints where applicable. Non-HTTP boundaries must be documented in quickstart and contract docs.
