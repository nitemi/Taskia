# Taskia — Cross-Platform Productivity Application

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](#)
[![NET Version](https://img.shields.io/badge/.NET-9.0-blue)](#)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-purple)](#)

Taskia is an enterprise-grade, cross-platform productivity application (Android, iOS, Windows) built with **.NET MAUI** on the frontend and **ASP.NET Core (.NET 9)** on the backend.

## Features & Architecture

- **Clean Architecture**: Strict unidirectional dependencies (`Domain` <- `Application` <- `Infrastructure` / `Api`).
- **Offline-First MAUI Client**: Local SQLite synchronization with timestamp-vector conflict resolution.
- **Domain Modeling**: Rich domain primitives, entities, and domain exceptions without ORM leakage.
- **Structured Logging & Diagnostics**: Serilog configured with console and file sinks.
- **RFC 7807 Exception Pipeline**: Global middleware mapping domain & application errors to standardized ProblemDetails.
- **Database & Persistence**: Entity Framework Core with PostgreSQL.

## Solution Structure

```
Taskia/
├── docs/
│   ├── Architecture.md
│   ├── DatabaseDesign.md
│   ├── API.md
│   ├── Requirements.md
│   ├── UseCases.md
│   ├── SequenceDiagrams.md
│   ├── Deployment.md
│   └── ADRs/
├── src/
│   ├── Taskia.Api
│   ├── Taskia.Application
│   ├── Taskia.Domain
│   ├── Taskia.Infrastructure
│   └── Taskia.Maui
└── tests/
    ├── Taskia.Domain.Tests
    ├── Taskia.Application.Tests
    └── Taskia.Infrastructure.Tests
```

## Quick Start

### Prerequisites
- .NET 9 SDK (or .NET 10 SDK with net9.0 workload)
- PostgreSQL database instance or Docker Desktop

### Run the API
```bash
dotnet run --project src/Taskia.Api/Taskia.Api.csproj
```

### Run Tests
```bash
dotnet test Taskia.slnx
```
