# Research: EMR Simulator Developer Portal

## Decision 1: Use .NET 8 with ASP.NET Core Web API for the simulator backend

- Decision: Implement the API and simulator runtime on .NET 8.
- Rationale: The architecture documents and ADR explicitly select .NET 8, and it aligns with the requested provider routing, Swagger, and testing stack.
- Alternatives considered: A different backend runtime was not pursued because it would conflict with the documented simulator stack.

## Decision 2: Use a Clean Architecture layout with explicit layer boundaries

- Decision: Separate the solution into Api, Application, Domain, Infrastructure, AdminUi, and Contracts projects.
- Rationale: The design documents require strong dependency direction and isolated provider, persistence, and UI concerns.
- Alternatives considered: A flatter project structure was rejected because it would blur boundaries and make provider-specific behavior harder to contain.

## Decision 3: Use SQLite and EF Core Fluent API for persistence

- Decision: Persist simulator state, scenarios, and logs in SQLite using EF Core mappings configured with Fluent API.
- Rationale: The source documents call for a local, zero-infrastructure store and explicitly require Fluent API rather than data annotations.
- Alternatives considered: In-memory-only storage was rejected because it would not support request logs, import persistence, or reproducible scenario state.

## Decision 4: Model provider behavior as deterministic scenarios

- Decision: Drive all provider responses through scenario state and seeded data so repeated requests produce the same outcome.
- Rationale: Determinism is required for debugging, reproducible QA, and the documented failure cases.
- Alternatives considered: Randomized mock responses were rejected because they would break reproducibility and make CI unreliable.

## Decision 5: Expose provider routes and admin workflows under a versioned API with Swagger

- Decision: Publish simulator endpoints under `/api/v1` and keep Swagger available for exploration and validation.
- Rationale: The architecture and endpoint inventory document a versioned surface and the constitution requires Swagger updates with behavior changes.
- Alternatives considered: Unversioned routes were rejected because the documented contract inventory already assumes stable versioned paths.

## Decision 6: Implement the admin experience as an Angular 20+ SPA

- Decision: Use Angular 20+ with Angular Material for the admin portal.
- Rationale: The ADR and agent build specification prefer Angular for the UI and the admin workflows include dashboard, patient management, logs, and import flows.
- Alternatives considered: A Blazor UI was not selected because the accepted design direction already favors Angular.

---

## Iteration 3 Decisions

## Decision 11: Angular 20 ESLint uses flat config (`eslint.config.mjs`)

- Decision: T003 must create `eslint.config.mjs` (ESLint flat config format), not `.eslintrc.json`.
- Rationale: Angular CLI ≥17 generates ESLint configuration in the flat config format. The legacy `.eslintrc.json` format is no longer produced and will not be picked up by `@angular-eslint` in Angular 20+.
- Alternatives considered: Using `.eslintrc.json` was rejected because it would not integrate with the Angular 20 build pipeline and the task requirement would be silently unmet.

## Decision 12: Repository interfaces belong in Application; implementations belong in Infrastructure

- Decision: `IPatientRepository`, `IAppointmentRepository`, `IOrderRepository`, and `IResultRepository` are declared in `src/EmrSimulator.Application/` and implemented in `src/EmrSimulator.Infrastructure/Persistence/`.
- Rationale: Constitution §IV requires `Api → Application ← Infrastructure`. Placing interface contracts in Application keeps the boundary clean and prevents Infrastructure from leaking into Application or Domain.
- Alternatives considered: Declaring interfaces in Infrastructure was rejected because it would violate the directional dependency rule and make the Application layer dependent on Infrastructure details.

## Decision 13: Integration tests use `WebApplicationFactory<Program>` with SQLite in-memory service override

- Decision: T059 updates the existing integration test helpers to override `AddDbContext<EmrSimulatorDbContext>` with `UseSqlite("DataSource=:memory:")` inside `ConfigureTestServices`, and call `dbContext.Database.EnsureCreated()` in the factory's `WithWebHostBuilder` setup.
- Rationale: This approach validates the full request/DI pipeline with a real EF Core provider, without requiring a file on disk. It is consistent with how the existing `WebApplicationFactory`-based tests work.
- Alternatives considered: Using `UseInMemoryDatabase` was rejected because it uses a different EF Core provider and would not validate SQLite-specific schema or FK behavior.


## Decision 7: Retain in-memory store for unit and contract tests; use SQLite for production and integration tests

- Decision: Keep `InMemoryEmrSimulatorStore` as the test double used in unit and contract tests. Wire the EF Core `DbContext` into the production startup and use a `:memory:` SQLite connection string for integration tests.
- Rationale: This avoids breaking the existing fast test suite while delivering durable persistence for the production path. Integration tests validate the real EF Core behavior without requiring a file on disk.
- Alternatives considered: Replacing in-memory tests with SQLite-backed tests was rejected because it would slow unit tests and couple them unnecessarily to persistence mechanics.

## Decision 8: Implement EF Core mappings in EntityTypeConfiguration classes, not in OnModelCreating directly

- Decision: Define one `IEntityTypeConfiguration<T>` per aggregate root in `src/EmrSimulator.Infrastructure/Persistence/Configurations/` and register them via `modelBuilder.ApplyConfigurationsFromAssembly`.
- Rationale: Separate configuration classes are easier to review, test, and extend than a single large `OnModelCreating` method. This pattern is idiomatic for EF Core Fluent API usage at scale.
- Alternatives considered: Inline `OnModelCreating` was rejected because it would grow unwieldy across eight entities and become harder to audit for constitution compliance.

## Decision 9: Fix Angular serve by invoking ng from the project root, not from the src subfolder

- Decision: All Angular CLI commands (`npm start`, `npm run build`, `ng serve`, `ng build`) must be run from `src/EmrSimulator.AdminUi/`, where `angular.json` is located.
- Rationale: The previous `ng serve` failure (exit code 1) was caused by running the command from `src/EmrSimulator.AdminUi/src/`, which is the TypeScript source subfolder and does not contain `angular.json`.
- Alternatives considered: Moving `angular.json` was rejected; it is conventional for Angular CLI to locate the workspace file at the project root.

## Decision 10: Use `Microsoft.AspNetCore.OpenApi` with `Swashbuckle.AspNetCore` for Swagger enrichment

- Decision: Add `WithSummary`, `WithDescription`, and `Produces<T>` extension calls to each minimal-API endpoint group to populate Swagger examples and descriptions.
- Rationale: The constitution requires Swagger to document all route surfaces, and the current setup only registers routes without descriptions or typed response examples.
- Alternatives considered: A standalone OpenAPI document was rejected in favor of inline decoration so descriptions stay co-located with the route handlers.