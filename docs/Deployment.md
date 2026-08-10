# Taskia — Deployment & Cloud Architecture

## Infrastructure Overview

Taskia is hosted on Azure using modern cloud-native primitives:
- **API Application Host**: Azure App Service (Linux, .NET 9 runtime container / deployment).
- **Database**: Azure Database for PostgreSQL Flexible Server.
- **File Storage**: Azure Blob Storage (for task attachments).
- **Container Registry**: Azure Container Registry (ACR) or Docker Hub.

## Local Docker Deployment

Taskia includes multi-container `docker-compose` orchestration for local developer setup:

```yaml
version: '3.8'

services:
  taskia-db:
    image: postgres:16-alpine
    container_name: taskia-postgres
    environment:
      POSTGRES_DB: taskia_db
      POSTGRES_USER: taskia_user
      POSTGRES_PASSWORD: TaskiaSecurePassword123!
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U taskia_user -d taskia_db"]
      interval: 5s
      timeout: 5s
      retries: 5

  taskia-api:
    build:
      context: .
      dockerfile: src/Taskia.Api/Dockerfile
    container_name: taskia-api
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=taskia-db;Database=taskia_db;Username=taskia_user;Password=TaskiaSecurePassword123!
    depends_on:
      taskia-db:
        condition: service_healthy

volumes:
  postgres_data:
```
