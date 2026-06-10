# PRD - EMR Simulator Developer Portal

## Vision
Provide developers with a local EMR simulation platform that replaces dependency on vendor test environments.

## Target Users
- Software Engineers
- QA Engineers
- Integration Engineers
- Solution Architects

## User Stories
1. As a developer, I want to switch between Epic and Cerner so I can validate provider-specific behavior.
2. As a QA engineer, I want to simulate failures so I can verify error handling.
3. As an architect, I want deterministic test data so environments are reproducible.
4. As a developer, I want to import mock patients from CSV/JSON.

## MVP Features
- Multi-provider EMR simulation
- Patient management
- Appointment lookup
- Order creation
- Result retrieval
- Scenario engine
- Request/response logs
- Swagger

## Success Metrics
- < 5 min local setup
- 100% offline operation
- < 1 sec average mock response
- 90% reduction in external EMR dependency
