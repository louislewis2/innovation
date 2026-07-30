# Innovation

A simple framework to implement **CQRS** in .NET applications with **immediate consistency**.  
Innovation does not implement, and does not attempt to support, Event Sourcing.

Innovation is **CQRS-first** and uses a **mediator-style dispatch pipeline** internally to route commands/queries to handlers and apply cross-cutting behaviors.

---

## Why Innovation

- **CQRS-first design** for explicit write/read separation.
- **Free forever** (no intent to charge).
- **Simple request/handler model** with low ceremony.
- **Clean architecture alignment** for maintainable boundaries.
- **Extensible pipeline** for validation, logging, transactions, and other cross-cutting concerns.

---

## Project Direction

A new implementation focused primarily on performance improvements is being developed as a separate repository:

[Innovation-vNext](https://github.com/louislewis2/innovation-vnext)

Both implementations are maintained, with future development focus centered on vNext.

---

## CQRS vs Mediator (Important Terminology)

- **CQRS** is the architectural pattern: commands (writes) and queries (reads) are separated.
- **Mediator** is the dispatch mechanism: requests are routed through a central pipeline to handlers.
- **Innovation**: CQRS is the primary model; mediator-style dispatch is how execution is coordinated.

---

## Clean Architecture Alignment

Innovation naturally supports Clean Architecture boundaries:

- **Presentation layer** (API/UI): creates and dispatches commands/queries.
- **Application layer**: contains command/query handlers, validators, interceptors.
- **Domain layer**: contains business rules, isolated from transport concerns.
- **Infrastructure layer**: provides persistence, external integrations, auditing.

This keeps endpoints/controllers thin and use-case logic explicit and testable.

---

## Dispatcher Command Pipeline

Commands:  
`Dispatcher -> Command Reactors -> Command Interceptors -> Command Validators -> Command Handler -> Command Result Reactors -> Audit Store -> Return Result`

---

## Framework Components

### Command Reactors

First step in command dispatch. Useful for logging or priming external/internal services about an incoming command.

- Command is passed by reference (mutation here is discouraged).
- Reactors do not influence pipeline execution.
- Run in parallel on a background thread.
- Must implement `ICommandReactor`

### Command Interceptors

Second step in command dispatch.

- Can modify commands/properties where required.
- Run sequentially (one after another, not in parallel).
- Must implement `ICommandInterceptor`

### Command Validators

Third step in command dispatch (separation of concerns).

- Validate command input before handler execution.
- If validation fails, handler is not called.
- If multiple validators exist, processing stops after first error-producing validator.
- Must implement `IValidator`

### Command Validation (Fallback)

If custom command validators are not registered, the framework validates using:

1. `System.ComponentModel.DataAnnotations.Validator`
2. `System.ComponentModel.DataAnnotations.IValidatableObject` (if implemented)

On failure, handler execution is skipped and validation errors are returned.

### Commands Handlers

Fourth step in command dispatch.

- Used to alter state.
- Must implement `ICommand`.
- Exactly one handler per command.

### Command Result Reactors

Fifth step in command dispatch.

- Useful for logging and auditing side effects.
- Do not influence execution outcome.
- Run in parallel on a background thread.
- Must implement `ICommandResultReactor`

### Audit Store

Final step in command dispatch.

Supports centralized auditing of commands, queries, and messages.

- Implement `IAuditStore`
- Register with DI
- If present, audit hooks are called; if absent, skipped.

## Query Pipeline

Queries:  
`Dispatcher -> Audit Store -> Query Handler  -> Return Result`

---

### Queries

- Used to read/load data.
- Must implement `IQuery`.
- Exactly one handler per query.

### Query Results

- Returned by query handlers.
- Must implement `IQueryResult`.
- Interface is framework-tracking oriented; no required fields/properties.

### Messages

Messages can broadcast to multiple handlers.

### Correlation

Dispatcher can create or consume an incoming correlation ID.

- ASP.NET Core implementation available using `X-Correlation-ID`.
- Handlers can implement `ICorrelationAware`.
- Dispatcher sets `CorrelationId` before `Handle` is called.

### SearchLocations

The Innovation loader can load assemblies from specified locations to support modular architectures.

---

## Dispatcher Context

> Reserved section (recommended: document how correlation, audit, and dispatch metadata are carried per request).

---

## Supported .NET Frameworks

1. .NET Standard 2.0
2. .NET 9.0

---

## Samples

- `Innovation.Sample.Console`
- `Innovation.Sample.Web`

---

## Tests

One primary test project plus two additional test-directory projects used to validate loading behavior.

---

## Building

1. Visual Studio 2022 >= 17.14.37
2. Latest .NET SDK  
   [Download](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
3. Latest .NET Runtime  
   [Download](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

---

## Benchmark Results

### Unit reference
- 1 second = 1,000 ms
- 1 second = 1,000,000 us
- 1 second = 1,000,000,000 ns

### DataAnnotationsValidator with `BlankCommand`

| Method          | Mean     | Error   | StdDev  | Gen0   | Allocated |
|----------------|---------:|--------:|--------:|-------:|----------:|
| BlankCommandNew | 460.1 ns | 6.35 ns | 5.63 ns | 0.1044 | 1.07 KB |

Operations per second: `1,000,000,000 / 460.1 = 2,173,440`

### DataAnnotationsValidator with `InsertCustomer`

| Method                | Mean     | Error     | StdDev    | Gen0   | Allocated |
|----------------------|---------:|----------:|----------:|-------:|----------:|
| InsertCustomerCommand | 2.478 us | 0.0166 us | 0.0130 us | 0.3281 | 3.36 KB |

Operations per second: `1,000,000 / 2.478 = 403,551`

### Dispatcher with `BlankCommand`

| Method               | Mean     | Error     | StdDev    | Gen0   | Allocated |
|---------------------|---------:|----------:|----------:|-------:|----------:|
| DispatchBlankCommand | 2.032 us | 0.0128 us | 0.0113 us | 0.2441 | 2.5 KB |

Operations per second: `1,000,000 / 2.032 = 492,125`

### Benchmark interpretation guidance

These results are from current measured scenarios and are useful for directional evaluation.  
Performance varies by runtime, hardware, workload shape, and enabled pipeline behaviors.