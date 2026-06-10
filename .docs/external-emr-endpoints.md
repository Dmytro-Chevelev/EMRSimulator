# External EMR Endpoint Inventory

Scanned: 2026-06-10

## Scan scope

Local repositories and source trees inspected:

- `C:\Projects\Midmark\src\Midmark.Connectors.Epic`
- `C:\Projects\Midmark\src\Midmark.Connectors.EpicPdf`
- `C:\Projects\Midmark\src\Midmark.Plugins.EpicVitals`
- `C:\Projects\Midmark\src\Midmark.Connectors.Athena`
- `C:\Projects\Midmark\src\Midmark.Connectors.Altera`
- `C:\Projects\Midmark\src\Midmark.Connectors.Cerner`
- `C:\Projects\Midmark\src\Midmark.Connectors.Core`

No product repositories were found under `C:\Users\DChevelevAD\Documents\Codex\2026-06-10\scan-all-available-repos-and-list` or `C:\Users\DChevelevAD\source\repos`.

For this inventory, "endpoint" includes EMR-originated launch/callback URLs, outbound provider REST/FHIR URLs, WCF/SOAP provider operations, HL7 TCP endpoints, and provider database endpoints. Midmark-internal APIs that support an EMR connector are listed in separate "Midmark-facing/internal" sections so they are not confused with provider endpoints.

## EPIC

### External Epic launch and OAuth/FHIR endpoints

| Method | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| GET | `/Midmark?launch={launchToken}&iss={fhirBaseUrl}` | Epic to Midmark | SMART-on-FHIR launch into Epic PDF middleware. The `iss` value becomes the Epic FHIR base URL. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\MidmarkController.cs` |
| GET | `/Midmark/Redirect?code={authorizationCode}&state={sessionId}` | Epic OAuth server to Midmark | OAuth authorization-code callback. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\MidmarkController.cs` |
| GET | `/Midmark/Close` | Epic shell/browser to Midmark | Close or abort launched workflow. Uses `uniqueMDLId` header when present. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\MidmarkController.cs` |
| GET | `{iss}/metadata` | Midmark to Epic | FHIR capability statement and OAuth endpoint discovery. Sends `Epic-Client-ID` header. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Helpers\OAuth2.cs` |
| POST | `{AuthorizationURI}` | Midmark to Epic | OAuth authorization request discovered from Epic metadata. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Helpers\OAuth2.cs` |
| POST | `{TokenURI}` | Midmark to Epic | OAuth token exchange using authorization code. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Helpers\OAuth2.cs` |
| POST | `{TokenURI}` | Midmark to Epic | Backend OAuth token request using `client_credentials` and JWT client assertion. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Helpers\OAuth2.cs` |
| POST | `{BackendAuthUrl}` | Midmark to Epic | Backend token flow target from configuration. Used as the audience/authorization server for backend JWT token acquisition. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Helpers\OAuth2.cs` |
| GET | `{fhirBaseUrl}/{resource}` | Midmark to Epic | General FHIR GET wrapper for patient, provider, observation, diagnostic report, report/PDF, and related resource reads. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\FhirHttpCaller.cs` |
| GET | `{BackendFHIRUrl}/{resource}` | Midmark to Epic | Backend FHIR resource retrieval for PDF/report conversion flow. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\PdfController.cs` |

### Epic PDF middleware endpoints exposed by Midmark

| Method | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| GET | `/Pdf/convert?environmentId={environmentId}&userId={userId}&documentId={documentId}` | Epic or Epic-adjacent report workflow to Midmark | Converts/retrieves a report PDF through Epic backend FHIR. Uses Basic auth inbound, then Epic backend OAuth/FHIR outbound. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\PdfController.cs` |
| GET | `/api/v1/Reports/patientId/{patientId}` | Midmark middleware/client to Midmark web API | Get reports for patient. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\ReportsController.cs`; `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\ReportsController.cs` |
| GET | `/api/v1/Reports/ReportType/{reportType}` | Midmark middleware/client to Midmark web API | Get reports by report type. `patientId` is supplied in a header. | Same as above |
| GET | `/api/v1/Reports/deviceId/{deviceId}` | Midmark middleware/client to Midmark web API | Get reports by device. `patientId` is supplied in a header. | Same as above |
| GET | `/api/v1/Reports/reportId/{reportId}` | Midmark middleware/client to Midmark web API | Get a report by ID. Middleware version accepts EMR context header and can fetch report content through Epic FHIR. | Same as above |
| POST | `/api/v1/Reports/SaveReport` | Midmark middleware/client to Midmark web API | Save report. | Same as above |
| GET | `/api/v1/Reports/GetDataFile` | Midmark middleware/client to Midmark web API | Retrieve report data file. Middleware version accepts EMR context header. | `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Controllers\ReportsController.cs` |
| POST | `/api/v1/Reports/ReviewReport/{reportId}` | Midmark middleware/client to Midmark web API | Open report review workflow. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\ReportsController.cs` |
| POST | `/api/v1/Reports/CompareReports` | Midmark middleware/client to Midmark web API | Open report comparison workflow. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\ReportsController.cs` |
| POST | `/api/v1/Reports/Convert/{reportType}/{reportId}` | Midmark middleware/client to Midmark web API | Convert report. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\ReportsController.cs` |
| POST | `/api/v1/Devices/StartTest` | Midmark middleware/client to Midmark web API | Start device test. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\DevicesController.cs` |
| POST | `/api/v1/Devices/Abort` | Midmark middleware/client to Midmark web API | Abort active device workflow. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\DevicesController.cs` |
| POST | `/api/v1/Authenticate/Auth` | Midmark client to Midmark web API | Authenticate into IQconnect web API. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\AuthenticateController.cs` |
| POST | `/api/v1/Register/Launcher` | Midmark client to Midmark web API | Register launcher. | `Midmark.Connectors.EpicPdf\src\IQconnectWeb\Controllers\RegisterController.cs` |

## Athena

Athena/Centricity does not expose fixed REST or FHIR routes in the scanned connector. The external integration boundary is a configured GE/Athena Centricity Unity WCF endpoint, plus direct database connectivity for CPS/CEMR deployments.

### External Athena/Centricity Unity endpoint

| Method/protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| SOAP/WCF over HTTP(S) | `{unityEndpoint}` | Midmark to Athena/Centricity Unity | Configured Unity service endpoint. Code comments identify the expected shape as `http://[server]/Unity/UnityService.svc`. | `Midmark.Connectors.Athena\src\MidmarkAppsDataLayer\TWContextInfo.cs`; `Midmark.Connectors.Athena\src\MidmarkAppsServiceLayer\UnityClient.cs` |

### Athena/Centricity Unity operations

| Operation | External action or parameter | Purpose | Source |
| --- | --- | --- | --- |
| `GetSecurityToken` | WCF operation | Authenticate service user. | `Midmark.Connectors.Athena\src\MidmarkAppsServiceLayer\UnityClient.cs` |
| `Magic` | `GetPatient` | Retrieve patient context/details. | Same as above |
| `Magic` | `GetProviders` | Retrieve provider list. | Same as above |
| `Magic` | `GetClinicalSummary` with `Allergies` | Retrieve allergies. | Same as above |
| `Magic` | `GetClinicalSummary` with `Medications` | Retrieve medications. | Same as above |
| `Magic` | `GetClinicalSummary` with `Problems` | Retrieve problems. | Same as above |
| `Magic` | `SaveDocumentImage` | Upload or update report/document image. | Same as above |
| `Magic` | `GetDocumentByAccession` | Retrieve document by accession. | Same as above |
| `Magic` | `GetDocuments` | Retrieve document list. | Same as above |
| `Magic` | `GetDocumentType` | Retrieve document type metadata. | Same as above |

### Athena/Centricity data-source endpoints

| Protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| SQL Server | CPS SQL Server connection string/server/database | Midmark to Athena/Centricity CPS database | Patient/search/report data access for CPS deployments. | `Midmark.Connectors.Athena\README.md`; `Midmark.Connectors.Athena\src\MidmarkAppsModels\PatientSearch.cs` |
| Oracle | CEMR Oracle host/service/port | Midmark to Athena/Centricity CEMR database | Patient/search/report data access for CEMR deployments. | `Midmark.Connectors.Athena\README.md`; `Midmark.Connectors.Athena\src\MidmarkAppsServiceLayerIQiCOracle`; `Midmark.Connectors.Athena\src\MidmarkApps\IQinterface\ViewModels\SettingsPageViewModel.cs` |

## Altera

Altera, formerly Allscripts, uses the Altera/Allscripts Unity WCF API. The scanned connector also contains Midmark framework SOAP and web UI routes, but those are Midmark-facing surfaces rather than external provider APIs.

### External Altera Unity endpoint

| Method/protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| SOAP/WCF over HTTP(S) | `{unityEndpoint}` | Midmark to Altera Unity | Configured Altera Unity service endpoint using `BasicHttpBinding("basichttp")`. | `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAppsServiceLayerIQiA\UnityClient.cs` |
| SOAP namespace | `http://www.allscripts.com/Unity` | Midmark to Altera Unity | Unity SOAP service namespace. | `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAppsServiceLayerIQiA\UnityService.cs` |

### Altera Unity SOAP actions and operations

| Operation | SOAP action | Purpose | Source |
| --- | --- | --- | --- |
| `Magic` | `http://www.allscripts.com/Unity/IUnityService/Magic` | Primary Unity action dispatcher for clinical and context operations. | `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAppsServiceLayerIQiA\UnityService.cs` |
| `ReturnMagicJSON` | `http://www.allscripts.com/Unity/IUnityService/ReturnMagicJSON` | JSON-returning Unity action dispatcher. | Same as above |
| `GetSecurityToken` | `http://www.allscripts.com/Unity/IUnityService/GetSecurityToken` | Authenticate service user. | Same as above |
| `GetValidSecurityToken` | `http://www.allscripts.com/Unity/IUnityService/GetValidSecurityToken` | Validate or retrieve active token. | Same as above |
| `GetValidSecurityTokenPost` | `http://www.allscripts.com/Unity/IUnityService/GetValidSecurityTokenPost` | POST variant for token validation/retrieval. | Same as above |
| `GetTokenJsonPost` | `http://www.allscripts.com/Unity/IUnityService/GetTokenJsonPost` | JSON token retrieval via POST. | Same as above |
| `RetireSecurityToken` | `http://www.allscripts.com/Unity/IUnityService/RetireSecurityToken` | Retire authentication token. | Same as above |
| `RetireSecurityTokenPost` | `http://www.allscripts.com/Unity/IUnityService/RetireSecurityTokenPost` | POST variant for retiring token. | Same as above |
| `RetireTokenJsonPost` | `http://www.allscripts.com/Unity/IUnityService/RetireTokenJsonPost` | JSON POST variant for retiring token. | Same as above |

Observed Unity `Magic` action names in the Altera client include `GetTokenValidation`, `GetUserID`, `GetServerInfo`, and patient/context retrieval flows.

### Altera data-source endpoints

| Protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| SQL Server | Altera/IQiA database connection string/server/database | Midmark to local Altera/IQiA data store | Connector data storage and Allscripts interface data access. | `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkIQiADataStorage\DbConnectionSettings.cs`; `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAllscriptsIfDataAccessLayer` |

### Midmark-facing/internal Altera endpoints

| Method/protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| SOAP/ASMX | `/IQFrameworkWebService/IQConnectIF.asmx` | Midmark components to Midmark framework service | IQconnect framework SOAP service. WSDL sample address is `http://localhost/IQFrameworkWebService/IQConnectIF.asmx`. | `Midmark.Connectors.Altera\src\MidmarkFramework\MidmarkFrameworkWebService\IQConnectIF.asmx`; `Midmark.Connectors.Altera\src\MidmarkFramework\Web References\FrameworkService\IQConnectIF.wsdl` |
| GET | `/XbapLauncher.aspx` | Browser/client to Midmark web UI | Launch XBAP workflow. | `Midmark.Connectors.Altera\src\MidmarkApps\IQinterfaceWeb` |
| GET | `/XbapTest.aspx` | Browser/client to Midmark web UI | Test workflow. | Same as above |
| GET | `/XbapReview.aspx` | Browser/client to Midmark web UI | Review workflow. | Same as above |
| GET | `/XbapCompare.aspx` | Browser/client to Midmark web UI | Compare workflow. | Same as above |
| GET | `/XbapCalibrate.aspx` | Browser/client to Midmark web UI | Calibration workflow. | Same as above |

## Cerner

Cerner integration has two provider-facing surfaces in the scanned code: CareAware/VitalsLink REST APIs and HL7 over TCP/MLLP. The connector also exposes Midmark web-service routes that support patient/report/device workflows.

### External Cerner CareAware/VitalsLink REST endpoints

All REST paths below are relative to configured `BASE_URL`.

| Method | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| POST | `{BASE_URL}/security/auth/login` | Midmark to Cerner/VitalsLink | Authenticate user and capture `JSESSIONID`. | `Midmark.Connectors.Cerner\src\MidmarkIQiM\VitalsLinkAPILib\APIVitalsLink.cs` |
| GET | `{BASE_URL}/cas/api/v1/barcode/formats?organizationId={organizationId}&formatTypes=PRSNL_USERNAME&formatTypes=PRSNL_CONTEXT_ID` | Midmark to Cerner/VitalsLink | Retrieve supported personnel barcode formats. | Same as above |
| GET | `{BASE_URL}/cas/api/v1/barcode/organizations/{organizationId}/barcodes/{barcode}/personnel` | Midmark to Cerner/VitalsLink | Resolve personnel barcode. | Same as above |
| GET | `{BASE_URL}/cas/api/v1/locations/getLocations?_ids={startingLocationId}&activeOnly=true&includeChildren={includeChildren}` | Midmark to Cerner/VitalsLink | Retrieve location hierarchy. | Same as above |
| GET | `{BASE_URL}/cas/api/v1/encounters?statusTypes=ACTIVE&visitTypes={visitType}&matchAssignedLocation={matchAssignedLocation}&locationIds={locationId}` | Midmark to Cerner/VitalsLink | Search active encounters. | Same as above |
| GET | `{BASE_URL}/cas/api/v1/encounters/{encounterId}` | Midmark to Cerner/VitalsLink | Retrieve a specific encounter. | Same as above |
| GET | `{BASE_URL}/cas/api/v1/patients?_id={patientId}` | Midmark to Cerner/VitalsLink | Retrieve patient details. | Same as above |
| POST | `{BASE_URL}/gda/api/devices` | Midmark to Cerner/VitalsLink | Register device. | Same as above |
| POST | `{BASE_URL}/gda/api/devices/heartbeat` | Midmark to Cerner/VitalsLink | Send device heartbeat. | Same as above |
| POST | `{BASE_URL}/cas/api/v1/chartdoc/discrete` | Midmark to Cerner/VitalsLink | Post discrete chart documentation/vitals. | Same as above |
| DELETE | `{BASE_URL}/gda/api/devices/{deviceId}/{instanceId}` | Midmark to Cerner/VitalsLink | Remove/unregister device instance. | Same as above |

Authentication/header variants observed in the VitalsLink client:

- Basic auth through `Authorization: Basic {base64(username:password)}`.
- Bearer auth through `Authorization: Bearer {token}`.
- Tenant context through `Tenant-Id` or `Tenant-Short-Name`.

Source: `Midmark.Connectors.Cerner\src\MidmarkIQiM\VitalsLinkAPILib\ClientHelper.cs`.

### External Cerner HL7 endpoints

| Protocol | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| HL7 over TCP/MLLP | `{ADTPublisherIP}:{ADTPublisherPort}`; default port observed: `22222` | Cerner/interface engine to Midmark | Inbound ADT listener receives patient ADT messages and sends ACKs. | `Midmark.Connectors.Cerner\src\ADTListener\App.config`; `Midmark.Connectors.Cerner\src\ADTListener\ADTListenerService.cs`; `Midmark.Connectors.Cerner\src\ADTListener\ListenerThd.cs` |
| HL7 over TCP/MLLP | `{AppSettings:Organization:HL7Settings:TCPIPHost}:{AppSettings:Organization:HL7Settings:IPPort}` | Midmark to Cerner/interface engine | Outbound ORU/result message sender. Uses configured HL7 header/trailer framing. | `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebService\Controllers\HL7MessagesController.cs`; `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebService\SocketSupport\HL7SocketClient.cs` |

### Midmark-facing/internal Cerner web-service endpoints

| Method | Endpoint or pattern | Direction | Purpose | Source |
| --- | --- | --- | --- | --- |
| POST | `/api/v1/ADTPatients/PatientSearchRequest` | Midmark client to Midmark Cerner web service | Search ADT patients. The route token is declared as `{PatientSearchRequest}` and the client calls `PatientSearchRequest`. | `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebService\Controllers\ADTPatientsController.cs`; `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebAPILib` |
| GET | `/api/v1/ADTPatients/{id}` | Midmark client to Midmark Cerner web service | Retrieve ADT patient by ID. | Same as above |
| PUT | `/api/v1/ADTPatients/UpdateLastAccessTime` | Midmark client to Midmark Cerner web service | Update patient access timestamp. | Same as above |
| GET | `/api/v1/Physicians?activeOnly={activeOnly}` | Midmark client to Midmark Cerner web service | Retrieve physician list. | `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebService\Controllers\PhysiciansController.cs` |
| POST | `/api/v1/HL7Messages` | Midmark client to Midmark Cerner web service | Submit HL7 message for outbound processing. | `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebService\Controllers\HL7MessagesController.cs` |
| POST | `/api/v1/HL7Messages/pendingtest/{id}` | Midmark client to Midmark Cerner web service | Submit HL7 message associated with a pending test. | Same as above |

## Notes for SDD

- EPIC is the only scanned provider with SMART-on-FHIR launch, OAuth discovery, OAuth token exchange, and FHIR base URL usage in the connector code.
- Athena and Altera do not expose fixed provider REST/FHIR endpoints in the scanned repositories. Their external provider boundary is the configured Unity WCF endpoint plus database connectivity where applicable.
- Cerner has both REST and HL7 integration surfaces. REST paths are relative to configured `BASE_URL`; HL7 TCP endpoints are deployment-configured.
- Several connector repositories expose Midmark-internal REST, SOAP, or ASP.NET page routes. These are listed only when they directly support an EMR connector flow and should be treated separately from provider-owned endpoints in architecture diagrams.
