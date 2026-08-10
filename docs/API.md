# Taskia — API Endpoint Specification

## Overview

The Taskia REST API follows standard RFC 7807 ProblemDetails for error handling, uses JWT Bearer authentication, and enforces camelCase JSON payloads.

## Base URL
`/api/v1`

## Error Response Standard (RFC 7807)
```json
{
  "type": "https://taskia.api/errors/validation-error",
  "title": "One or more validation failures occurred.",
  "status": 400,
  "detail": "See the errors property for details.",
  "errors": {
    "Email": ["Email address is invalid."]
  }
}
```

## Initial Endpoint Definitions

### 1. Health & System
- `GET /health`: Returns `{ "status": "Healthy", "timestamp": "..." }`

### 2. Auth Endpoints
- `POST /api/v1/auth/register`: Register new user
- `POST /api/v1/auth/login`: Authenticate and obtain Access + Refresh Tokens
- `POST /api/v1/auth/refresh`: Rotate refresh token
- `POST /api/v1/auth/verify-email`: Verify account via email token
- `POST /api/v1/auth/forgot-password`: Request password reset email
- `POST /api/v1/auth/reset-password`: Reset password with token

### 3. Task Management Endpoints
- `GET /api/v1/tasks`: Search/filter tasks (Query params: `keyword`, `categoryId`, `priority`, `status`, `page`, `pageSize`)
- `GET /api/v1/tasks/{id}`: Get single task item
- `POST /api/v1/tasks`: Create new task item
- `PUT /api/v1/tasks/{id}`: Update task item
- `PATCH /api/v1/tasks/{id}/complete`: Toggle completed status
- `PATCH /api/v1/tasks/{id}/archive`: Toggle archived status
- `DELETE /api/v1/tasks/{id}`: Delete task item

### 4. Categories & Tags Endpoints
- `GET /api/v1/categories`: List user categories
- `POST /api/v1/categories`: Create category
- `GET /api/v1/tags`: List user tags
- `POST /api/v1/tags`: Create tag

### 5. Synchronization Endpoints
- `POST /api/v1/sync/push`: Push offline changes from local client SQLite
- `GET /api/v1/sync/pull`: Pull modified server entities since timestamp
