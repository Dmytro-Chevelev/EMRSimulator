# Coverage Results

Endpoint coverage is exposed through `/api/v1/endpoint-contracts` and the Admin UI Compatibility page.

## Implemented Families

- Epic launch, OAuth, FHIR, reports, PDF, and device workflow surfaces.
- Cerner VitalsLink REST, Midmark-facing Cerner REST, and HL7 MLLP ACK surfaces.
- Athena/Centricity Unity SOAP/XML surfaces.
- Altera/Allscripts Framework ASMX, Unity operation, and browser route surfaces.
- Admin/control catalog, verification evidence, request log, and reset surfaces.

## Evidence

Story-specific smoke tests were added under `tests/EmrSimulator.Tests.Contracts` and `tests/EmrSimulator.Tests.Integration` for Epic, Cerner, Unity, HL7, and Admin API behavior.

## Source Entry Coverage

| Source group | Documented entry | Simulator coverage |
| --- | --- | --- |
| Epic launch | `/Midmark`, `/Midmark/Redirect`, `/Midmark/Close` | Implemented native routes |
| Epic OAuth/FHIR | `/oauth2/token`, `/metadata`, `/FHIR/R4/{resource}`, `{BackendFHIRUrl}/{resource}` | Implemented token, metadata, and wildcard FHIR route |
| Epic PDF | `/Pdf/convert?environmentId={environmentId}&userId={userId}&documentId={documentId}` | Implemented native GET and POST aliases |
| Epic reports | `/api/v1/Reports/patientId/{patientId}` | Implemented native route |
| Epic reports | `/api/v1/Reports/ReportType/{reportType}` | Implemented native route |
| Epic reports | `/api/v1/Reports/deviceId/{deviceId}` | Implemented native route |
| Epic reports | `/api/v1/Reports/reportId/{reportId}` | Implemented native route |
| Epic reports | `/api/v1/Reports/SaveReport` | Implemented native route |
| Epic reports | `/api/v1/Reports/GetDataFile` | Implemented synthetic data-file response |
| Epic reports | `/api/v1/Reports/ReviewReport/{reportId}` | Implemented synthetic review response |
| Epic reports | `/api/v1/Reports/CompareReports` | Implemented synthetic comparison response |
| Epic reports | `/api/v1/Reports/Convert/{reportType}/{reportId}` | Implemented synthetic conversion response |
| Epic devices/auth/register | `/api/v1/Devices/StartTest`, `/api/v1/Devices/Abort`, `/api/v1/Authenticate/Auth`, `/api/v1/Register/Launcher` | Implemented native aliases |
| Cerner VitalsLink | `/security/auth/login` | Implemented native route |
| Cerner VitalsLink | `/cas/api/v1/barcode/formats` | Implemented native route |
| Cerner VitalsLink | `/cas/api/v1/barcode/organizations/{organizationId}/barcodes/{barcode}/personnel` | Implemented native route |
| Cerner VitalsLink | `/cas/api/v1/locations/getLocations` | Implemented native route |
| Cerner VitalsLink | `/cas/api/v1/encounters`, `/cas/api/v1/encounters/{encounterId}` | Implemented native routes |
| Cerner VitalsLink | `/cas/api/v1/patients?_id={patientId}` | Implemented native route |
| Cerner VitalsLink | `/gda/api/devices`, `/gda/api/devices/heartbeat`, `/gda/api/devices/{deviceId}/{instanceId}` | Implemented native routes |
| Cerner VitalsLink | `/cas/api/v1/chartdoc/discrete` | Implemented native route |
| Cerner HL7 | Inbound ADT MLLP listener | Implemented MLLP ACK service and hosted listener |
| Cerner HL7 | Outbound ORU/result sender | Simulated ACK/generation boundary with evidence hooks |
| Cerner Midmark service | `/api/v1/ADTPatients/PatientSearchRequest`, `/api/v1/ADTPatients/{id}`, `/api/v1/ADTPatients/UpdateLastAccessTime` | Implemented native routes |
| Cerner Midmark service | `/api/v1/Physicians`, `/api/v1/HL7Messages`, `/api/v1/HL7Messages/pendingtest/{id}` | Implemented native routes |
| Athena Unity | Unity SOAP endpoint and `GetSecurityToken`, `Magic` actions | Implemented SOAP envelope dispatcher and token/Magic operation routes |
| Athena data sources | CPS SQL Server and CEMR Oracle contracts | Simulated through synthetic data-source/catalog family without external database dependency |
| Altera Unity | All documented token, Magic, ReturnMagicJSON, validation, and retire-token actions | Implemented SOAP dispatcher family plus direct token/Magic operation routes |
| Altera Framework ASMX | `/IQFrameworkWebService/IQConnectIF.asmx` documented framework methods | Implemented ASMX SOAP dispatcher for framework operations |
| Altera browser routes | `/XbapLauncher.aspx`, `/XbapTest.aspx`, `/XbapReview.aspx`, `/XbapCompare.aspx`, `/XbapCalibrate.aspx` | Implemented exact native routes |
| Admin/control | `/api/v1/endpoint-contracts`, `/api/v1/endpoint-contracts/{id}/verification`, `/api/v1/simulator/reset` | Implemented versioned admin/control APIs |