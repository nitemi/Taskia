# Taskia — Use Cases

## Use Case 1: Create Task with Reminder
- **Actor**: User
- **Precondition**: User is authenticated.
- **Main Flow**:
  1. User fills in Task Title, Description, Priority, Due Date, Category, and Reminder schedule.
  2. Client validates inputs locally.
  3. Client posts request to `/api/v1/tasks` (or saves to SQLite if offline).
  4. Server validates DTO using FluentValidation, creates TaskItem aggregate, schedules reminder, and returns `201 Created`.
  5. MAUI client schedules local system notification.

## Use Case 2: Offline Synchronization
- **Actor**: MAUI Client App
- **Precondition**: Device regains internet connection after offline edits.
- **Main Flow**:
  1. Client detects network availability via `Connectivity.Current`.
  2. Client pulls un-synced local changes from SQLite store.
  3. Client calls `/api/v1/sync/push` with local change payload.
  4. Server applies conflict resolution strategy (Last-Write-Wins based on `UpdatedAtUtc`).
  5. Server returns sync result with updated entity IDs and timestamp vectors.
  6. Client pulls server updates via `/api/v1/sync/pull` and updates local SQLite store.
