---
description: "Use when writing or modifying C# code. Covers naming conventions, code style, and best practices for C# development."
applyTo: "**/*.cs"
---

# C# Development Style Guidelines

## Naming Conventions

- Use `PascalCase` for public members, types, namespaces, and methods
- Use `camelCase` for local variables and parameters
- Prefix private fields with an underscore: `_fieldName`
- Prefix interfaces with `I`: `IRepository`
- Use meaningful, descriptive names; avoid abbreviations

## Code Style

- Use file-scoped namespaces
- Use `var` when the type is obvious from the right side of the assignment
- Prefer expression-bodied members for single-line implementations
- Use pattern matching where it improves readability
- Place `using` directives outside the namespace, sorted alphabetically
- Use primary constructors where appropriate (.NET 8+)

## Best Practices

- Favor immutability: use `readonly`, `init`, and records where appropriate
- Use nullable reference types and annotate nullability explicitly
- Prefer async/await over synchronous blocking calls
- Use dependency injection over static dependencies
- Keep methods short and focused on a single responsibility
- Seal classes that are not designed for inheritance

## SOLID Principles

- **Single Responsibility**: Each class should have one reason to change
- **Open/Closed**: Design classes to be extended without modification; use abstractions and polymorphism
- **Liskov Substitution**: Subtypes must be substitutable for their base types without altering correctness
- **Interface Segregation**: Prefer small, focused interfaces over large general-purpose ones
- **Dependency Inversion**: Depend on abstractions, not concretions; inject dependencies via constructor

## Error Handling

- Throw specific exceptions, not `Exception`
- Use guard clauses for parameter validation at public API boundaries
- Do not catch exceptions unless you can handle them meaningfully

## Logging

- Use `ILogger<T>` via dependency injection; avoid static loggers
- Log at appropriate levels: `Trace`/`Debug` for diagnostics, `Information` for key business events, `Warning` for recoverable issues, `Error`/`Critical` for failures
- Include structured logging with meaningful properties (e.g., `_logger.LogInformation("Order {OrderId} processed", orderId)`)
- Log at service boundaries, entry/exit of significant operations, and in catch blocks
- Do not log sensitive data (PII, credentials, tokens)

## Collections and LINQ

- Prefer `IReadOnlyList<T>` or `IReadOnlyCollection<T>` for return types when mutation is not needed
- Use LINQ for queries but avoid deeply nested or overly complex expressions
- Prefer method syntax for multi-step LINQ chains
