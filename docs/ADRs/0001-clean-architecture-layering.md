# ADR 0001: Strict Clean Architecture Layering & Dependency Flow

- **Status**: Approved
- **Date**: 2026-08-01
- **Deciders**: Lead Architect

## Context

Taskia requires long-term maintainability, offline-first sync capability, and complete testability of business logic without coupling to databases, web frameworks, or mobile UI SDKs.

## Decision

We adopt **Strict Clean Architecture** with four core solution layers:
1. `Taskia.Domain`: Pure C# core containing entities, domain events, value objects, and domain exceptions. Zero dependencies on external packages or framework assemblies.
2. `Taskia.Application`: Use case specifications, application DTOs, repository interfaces, service contracts, and validation rules. Depends strictly on `Taskia.Domain`.
3. `Taskia.Infrastructure`: EF Core PostgreSQL persistence, JWT token generation, external email senders, and infrastructure abstractions. Depends on `Taskia.Application` and `Taskia.Domain`.
4. `Taskia.Api` / `Taskia.Maui`: Presentation hosts depending on `Taskia.Application` and using `Taskia.Infrastructure` strictly for Dependency Injection registration at startup.

## Alternatives Considered

- **Monolithic Web API with Direct EF Core Access in Controllers**: Rejected due to high coupling, poor unit testability, and difficulty maintaining offline-first client synchronization.
- **Traditional 3-Tier Layering (UI -> BLL -> DAL)**: Rejected because dependencies point downward towards the database rather than inward towards the domain model, violating the Dependency Inversion Principle.

## Consequences

- Business logic can be unit-tested thoroughly in milliseconds without database or HTTP mocks.
- Entity Framework Core attributes (`[Key]`, `[Table]`) are excluded from Domain entities; configuration is handled via Fluent Configurations in Infrastructure.
- Adding new presentation clients (MAUI, CLI, Web SPA) requires zero modifications to business or domain logic.
