# Constitution Gates

## Synthetic Data Only And Offline By Default

Pass. New compatibility routes return synthetic payloads and reject credential markers that look real.

## Provider Contract Fidelity

Pass. Native connector-facing paths are preserved for Epic, Cerner, Unity, ASMX, browser routes, FHIR, and HL7 while simulator-owned APIs remain under `/api/v1`.

## Deterministic Scenario Engine

Pass. Existing scenario behavior remains intact; reset clears generated state without live EMR dependencies.

## Clean Architecture And Explicit Boundaries

Pass. DTOs live in Contracts, interfaces in Application, EF/services in Infrastructure, and route mappings in Api.

## Observable, Tested, And Versioned Changes

Pass. Catalog, reset, evidence, provider smoke, and Admin UI build validation are recorded in this verification folder.