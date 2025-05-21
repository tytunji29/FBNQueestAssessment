# TeamTaskAPI

A .NET 9 Web API for managing teams and tasks with role-based access, JWT authentication, and entity relationships.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 9 (Web API)
- **Language:** C#
- **Database:** SQL Server (EF Core)
- **Authentication:** JWT Bearer Token
- **ORM:** Entity Framework Core
- **Logging:** Serilog
- **API Docs:** Swagger / Swashbuckle
- **Unit Testing:** xUnit (optional)

---

## ✅ Features

- User registration and login with hashed passwords
- Role-based authorization (Admin, Member)
- Create and manage teams and tasks
- Assign users to teams and tasks
- Get task statuses (Pending, InProgress, Completed)
- Swagger UI for API documentation

# Assumptions

I assume That The Super admin Create all user then all Team And Task The Add all The users to task
Then the user  in the team with ADMIN role are the only one o delete/uopdate a task assigned to the team
---

## 🚀 Setup Steps

### 1. Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Postman or curl for testing

### 2. Clone the repository

```bash
git clone https://github.com/tytunji29/FBNQueestAssessment.git
cd TeamTaskAPI
