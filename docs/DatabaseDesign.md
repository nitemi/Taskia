# Taskia — Database Design Document

## Relational Schema (PostgreSQL)

The Taskia data persistence model is structured around core productivity aggregates: `Users`, `TaskItems`, `Categories`, `Tags`, `Reminders`, and `RecurrenceRules`.

```
               ┌────────────────┐
               │     Users      │
               ├────────────────┤
               │ Id (PK)        │
               │ Email          │
               │ Username       │
               │ PasswordHash   │
               │ IsEmailVerified│
               └───────┬────────┘
                       │ 1
                       │
        ┌──────────────┼──────────────┐
        │ N            │ N            │ N
┌───────▼────────┐  ┌──▼─────────┐  ┌─▼─────────────┐
│   Categories   │  │   Tags     │  │   TaskItems   │
├────────────────┤  ├────────────┤  ├───────────────┤
│ Id (PK)        │  │ Id (PK)    │  │ Id (PK)       │
│ Name           │  │ Name       │  │ Title         │
│ ColorHex       │  │ ColorHex   │  │ Description   │
│ UserId (FK)    │  │ UserId(FK) │  │ Status        │
└───────┬────────┘  └─────┬──────┘  │ Priority      │
        │ 1               │ M       │ DueDateUtc    │
        │                 │         │ CategoryId(FK)│
        │ N               │ N       │ Recurrence(FK)│
┌───────▼─────────────────┴──────┐  │ UserId (FK)   │
│         TaskItemTags           │  └──────┬────────┘
├────────────────────────────────┤         │ 1
│ TaskItemId (FK, PK)            │         │
│ TagId (FK, PK)                 │         │ N
└────────────────────────────────┘  ┌──────▼────────┐
                                    │   Reminders   │
                                    ├───────────────┤
                                    │ Id (PK)       │
                                    │ ScheduledAtUtc│
                                    │ ReminderType  │
                                    │ IsSent        │
                                    │ TaskItemId(FK)│
                                    └───────────────┘
```

## Entity Table Definitions

### 1. Users (`users`)
- `id` (uuid, PK)
- `email` (varchar(256), Unique, Not Null)
- `username` (varchar(100), Unique, Not Null)
- `password_hash` (varchar(500), Not Null)
- `is_email_verified` (boolean, Default false)
- `created_at_utc` (timestamp with time zone, Not Null)
- `updated_at_utc` (timestamp with time zone, Nullable)

### 2. TaskItems (`task_items`)
- `id` (uuid, PK)
- `user_id` (uuid, FK -> users.id, Not Null, Indexed)
- `title` (varchar(200), Not Null)
- `description` (text, Nullable)
- `status` (integer, Not Null, Default 0 [Pending])
- `priority` (integer, Not Null, Default 1 [Medium])
- `due_date_utc` (timestamp with time zone, Nullable, Indexed)
- `category_id` (uuid, FK -> categories.id, Nullable, Indexed)
- `recurrence_rule_id` (uuid, FK -> recurrence_rules.id, Nullable)
- `is_archived` (boolean, Default false, Indexed)
- `created_at_utc` (timestamp with time zone, Not Null)
- `updated_at_utc` (timestamp with time zone, Nullable)

### 3. Categories (`categories`)
- `id` (uuid, PK)
- `user_id` (uuid, FK -> users.id, Not Null, Indexed)
- `name` (varchar(100), Not Null)
- `color_hex` (varchar(7), Default '#6366F1')
- `icon` (varchar(50), Nullable)
- `created_at_utc` (timestamp with time zone, Not Null)

### 4. Tags (`tags`)
- `id` (uuid, PK)
- `user_id` (uuid, FK -> users.id, Not Null, Indexed)
- `name` (varchar(50), Not Null)
- `color_hex` (varchar(7), Default '#8B5CF6')

### 5. TaskItemTags (`task_item_tags`) — Join Table
- `task_item_id` (uuid, FK -> task_items.id, PK)
- `tag_id` (uuid, FK -> tags.id, PK)

### 6. Reminders (`reminders`)
- `id` (uuid, PK)
- `task_item_id` (uuid, FK -> task_items.id, Not Null, Indexed)
- `scheduled_at_utc` (timestamp with time zone, Not Null, Indexed)
- `reminder_type` (integer, Not Null, Default 0 [LocalNotification])
- `is_sent` (boolean, Default false, Indexed)
- `snoozed_until_utc` (timestamp with time zone, Nullable)

### 7. RecurrenceRules (`recurrence_rules`)
- `id` (uuid, PK)
- `recurrence_type` (integer, Not Null [Daily=0, Weekly=1, Monthly=2, Custom=3])
- `interval` (integer, Default 1)
- `days_of_week` (integer, Flags Bitmask)
- `end_date_utc` (timestamp with time zone, Nullable)

## Indexing Strategy
- Composite index on `task_items(user_id, status, due_date_utc)` for fast dashboard queries.
- Index on `reminders(scheduled_at_utc, is_sent)` for background reminder polling job.
