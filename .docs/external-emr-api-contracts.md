# External EMR API Contract Inventory

Scanned: 2026-06-10

## Scope

This file is a companion to `external-emr-endpoints.md`. It lists the request and response contracts used by the scanned API surfaces, represented as JSON-serialized shapes.

Notes:

- JSON keys use the public model property names found in code unless the model itself uses lower camel-case names.
- Enum values are shown as strings for readability, but the default .NET serializers may emit numeric enum values unless configured otherwise.
- `byte[]` payloads are represented as base64 strings.
- Athena and Altera provider integrations are WCF/SOAP/XML rather than JSON REST APIs. Their WCF/SOAP contracts are represented here as JSON envelopes for SDD documentation.
- Simulator provider-facing request and response payloads are implemented as typed DTOs/records in `EmrSimulator.Contracts` so contract tests can detect shape drift.
- Simulator persistence uses repo-local `.data/emrsimulator.db` by default, seeds 15 Cerner/Midmark patient records non-destructively at startup, and reset removes imported/generated patients before restoring that default baseline.

Source roots:

- `C:\Projects\Midmark\src\Midmark.Connectors.EpicPdf`
- `C:\Projects\Midmark\src\Midmark.Connectors.Epic`
- `C:\Projects\Midmark\src\Midmark.Connectors.Athena`
- `C:\Projects\Midmark\src\Midmark.Connectors.Altera`
- `C:\Projects\Midmark\src\Midmark.Connectors.Cerner`

## EPIC

### SMART/OAuth Token Response

Source: `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Models\TokensResponse.cs`

```json
{
  "TokensResponse": {
    "Access_Token": "string",
    "Token_Type": "Bearer",
    "Expires_In": 3600,
    "Scope": "string",
    "Patient": "string",
    "Encounter": "string",
    "Location": "string",
    "State": "string",
    "MidmarkRole": 1,
    "ProviderId": "string",
    "FhirDiagnosticReport": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": "diagnosticReportId|2026-06-10T12:00:00Z,diagnosticReportId2|2026-06-10T12:05:00Z",
    "Workstation": "string",
    "MidmarkOperation": 1,
    "MidmarkType": 1,
    "ReadOnly": 0,
    "UserID": "string"
  }
}
```

### Middleware Request Model

Source: `Midmark.Connectors.EpicPdf\src\EpicMiddleware\Models\RequestModel.cs`

```json
{
  "RequestModel": {
    "AccessToken": "string",
    "ExpiresIn": 3600,
    "MidmarkRole": "Technician",
    "PatientInfo": {
      "PatientId": "string",
      "PatientDisplayId": "string",
      "LastName": "string",
      "FirstName": "string",
      "MiddleName": "string",
      "DOB": "1970-01-01T00:00:00",
      "PatientDOB": "01/01/1970",
      "Age": 56,
      "PatientSex": "Male",
      "PatientRaceCode": "Unspecified",
      "SmokingStartYear": 2001,
      "SmokingQuitYear": 2020,
      "SmokingQuantity": 1,
      "WeightInKg": 80.0,
      "WeightInLb": 176.37,
      "HeightInCm": 180.0,
      "HeightInInches": 70.87,
      "TempInDegC": 36.8,
      "TempInDegF": 98.24,
      "SystolicBP": 120,
      "DiastolicBP": 80,
      "Pulse": 72,
      "SpO2": 98,
      "BMI": 24.69,
      "RespRate": 16,
      "PainScore": 0,
      "MedicalHistory": "string",
      "Pacemaker": "string",
      "Medications": [],
      "RiskFactors": [
        "string"
      ],
      "LinkedPatientIds": [
        "string"
      ],
      "PatientNotes": "string"
    },
    "MidmarkType": "ECG",
    "ReadOnly": false,
    "ReportId": "string",
    "MidmarkOperation": "Acquire",
    "FhirServiceRequest": "string",
    "BaseURL": "https://epic-fhir-base-url",
    "Save": true,
    "FhirDrList": [
      {
        "ReportId": "string",
        "PatientId": "string",
        "DeviceId": "00000000-0000-0000-0000-000000000000",
        "ReportType": "ECG",
        "CreateDate": "2026-06-10T12:00:00",
        "ReferredBy": "string",
        "RequestedBy": "string",
        "SupervisedBy": "string",
        "Technician": "string",
        "ReviewedBy": "string",
        "ReviewDate": null,
        "Archived": false,
        "UploadDate": null,
        "SignOffDate": null,
        "SignOffBy": "string",
        "Version": "string",
        "IsSigned": false,
        "IsReviewed": false,
        "IsSignedOrReviewed": false,
        "IsReviewedButNotSigned": false,
        "Synopsis": "string",
        "DiscreteData": "<DiscreteData />"
      }
    ],
    "ProviderID": "string",
    "UserID": "string",
    "PractitionerName": "string",
    "UniqueMDLId": "string"
  }
}
```

### Midmark Operation Requests

Sources:

- `Midmark.Connectors.EpicPdf\src\MCFWebModels\Requests\MidmarkOpRequest.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\Requests\StartTestRequest.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\Requests\ReviewReportRequest.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\Requests\CompareReportsRequest.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\Requests\AbortRequest.cs`

```json
{
  "MidmarkOpRequest": {
    "PatientInfo": "PatientInfo",
    "ReportInfo": {
      "ReferredBy": "string",
      "RequestedBy": "string",
      "ReviewedBy": "string",
      "SupervisedBy": "string",
      "Technician": "string",
      "Indication": "string",
      "UserID": "string",
      "NoSignOff": false
    },
    "MidmarkRole": "Technician",
    "ReportId": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": [
      "DeviceReportInfo"
    ],
    "BaseURL": "https://epic-fhir-base-url",
    "AccessToken": "string",
    "ReportType": "ECG",
    "PractitionerName": "string",
    "UserID": "string",
    "IsLocalFileRequest": false
  },
  "StartTestRequest": {
    "DeviceId": "00000000-0000-0000-0000-000000000000",
    "PatientInfo": "PatientInfo",
    "ReportInfo": "ReportInfo",
    "MidmarkRole": "Technician",
    "ReportId": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": [
      "DeviceReportInfo"
    ],
    "BaseURL": "https://epic-fhir-base-url",
    "AccessToken": "string",
    "ReportType": "ECG",
    "PractitionerName": "string",
    "UserID": "string",
    "IsLocalFileRequest": false
  },
  "ReviewReportRequest": {
    "ReadOnly": true,
    "IsReanalyze": false,
    "PatientInfo": "PatientInfo",
    "ReportInfo": "ReportInfo",
    "MidmarkRole": "Physician",
    "ReportId": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": [
      "DeviceReportInfo"
    ],
    "BaseURL": "https://epic-fhir-base-url",
    "AccessToken": "string",
    "ReportType": "ECG",
    "PractitionerName": "string",
    "UserID": "string",
    "IsLocalFileRequest": false
  },
  "CompareReportsRequest": {
    "ReadOnly": true,
    "PatientInfo": "PatientInfo",
    "ReportInfo": "ReportInfo",
    "MidmarkRole": "Physician",
    "ReportId": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": [
      "DeviceReportInfo"
    ],
    "BaseURL": "https://epic-fhir-base-url",
    "AccessToken": "string",
    "ReportType": "ECG",
    "PractitionerName": "string",
    "UserID": "string",
    "IsLocalFileRequest": false
  },
  "AbortRequest": {
    "DoSave": true,
    "PatientInfo": "PatientInfo",
    "ReportInfo": "ReportInfo",
    "MidmarkRole": "Technician",
    "ReportId": "string",
    "FhirServiceRequest": "string",
    "FhirDRList": [
      "DeviceReportInfo"
    ],
    "BaseURL": "https://epic-fhir-base-url",
    "AccessToken": "string",
    "ReportType": "ECG",
    "PractitionerName": "string",
    "UserID": "string",
    "IsLocalFileRequest": false
  }
}
```

### Report Save and Retrieval Contracts

Sources:

- `Midmark.Connectors.EpicPdf\src\MCFWebModels\FhirModels\EmrDTO\RequiredEMRInfo.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\FhirModels\EmrDTO\EMRInfo.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\CustomToMidmark\DeviceReport.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\CustomToMidmark\DeviceReportInfo.cs`

```json
{
  "RequiredEMRInfo": {
    "DeviceReport": {
      "ReportId": "string",
      "ReportFormat": "NATIVE",
      "PatientId": "string",
      "DeviceId": "00000000-0000-0000-0000-000000000000",
      "ReportType": "ECG",
      "CreateDate": "2026-06-10T12:00:00",
      "Saved": true,
      "ReviewDate": null,
      "Archived": false,
      "UploadDate": null,
      "SignOffDate": null,
      "Revision": "0",
      "ReportInfo": "ReportInfo",
      "PatientInfo": "PatientInfo",
      "ReportData": "base64",
      "ReportRawData": "base64",
      "DiscreteData": "<DiscreteData />",
      "FhirStatus": "string",
      "ReferredBy": "string",
      "RequestedBy": "string",
      "SupervisedBy": "string",
      "Technician": "string",
      "UserID": "string",
      "ReviewedBy": "string",
      "Synopsis": "string"
    },
    "EMRInfo": {
      "FhirServiceRequest": "string",
      "ReportId": "string",
      "BaseURL": "https://epic-fhir-base-url",
      "AccessToken": "string",
      "ETag": "string",
      "FhirDRList": [
        "DeviceReportInfo"
      ],
      "ReportType": "ECG"
    }
  }
}
```

### Device Registration and Device Metadata Contracts

Sources:

- `Midmark.Connectors.EpicPdf\src\MCFWebModels\CustomToMidmark\LauncherRegistration.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\CustomToMidmark\DeviceInfo.cs`
- `Midmark.Connectors.EpicPdf\src\MCFWebModels\CustomToMidmark\DeviceCategory.cs`

```json
{
  "MDLRegistration": {
    "MDLId": "string",
    "MDLSessionGuid": "string",
    "ClientName": "string",
    "UniqueMDLId": "string"
  },
  "DeviceInfo": {
    "DeviceId": "00000000-0000-0000-0000-000000000000",
    "DeviceName": "string",
    "Base64DeviceIcon": "base64",
    "DeviceDescription": "string",
    "Manufacturer": "string",
    "DeviceCategory": {
      "DeviceCategoryString": "string",
      "DeviceCategoryId": "00000000-0000-0000-0000-000000000000"
    },
    "VersionNumber": "string",
    "AssemblyName": "string",
    "ClassName": "string",
    "HasConnection": true,
    "DefaultSettings": {
      "key": "value"
    },
    "SupportsTest": true,
    "SupportsStat": true,
    "SupportsTestWPFControl": true,
    "SupportsTestWinFormsControl": true,
    "SupportsTestWebControl": false,
    "SupportsCalibrate": false,
    "SupportsCalibrateWPFControl": false,
    "SupportsCalibrateWinFormsControl": false,
    "SupportsCalibrateWebControl": false,
    "SupportsSettingsEdit": true,
    "SupportsSettingsEditWPFControl": true,
    "SupportsSettingsEditWinFormsControl": true,
    "SupportsSettingsEditWebControl": false,
    "SupportsCompare": true,
    "PluginFolder": "string",
    "NativeWebServerName": "string",
    "NativeWebSiteName": "string",
    "NativeWebPageReportReview": "string",
    "NativeWebPageStartTest": "string",
    "NativeWebPageStartCalibration": "string",
    "NativeWebPageStartSettings": "string"
  }
}
```

### Epic FHIR Resource Shapes Used by the Middleware

Sources: `Midmark.Connectors.EpicPdf\src\MCFWebModels\FhirModels`

```json
{
  "EMRPatientInfo": {
    "ResourceType": "Patient",
    "Id": "string",
    "Extension": [],
    "Identifier": [
      {
        "Extension": [],
        "Use": "usual",
        "System": "string",
        "Value": "string",
        "Type": {
          "Text": "string"
        }
      }
    ],
    "Active": true,
    "Name": [
      {
        "Use": "official",
        "Text": "string",
        "Family": "string",
        "Given": [
          "string"
        ],
        "Suffix": [
          "string"
        ]
      }
    ],
    "Telecom": [],
    "Gender": "male",
    "BirthDate": "1970-01-01",
    "DeceasedBoolean": false,
    "Address": [
      {
        "Use": "home",
        "Line": [
          "string"
        ],
        "City": "string",
        "District": "string",
        "State": "string",
        "PostalCode": "string",
        "Country": "string",
        "Period": {
          "Start": "string",
          "End": "string"
        }
      }
    ],
    "MaritalStatus": {
      "Text": "string"
    },
    "Communication": [],
    "GeneralPractitioner": [],
    "ManagingOrganization": {
      "Reference": "string",
      "Display": "string",
      "System": "string",
      "Code": "string"
    }
  },
  "ObservationSearch": {
    "Entry": [
      {
        "Resource": {
          "Code": {
            "Coding": [
              {
                "System": "string",
                "Code": "string",
                "Display": "string"
              }
            ],
            "Text": "string"
          },
          "Component": [
            {
              "Code": "ResourceCode",
              "ValueQuantity": {
                "Value": "string",
                "Unit": "string",
                "System": "string",
                "Code": "string"
              }
            }
          ],
          "ValueQuantity": "ValueQuantity",
          "EffectivePeriod": {
            "Start": "string",
            "End": "string"
          }
        }
      }
    ]
  },
  "DiagnosticReport": {
    "Status": "final",
    "Id": "string",
    "Code": {
      "System": "string",
      "Code": "string",
      "Display": "string"
    },
    "BasedOn": {
      "Reference": "string",
      "Type": "string"
    },
    "Subject": {
      "Reference": "Patient/string",
      "Display": "string"
    },
    "Performer": {
      "Display": "string"
    },
    "Results": [
      {
        "Reference": "Observation/string"
      }
    ],
    "Conclusion": "string",
    "Is_RR_ECG": false
  },
  "Observation": {
    "Status": "final",
    "Category": {
      "Coding": {
        "System": "string",
        "Code": "string",
        "Display": "string"
      },
      "Text": "string"
    },
    "Code": {
      "Coding": {
        "System": "string",
        "Code": "string",
        "Display": "string"
      },
      "Text": "string"
    },
    "Subject": {
      "Reference": "Patient/string",
      "Display": "string"
    },
    "BasedOn": {
      "Reference": "ServiceRequest/string",
      "Type": "ServiceRequest"
    },
    "ValueQuantity": {
      "Value": "string",
      "Unit": "string",
      "System": "string",
      "Code": "string"
    },
    "ValueString": {
      "Value": "string"
    },
    "SourceDeviceType": "string"
  },
  "ReadDrDTO": {
    "ResourceType": "DiagnosticReport",
    "Id": "string",
    "BasedOn": [
      {
        "Reference": "ServiceRequest/string",
        "Type": "ServiceRequest"
      }
    ],
    "Status": "final",
    "Code": {
      "Coding": [
        {
          "System": "string",
          "Code": "string",
          "Display": "string"
        }
      ]
    },
    "Subject": {
      "Reference": "Patient/string",
      "Display": "string"
    },
    "PresentedForm": [
      {
        "ContentType": "application/pdf",
        "Url": "Binary/string",
        "Extension": []
      }
    ]
  },
  "BinaryDTO": {
    "ContentType": "application/pdf",
    "Data": "base64"
  }
}
```

## Athena

Athena/Centricity uses Unity WCF calls and database access rather than JSON REST contracts in the scanned connector. The shapes below represent the service calls as JSON envelopes for SDD documentation.

Sources:

- `Midmark.Connectors.Athena\src\MidmarkAppsDataLayer\TWContextInfo.cs`
- `Midmark.Connectors.Athena\src\MidmarkAppsServiceLayer\UnityClient.cs`

```json
{
  "UnityEndpoint": {
    "Endpoint": "http://server/Unity/UnityService.svc",
    "Binding": "BasicHttpBinding",
    "SecurityMode": "Transport when endpoint starts with https, otherwise None"
  },
  "GetSecurityToken": {
    "Request": {
      "applicationUserName": "string",
      "applicationPassword": "string"
    },
    "Response": {
      "securityToken": "string"
    }
  },
  "Magic": {
    "Request": {
      "action": "GetPatient | GetProviders | GetClinicalSummary | SaveDocumentImage | GetDocumentByAccession | GetDocuments | GetDocumentType",
      "userName": "string",
      "applicationName": "string",
      "patientId": "string",
      "token": "string",
      "parameter1": "string",
      "parameter2": "string",
      "parameter3": "string",
      "parameter4": "string",
      "parameter5": "string",
      "parameter6": "string",
      "xmlInput": "string or null"
    },
    "Response": {
      "xml": "<UnityResponse />"
    }
  },
  "CPSDatabaseContract": {
    "Protocol": "SQL Server",
    "ConnectionString": "string",
    "Purpose": "CPS patient/search/report data access"
  },
  "CEMRDatabaseContract": {
    "Protocol": "Oracle",
    "Host": "string",
    "Port": "string",
    "ServiceName": "string",
    "Purpose": "CEMR patient/search/report data access"
  }
}
```

## Altera

Altera uses Altera/Allscripts Unity WCF plus a Midmark IQconnect ASMX framework service. WCF and ASMX are SOAP/XML surfaces, so they are represented as JSON envelopes for SDD documentation.

### Altera Unity WCF Contracts

Sources:

- `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAppsServiceLayerIQiA\UnityClient.cs`
- `Midmark.Connectors.Altera\src\MidmarkApps\MidmarkAppsServiceLayerIQiA\UnityService.cs`

```json
{
  "UnityEndpoint": {
    "Endpoint": "configured unityEndpoint",
    "Binding": "BasicHttpBinding",
    "Namespace": "http://www.allscripts.com/Unity"
  },
  "GetSecurityToken": {
    "SoapAction": "http://www.allscripts.com/Unity/IUnityService/GetSecurityToken",
    "Request": {
      "userName": "string",
      "password": "string"
    },
    "Response": {
      "securityToken": "string"
    }
  },
  "Magic": {
    "SoapAction": "http://www.allscripts.com/Unity/IUnityService/Magic",
    "Request": {
      "action": "GetTokenValidation | GetUserID | GetServerInfo | patient/context action",
      "userName": "string",
      "applicationName": "string",
      "patientId": "string",
      "token": "string",
      "parameter1": "string",
      "parameter2": "string",
      "parameter3": "string",
      "parameter4": "string",
      "parameter5": "string",
      "parameter6": "string",
      "xmlInput": "string or null"
    },
    "Response": {
      "xml": "<UnityResponse />"
    }
  },
  "ReturnMagicJSON": {
    "SoapAction": "http://www.allscripts.com/Unity/IUnityService/ReturnMagicJSON",
    "Request": {
      "action": "string",
      "userName": "string",
      "applicationName": "string",
      "patientId": "string",
      "token": "string",
      "parameters": []
    },
    "Response": {
      "json": {}
    }
  },
  "TokenOperations": [
    {
      "Operation": "GetValidSecurityToken",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/GetValidSecurityToken"
    },
    {
      "Operation": "GetValidSecurityTokenPost",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/GetValidSecurityTokenPost"
    },
    {
      "Operation": "GetTokenJsonPost",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/GetTokenJsonPost"
    },
    {
      "Operation": "RetireSecurityToken",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/RetireSecurityToken"
    },
    {
      "Operation": "RetireSecurityTokenPost",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/RetireSecurityTokenPost"
    },
    {
      "Operation": "RetireTokenJsonPost",
      "SoapAction": "http://www.allscripts.com/Unity/IUnityService/RetireTokenJsonPost"
    }
  ]
}
```

### Altera Midmark IQconnect ASMX Contracts

Source: `Midmark.Connectors.Altera\src\MidmarkFramework\MidmarkFrameworkWebService\IQConnectIF.asmx.cs`

```json
{
  "IQconnectIF": {
    "Endpoint": "/IQFrameworkWebService/IQConnectIF.asmx",
    "Contracts": [
      {
        "Method": "SetPluginsFolder",
        "Request": {
          "strPluginsFolder": "string"
        },
        "Response": null
      },
      {
        "Method": "GetPluginsRootFolder",
        "Request": {},
        "Response": "string"
      },
      {
        "Method": "SetReportManagersFolder",
        "Request": {
          "reportManagersFolder": "string"
        },
        "Response": null
      },
      {
        "Method": "GetReportManagersFolder",
        "Request": {},
        "Response": "string"
      },
      {
        "Method": "SetConfigurationRootFolder",
        "Request": {
          "configurationRootFolder": "string"
        },
        "Response": null
      },
      {
        "Method": "GetConfigurationRootFolder",
        "Request": {},
        "Response": "string"
      },
      {
        "Method": "SetSettingsRootFolder",
        "Request": {
          "settingsRootFolder": "string"
        },
        "Response": null
      },
      {
        "Method": "GetSettingsRootFolder",
        "Request": {},
        "Response": "string"
      },
      {
        "Method": "GetPluginDirectoryFromPluginId",
        "Request": {
          "strPluginId": "string"
        },
        "Response": {
          "returnValue": "string",
          "strPluginAssemblyFilePath": "string",
          "strMsiProductCode": "string"
        }
      },
      {
        "Method": "GetPluginFileNamesFromPluginId",
        "Request": {
          "strPluginId": "string"
        },
        "Response": {
          "returnValue": [
            "string"
          ],
          "iPluginAssemblyIndex": 0
        }
      },
      {
        "Method": "GetReportManagerFileNames",
        "Request": {},
        "Response": [
          "string"
        ]
      },
      {
        "Method": "GetReportManagerAllFileNames",
        "Request": {},
        "Response": [
          "string"
        ]
      },
      {
        "Method": "GetPluginPrerequisiteFileNames",
        "Request": {
          "strPluginFolder": "string",
          "strPreqName": "string",
          "strSetupProg": "string"
        },
        "Response": {
          "returnValue": [
            "string"
          ],
          "iSetupProgIndex": 0
        }
      },
      {
        "Method": "GetPluginInstallationFileNames",
        "Request": {
          "strPluginFolder": "string",
          "strSetupProg": "string"
        },
        "Response": {
          "returnValue": [
            "string"
          ],
          "iSetupProgIndex": 0
        }
      },
      {
        "Method": "DownloadFileFromFrameworkServer",
        "Request": {
          "strFilePath": "string",
          "offset": 0
        },
        "Response": {
          "returnValue": "base64",
          "isComplete": false
        }
      },
      {
        "Method": "GetServerFileVersion",
        "Request": {
          "strFilePath": "string"
        },
        "Response": "string"
      },
      {
        "Method": "SaveReportAudit",
        "Request": {
          "pluginName": "string",
          "patientFullName": "string",
          "patientDOB": "1970-01-01T00:00:00",
          "enteredDOB": "1970-01-01T00:00:00",
          "createdBy": "string",
          "bDOBMatch": true
        },
        "Response": null
      },
      {
        "Method": "EndTestNotify",
        "Request": {
          "sessionIdEMR": "string"
        },
        "Response": null
      },
      {
        "Method": "ReviewEndingNotify",
        "Request": {
          "reportId": "string"
        },
        "Response": null
      },
      {
        "Method": "ReportDataChangedNotify",
        "Request": {
          "reportId": "string"
        },
        "Response": null
      },
      {
        "Method": "GetReportListByReportType",
        "Request": {
          "strPatientId": "string",
          "strReportType": "string"
        },
        "Response": [
          "DeviceReportInfo"
        ]
      },
      {
        "Method": "GetReportListByPluginId",
        "Request": {
          "strPatientId": "string",
          "pluginId": "00000000-0000-0000-0000-000000000000"
        },
        "Response": [
          "DeviceReportInfo"
        ]
      },
      {
        "Method": "GetReportListByPatient",
        "Request": {
          "strPatientId": "string"
        },
        "Response": [
          "DeviceReportInfo"
        ]
      },
      {
        "Method": "GetReportFromFramework",
        "Request": {
          "reportId": "string"
        },
        "Response": "DeviceReport"
      },
      {
        "Method": "DeleteReportFromFramework",
        "Request": {
          "reportId": "string"
        },
        "Response": null
      },
      {
        "Method": "DoesReportExistOnFramework",
        "Request": {
          "reportId": "string"
        },
        "Response": true
      },
      {
        "Method": "PrepareXbapForTest",
        "Request": {
          "devID": "00000000-0000-0000-0000-000000000000",
          "patInfo": "PatientInfo",
          "repInfo": "ReportInfo",
          "EMRSessionId": "string",
          "user": "string",
          "site": "string"
        },
        "Response": "url"
      },
      {
        "Method": "PrepareXbapForReview",
        "Request": {
          "reportId": "string",
          "deviceId": "00000000-0000-0000-0000-000000000000",
          "patInfo": "PatientInfo",
          "EMRSessionId": "string",
          "user": "string",
          "site": "string"
        },
        "Response": "url"
      },
      {
        "Method": "PrepareXbapForCompare",
        "Request": {
          "reportList": [
            "string"
          ],
          "deviceId": "00000000-0000-0000-0000-000000000000",
          "patInfo": "PatientInfo",
          "EMRSessionId": "string",
          "user": "string",
          "site": "string"
        },
        "Response": "url"
      },
      {
        "Method": "PrepareXbapForCalibrationOrSettings",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "bForSettings": true,
          "EMRSessionId": "string",
          "user": "string",
          "site": "string"
        },
        "Response": "url"
      },
      {
        "Method": "PrepareErrorPage",
        "Request": {
          "strErrorMessage": "string",
          "EMRSessionId": "string"
        },
        "Response": "url"
      },
      {
        "Method": "GetDataFilePath",
        "Request": {
          "reportId": "string",
          "type": "string"
        },
        "Response": "string"
      },
      {
        "Method": "GetDataFileBlock",
        "Request": {
          "strFilePath": "string",
          "lOffset": 0
        },
        "Response": {
          "returnValue": "base64",
          "isComplete": false
        }
      },
      {
        "Method": "SaveDataFileBlock",
        "Request": {
          "strFilePath": "string",
          "buffer": "base64",
          "nLength": 0
        },
        "Response": true
      },
      {
        "Method": "DataFileExists",
        "Request": {
          "reportId": "string",
          "type": "string"
        },
        "Response": true
      },
      {
        "Method": "DeleteDataFile",
        "Request": {
          "strFilePath": "string"
        },
        "Response": true
      },
      {
        "Method": "GetDataFileSize",
        "Request": {
          "strFilePath": "string"
        },
        "Response": 0
      },
      {
        "Method": "GetCalibrationReportFromFramework",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "strHardwareId": "string"
        },
        "Response": "base64"
      },
      {
        "Method": "SaveCalibrationReportToFramework",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "strHardwareId": "string",
          "strSerialNumber": "string",
          "strPerformedBy": "string",
          "createDate": "2026-06-10T12:00:00",
          "reportData": "base64"
        },
        "Response": true
      },
      {
        "Method": "GetTopNCalibrationReports",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "hardwareId": "string",
          "numberOfReports": 5
        },
        "Response": [
          "IdentifiableCalibrationReport"
        ]
      },
      {
        "Method": "GetCalibrationReportsWithDateRange",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "hardwareId": "string",
          "startDate": "2026-06-01T00:00:00",
          "endDate": "2026-06-10T23:59:59"
        },
        "Response": [
          "IdentifiableCalibrationReport"
        ]
      },
      {
        "Method": "GetMostRecentCalibrationReportOfEachSensor",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000"
        },
        "Response": [
          "IdentifiableCalibrationReport"
        ]
      },
      {
        "Method": "GetCalibrationReportByIdentifier",
        "Request": {
          "pluginId": "00000000-0000-0000-0000-000000000000",
          "hardwareId": "string",
          "identifier": "string"
        },
        "Response": "IdentifiableCalibrationReport"
      },
      {
        "Method": "GetSettingsFromFramework",
        "Request": {
          "strSettingsName": "string",
          "strSiteId": "string",
          "strUserId": "string",
          "filename": "string"
        },
        "Response": "base64"
      },
      {
        "Method": "SaveSettingsToFramework",
        "Request": {
          "strSettingsName": "string",
          "settingsData": "base64",
          "strSiteId": "string",
          "strUserId": "string",
          "filename": "string"
        },
        "Response": true
      },
      {
        "Method": "GetCommonSettings",
        "Request": {
          "user": "string",
          "site": "string"
        },
        "Response": "CommonSettings"
      },
      {
        "Method": "GetListProviderList",
        "Request": {
          "listname": "string",
          "sessionIdEMR": "string"
        },
        "Response": "<xml />"
      }
    ]
  }
}
```

## Cerner

### Cerner VitalsLink Provider Contracts

Sources: `Midmark.Connectors.Cerner\src\MidmarkIQiM\VitalsLinkAPILib`

```json
{
  "AuthenticateUserRequest": {
    "contentType": "application/x-www-form-urlencoded",
    "username": "string",
    "password": "string"
  },
  "BarcodeFormatResponse": {
    "formats": [
      {
        "aliasType": "string",
        "aliasTypeId": "string",
        "barcodeType": "string",
        "checkDigitIndicator": true,
        "formatId": "string",
        "organizationId": "string",
        "postfix": "string",
        "prefix": "string"
      }
    ],
    "first": "string",
    "last": "string",
    "previous": "string",
    "next": "string"
  },
  "BarcodeToUserNameResponse": {
    "username": "string",
    "issuer": "string"
  },
  "GetLocationsResponse": {
    "locations": [
      {
        "_id": "string",
        "display": "string",
        "type": 0,
        "typeDisplay": "string",
        "parentId": "string",
        "childIds": [
          "string"
        ],
        "identifiers": [
          {
            "identifier": "string",
            "context": "string",
            "issuer": "string"
          }
        ],
        "isMobile": false,
        "isActive": true
      }
    ],
    "first": "string",
    "last": "string",
    "previous": "string",
    "next": "string"
  },
  "GetEncountersResponse": {
    "encounters": [
      {
        "_id": "string",
        "patientId": "string",
        "status": "ACTIVE",
        "visitType": "string",
        "creationDateTime": "2026-06-10T12:00:00",
        "updateDateTime": "2026-06-10T12:00:00",
        "registrationDateTime": "2026-06-10T12:00:00",
        "identifiers": [
          {
            "identifier": "string",
            "context": "string",
            "issuer": "string"
          }
        ],
        "encounterLocations": [
          {
            "locatonId": "string",
            "type": "string",
            "identifiers": [
              {
                "identifier": "string",
                "context": "string",
                "issuer": "string"
              }
            ]
          }
        ],
        "encounterPersonnels": [],
        "estimatedArriveDateTime": "2026-06-10T12:00:00",
        "arriveDateTime": "2026-06-10T12:00:00",
        "admitDateTime": "2026-06-10T12:00:00",
        "reasonForVisit": "string",
        "mentalHealthDateTime": "2026-06-10T12:00:00",
        "estimatedDischargeDateTime": "2026-06-10T12:00:00"
      }
    ],
    "first": "string",
    "last": "string",
    "previous": "string",
    "next": "string"
  },
  "GetPatientsResponse": {
    "patients": [
      {
        "_id": "string",
        "firstName": "string",
        "middleName": "string",
        "lastName": "string",
        "nameTitle": "string",
        "namePrefix": "string",
        "nameSuffix": "string",
        "gender": "string",
        "dateOfBirth": "1970-01-01",
        "deceased": false,
        "identifiers": [
          {
            "identifier": "string",
            "context": "string",
            "issuer": "string"
          }
        ]
      }
    ],
    "first": "string",
    "last": "string",
    "previous": "string",
    "next": "string"
  },
  "DeviceRegisterBody": {
    "deviceId": "string",
    "displayName": "string",
    "instanceId": "string",
    "type": 0,
    "connected": true,
    "disconReason": -1,
    "categories": [
      "DISCRETE_DATA"
    ],
    "subcategories": [
      "VITAL_SIGNS_MONITOR"
    ],
    "network": {
      "adptHostName": "string",
      "adptIP": "string",
      "adptMAC": "string",
      "conduitName": "string",
      "conduitType": "string",
      "conEngine": "string",
      "devHostName": "string",
      "devIP": "string",
      "devMAC": "string"
    },
    "model": {
      "modelName": "Digital Vital Signs Monitor",
      "serNumber": "string",
      "vendor": "MDMK"
    }
  },
  "HeartBeatRequest": {
    "heartbeats": [
      {
        "deviceId": "string",
        "instanceId": "string"
      }
    ]
  },
  "HeartBeatResponse": {
    "statuses": [
      {
        "deviceId": "string",
        "displayName": "string",
        "instanceId": "string",
        "code": 0,
        "desc": "SUCCESS"
      }
    ]
  },
  "PostVitalsRequest": {
    "patientIdentifiers": [
      {
        "identifier": "string",
        "context": "string",
        "issuer": "string"
      }
    ],
    "useDeviceTimeStamp": true,
    "deviceDisplayName": "string",
    "deviceId": "string",
    "organizationId": "string",
    "ackRequired": false,
    "username": "string",
    "discreteEntries": [
      {
        "value": "120",
        "acquiredDateTime": "2026-06-10T12:00:00",
        "clinSigDateTime": "2026-06-10T12:00:00",
        "context": "SystolicBP",
        "units": "mmHg",
        "autoVerInd": false,
        "codifiedContext": "string",
        "codifiedContextNomenclature": "CODIFIED_CONTEXT_NOMENCLAUTRE_UNKNOWN",
        "codifiedUnitType": "mmHg",
        "codifiedUnitTypeNomenclature": "CODIFIED_UNIT_NOMENCLAUTRE_UNKNOWN",
        "groupSeq": "",
        "contentSeq": 0
      }
    ],
    "encounterDescriptors": [
      {
        "key": "string",
        "value": "string"
      }
    ]
  },
  "PostVitalsResponse": {
    "status": "CHART_SUCCEEDED",
    "failureReasonText": "string"
  },
  "GetDiscreteDocumentsResponse": {
    "discreteDocuments": [
      {
        "deviceDisplayName": "string",
        "deviceId": "string",
        "organizationId": "string",
        "messageToken": "string",
        "ackRequired": false,
        "username": "string",
        "discreteEntries": [
          "Discreteentry"
        ],
        "_id": "string",
        "status": "string",
        "useDeviceTimeStamp": true,
        "source": "string",
        "encounterDescriptors": []
      }
    ],
    "first": "string",
    "last": "string",
    "previous": "string",
    "next": "string"
  },
  "VitalsData": {
    "AcquiredTime": "2026-06-10T12:00:00",
    "AcquiredTimeWeight": "2026-06-10T12:00:00",
    "AcquiredTimeHeight": "2026-06-10T12:00:00",
    "SystolicBP": 120,
    "DiastolicBP": 80,
    "Pulse": 72,
    "SpO2": 98,
    "TempInDegC": 36.8,
    "TempInDegF": 98.24,
    "TempProbeType": "string",
    "WeightInKg": 80.0,
    "WeightInLb": 176.37,
    "HeightInCm": 180.0,
    "HeightInInch": 70.87,
    "HeightInFeet": 5.91,
    "RespRate": 16,
    "PainScore": 0,
    "BMI": 24.69
  }
}
```

### Cerner Midmark Web Service Contracts

Sources: `Midmark.Connectors.Cerner\src\IQiMWebService\IQiMWebAPIModel`

Simulator status: Cerner Midmark patient, physician, and HL7 submission responses are typed contract records. `GET /api/v1/cerner/patients` returns the current SQLite patient table, including the default 15-patient seed and later imports until operator reset.

```json
{
  "PatientSearchRequest": {
    "Count": 200,
    "FirstOrder": "string",
    "SecondOrder": "string",
    "SearchString": "string",
    "SearchCheckedInOnly": true,
    "ThreeTwoSearchOnly": false
  },
  "ADTPatient": {
    "PidPatientID": "string",
    "EventDateTime": "2026-06-10T12:00:00",
    "PidPatientLastName": "string",
    "PidPatientMiddleName": "string",
    "PidPatientFirstName": "string",
    "PidDOB": "1970-01-01T00:00:00",
    "PidSex": "string",
    "PidRace": "string",
    "PidStreetAddress": "string",
    "PidCity": "string",
    "PidStateProvince": "string",
    "PidZipPostal": "string",
    "PidCountry": "string",
    "PidCountryCode": "string",
    "PidPhone": "string",
    "PidAccountNumber": "string",
    "Pv1AssignLocationPOC": "string",
    "Pv1Room": "string",
    "Pv1Bed": "string",
    "Pv1FacilityNumber": "string",
    "Pv1FacilityName": "string",
    "ObxHeight": 180.0,
    "ObxWeight": 80.0,
    "AL1AllergyType": "string",
    "AL1AllergyNnemonic": "string",
    "AL1AllergySeverity": "string",
    "CheckedIn": true,
    "LastAccessDate": "2026-06-10T12:00:00"
  },
  "ADTPatientLastAccessUpdateRequest": {
    "PidPatientID": "string",
    "LastAccessDate": "2026-06-10T12:00:00"
  },
  "Physician": {
    "PhysicianID": "string",
    "PhysicianLName": "string",
    "PhysicianMName": "string",
    "PhysicianFName": "string",
    "UpdateDate": "2026-06-10T12:00:00",
    "Active": true
  },
  "OrganizationSettings": {
    "OrgId": "string",
    "UseWindowsAuthentication": false,
    "PasswordEnabled": false,
    "DrivingPatientIdentifierType": "string",
    "EncounterDescriptorType": "string",
    "EncounterDescriptorCode": "string",
    "UseDeviceTimestamp": true,
    "AutoLogOffInMinutes": 0,
    "HL7Settings": {
      "ReportPath": "string",
      "TCPIPHost": "string",
      "IPPort": "string",
      "HL7Header": "string",
      "HL7Trailer": "string"
    }
  },
  "VitalsLinkSettings": {
    "VitalsLinkBaseURL": "string",
    "AuthenticationType": "BASIC",
    "BasicUserName": "string",
    "BasicPassword": "string",
    "TenantId": "string",
    "TenantShortName": "string",
    "BearerToken": "string",
    "TimeOutInSecs": 30
  },
  "VitalsMap": {
    "Name": "string",
    "MapTo": "string",
    "Unit": "string"
  },
  "PainScoreMap": {
    "Score": 0,
    "Text": "string"
  },
  "FilterReportRequest": {
    "DateFilter": "string",
    "ReqByPhysician": "string",
    "ReviewedBy": "string",
    "SignedBy": "string",
    "ReportType": "string",
    "PatientID": "string",
    "Count": 200
  },
  "Report": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "PatientId": "string",
    "ReportType": "ECG",
    "DeviceId": "00000000-0000-0000-0000-000000000000",
    "RequestedBy": "string",
    "Technician": "string",
    "CreateDate": "2026-06-10T12:00:00",
    "ReviewedBy": "string",
    "ReviewDate": null,
    "ReportDataPath": "string",
    "DiscreteData": "<DiscreteData />",
    "SignOffBy": "string",
    "SignOffDate": null,
    "Synopsis": "string",
    "Version": 1,
    "ReportData": "base64",
    "FirstName": "string",
    "MiddleName": "string",
    "LastName": "string",
    "CernerLocationCD": "string",
    "PatientDOB": "1970-01-01T00:00:00",
    "PatientSex": "string",
    "EncounterDateTime": "2026-06-10T12:00:00",
    "PatientDisplayID": "string"
  },
  "PendingTest": {
    "ID": 1,
    "EncounterDateTime": "2026-06-10T12:00:00",
    "PatientMRN": "string",
    "PatientDisplayID": "string",
    "LastName": "string",
    "FirstName": "string",
    "MiddleName": "string",
    "CernerLocationCD": "string",
    "Type": "ECG",
    "TestType": "ECG",
    "ErrorDescription": "string",
    "JsonPayload": "{}",
    "PdfContent": "base64",
    "PendingTestLock": {
      "PendingTestID": 1,
      "LockTime": "2026-06-10T12:00:00",
      "LockedByUser": "string",
      "LockedByMachine": "string",
      "PendingTest": null
    }
  },
  "HL7MessageRequest": {
    "Hl7Message": "MSH|..."
  },
  "SaveDataFileBlockRequest": {
    "FilePath": "string",
    "BlockBuffer": "base64",
    "BlockLength": 0
  },
  "GetDataFileBlockRequest": {
    "FilePath": "string",
    "Offset": 0
  },
  "GetDataFileBlockResponse": {
    "Buffer": "base64",
    "IsComplete": false
  },
  "GetDataFileInfoResponse": {
    "FilePath": "string",
    "FileSize": 0
  }
}
```

## Cross-provider Shared Concepts

These contracts recur across Epic and Altera framework-style APIs. The exact implementation type may come from different assemblies, but the JSON shape is the useful SDD-level contract.

```json
{
  "PatientInfo": {
    "PatientId": "string",
    "PatientDisplayId": "string",
    "LastName": "string",
    "FirstName": "string",
    "MiddleName": "string",
    "DOB": "1970-01-01T00:00:00",
    "PatientDOB": "01/01/1970",
    "Age": 56,
    "PatientSex": "Male",
    "PatientRaceCode": "Unspecified",
    "SmokingStartYear": 2001,
    "SmokingQuitYear": 2020,
    "SmokingQuantity": 1,
    "WeightInKg": 80.0,
    "WeightInLb": 176.37,
    "HeightInCm": 180.0,
    "HeightInInches": 70.87,
    "TempInDegC": 36.8,
    "TempInDegF": 98.24,
    "SystolicBP": 120,
    "DiastolicBP": 80,
    "Pulse": 72,
    "SpO2": 98,
    "BMI": 24.69,
    "RespRate": 16,
    "PainScore": 0,
    "MedicalHistory": "string",
    "Pacemaker": "string",
    "Medications": [],
    "RiskFactors": [
      "string"
    ],
    "LinkedPatientIds": [
      "string"
    ],
    "PatientNotes": "string"
  },
  "ReportInfo": {
    "ReferredBy": "string",
    "RequestedBy": "string",
    "ReviewedBy": "string",
    "SupervisedBy": "string",
    "Technician": "string",
    "Indication": "string",
    "UserID": "string",
    "NoSignOff": false
  },
  "DeviceReportInfo": {
    "ReportId": "string",
    "PatientId": "string",
    "DeviceId": "00000000-0000-0000-0000-000000000000",
    "ReportType": "ECG",
    "CreateDate": "2026-06-10T12:00:00",
    "ReferredBy": "string",
    "RequestedBy": "string",
    "SupervisedBy": "string",
    "Technician": "string",
    "ReviewedBy": "string",
    "ReviewDate": null,
    "Archived": false,
    "UploadDate": null,
    "SignOffDate": null,
    "SignOffBy": "string",
    "Version": "string",
    "IsSigned": false,
    "IsReviewed": false,
    "IsSignedOrReviewed": false,
    "IsReviewedButNotSigned": false,
    "Synopsis": "string",
    "DiscreteData": "<DiscreteData />"
  },
  "DiscreteDatum": {
    "Name": "string",
    "Description": "string",
    "Code": "string",
    "AcquireMethod": "Measured",
    "SynopsisOrder": 1,
    "InSynopsis": true,
    "Value": "string"
  }
}
```

## Validation Notes

- EPIC contracts were taken from Epic middleware controllers, request providers, FHIR DTOs, and `MCFWebModels`.
- Athena provider contracts are WCF/Unity contracts; no JSON REST DTOs were found for the provider boundary.
- Altera provider contracts are WCF/Unity contracts; the Midmark framework ASMX contracts are SOAP/XML and represented here as JSON envelopes.
- Cerner contracts include both outbound VitalsLink provider DTOs and local Midmark Cerner web-service DTOs.
