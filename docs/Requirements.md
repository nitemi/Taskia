# Taskia — Requirements Specification

## 1. Functional Requirements

### 1.1 Authentication & User Management
- FR-1.1: Users must be able to register with email, username, and strong password.
- FR-1.2: Users must verify email before full access.
- FR-1.3: Secure JWT Access Tokens (short-lived) + Refresh Token rotation (stored securely in Client Storage / HTTP-only cookie).

### 1.2 Task Organization
- FR-2.1: Users can create, update, delete, complete, and archive tasks.
- FR-2.2: Tasks support Priority (Low, Medium, High, Urgent), Due Date, Notes, Tags, Category, and File Attachments (Azure Blob Storage).

### 1.3 Reminders & Notifications
- FR-3.1: Support one-time and recurring (Daily, Weekly, Monthly, Custom) reminders.
- FR-3.2: MAUI local scheduled notifications and server push notifications.

### 1.4 Offline Sync
- FR-4.1: MAUI app operates seamlessly with zero connectivity using local SQLite store.
- FR-4.2: Bi-directional synchronization reconciling updates via UTC timestamp vectors.

## 2. Non-Functional Requirements
- NFR-1 (Security): Passwords hashed with BCrypt/Argon2. JWTs signed with RSA/HMAC-SHA256.
- NFR-2 (Performance): API responses under 150ms for task lists.
- NFR-3 (Maintainability): Clean Architecture layer rules strictly enforced via build and unit tests.
