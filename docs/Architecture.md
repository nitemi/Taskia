# Taskia — System Architecture Specification

## Architectural Overview

Taskia is designed following **Clean Architecture** principles as popularized by Robert C. Martin, combined with **Domain-Driven Design (DDD)** concepts and **CQRS (Command Query Responsibility Segregation)** readiness.

```
┌─────────────────────────────────────────────────────────┐
│                 Taskia.Maui (Presentation)               │
└────────────────────────────┬────────────────────────────┘
                             │ HTTP / DTOs
┌────────────────────────────▼────────────────────────────┐
│                 Taskia.Api (Presentation Host)           │
└──────────────┬──────────────────────────┬───────────────┘
               │ Composition Root         │
┌──────────────▼──────────────┐ ┌─────────▼───────────────┐
│    Taskia.Infrastructure    │ │    Taskia.Application   │
│  (EF Core, PostgreSQL, JWT) ├─► (Use Cases, Interfaces) │
└──────────────┬──────────────┘ └─────────┬───────────────┘
               │                          │
               └────────────┬─────────────┘
                            │
              ┌─────────────▼─────────────┐
              │       Taskia.Domain       │
              │ (Entities, Value Objects) │
              └───────────────────────────┘
```

## Layer Responsibilities

### 1. Taskia.Domain
- **Dependencies**: None (Pure C#).
- **Contains**: Entities, Enums, Value Objects, Domain Events, Domain Exceptions.
- **Rules**:
  - No ORM attributes (e.g., EF `[Key]`, `[Table]`).
  - No framework references (`Microsoft.AspNetCore`, `EntityFrameworkCore`, etc.).
  - Business rules are encapsulated in aggregate entities and domain models.

### 2. Taskia.Application
- **Dependencies**: `Taskia.Domain`.
- **Contains**: Use cases, Application DTOs, Repository Interfaces, Service Interfaces (`IApplicationDbContext`, `ITokenService`, `IEmailService`, `IDateTimeProvider`), Result abstractions.
- **Rules**:
  - Defines what the system does without committing to *how* data is stored or transported.
  - Unit testable in isolation without a database.

### 3. Taskia.Infrastructure
- **Dependencies**: `Taskia.Application`, `Taskia.Domain`.
- **Contains**: Persistence implementations (`TaskiaDbContext`, EF Core Fluent Configurations, Repositories), JWT token generators, Email senders, External integration services.
- **Rules**:
  - Implements interfaces declared in Application.
  - Handles database migrations and data access logic.

### 4. Taskia.Api
- **Dependencies**: `Taskia.Application`, `Taskia.Infrastructure` (DI wiring only).
- **Contains**: ASP.NET Core Controllers, Exception Handling Middleware, Serilog logging setup, Swagger configuration, DI Composition Root.
- **Rules**:
  - Controllers depend strictly on `Taskia.Application` abstractions.
  - Maps HTTP requests to Commands/Queries and domain results to ProblemDetails responses.

### 5. Taskia.Maui
- **Dependencies**: `Taskia.Application` (DTOs/Contracts).
- **Contains**: MVVM Cross-Platform UI (Android, iOS, Windows) using CommunityToolkit.Mvvm.
- **Rules**:
  - Communicates with `Taskia.Api` strictly over typed HTTP APIs.
  - Offline-first SQLite local store mirroring server schema for offline caching and synchronization.
