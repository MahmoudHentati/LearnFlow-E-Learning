# 🎓 LearnFlow — Modern Enterprise E-Learning Platform

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![ASP.NET Core API](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)](https://learn.microsoft.com/aspnet/core/)
[![Entity Framework Core](https://img.shields.io/badge/ORM-EF%20Core%209-512BD4)](https://learn.microsoft.com/ef/core/)
[![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20Layered-success)]()

A full-stack, enterprise-grade educational platform built with **.NET 9**, **Blazor WebAssembly**, and **ASP.NET Core Web API**, architected according to **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

---

## 🏛️ Architecture Overview

The solution follows a multi-project decoupled structure ensuring separation of concerns:

```mermaid
graph TD
    Client["🌐 E_learning.Client\n(Blazor WebAssembly UI)"]
    API["⚡ E_Learning.API\n(ASP.NET Core Web API & Identity)"]
    Models["📦 E_learning.Models\n(Shared DTOs & Contracts)"]
    Interfaces["🔌 E_learning.Interfaces\n(Repository Abstractions)"]
    Domain["🧠 E_Learning.Domain\n(Core Entities & Business Logic)"]
    Repos["🔄 E_learning.Repositories\n(Data Access Implementation)"]
    Persistence["💾 E_learning.Persistence\n(EF Core & PostgreSQL Context)"]

    Client -->|HTTP / JSON| API
    API --> Models
    API --> Interfaces
    Repos --> Interfaces
    Repos --> Persistence
    Persistence --> Domain
    Interfaces --> Domain
    Models --> Domain
```

### Projects in Solution:
* **`E_Learning.Domain`**: Core domain entities (`Formation`, `Module`, `Quiz`, `Test`, `Avis`, `Inscription`, `AppUser`).
* **`E_learning.Interfaces`**: Repository contracts and service abstractions.
* **`E_learning.Models`**: Strongly typed Data Transfer Objects (DTOs) shared across API and Client.
* **`E_learning.Persistence`**: Entity Framework Core database context, fluent configurations, and migrations for PostgreSQL.
* **`E_learning.Repositories`**: Concrete implementation of the Repository and Unit of Work patterns.
* **`E_Learning.API`**: RESTful endpoints, JWT / Cookie Identity auth, seeding pipeline.
* **`E_learning.Client`**: Single Page Application (SPA) built with Blazor WebAssembly.

---

## 🔐 Authentication & Role-Based Access Control (RBAC)

Secured using modern ASP.NET Core Identity (`MapIdentityApi<AppUser>()`) with tailored permissions:

| Role | Permissions & Capabilities |
|------|---------------------------|
| 👑 `SuperAdmin` / `Admin` | Full platform administration, category moderation, instructor validation, and user management. |
| 👨‍🏫 `Instructor` | Create and manage courses, curriculum modules, video lessons, quizzes, and grade student submissions. |
| 🎓 `Student` | Browse course catalog, self-enroll, track progression, take timed quizzes, submit assignments, and leave reviews. |

---

## 🚀 Key Functional Modules

* **Course Management:** Hierarchical taxonomy (Categories, Sub-categories, Courses) with difficulty rating and prerequisites.
* **Curriculum & Resources:** Ordered modules containing video streaming links and downloadable documents.
* **Assessment Engine:**
  * Interactive Quizzes (MCQ, True/False) with automated grading and pass-threshold validation.
  * Practical Assignments / Tests with student submission uploads and instructor feedback/scoring.
* **Enrollment & Tracking:** Real-time completion progression tracking for active students.
* **Reviews & Feedback:** Rating and testimonial system calculating dynamic course reputation.

---

## 🛠️ Quick Start

### Prerequisites
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [PostgreSQL](https://www.postgresql.org/download/)

### 1. Database Configuration
Update the connection string in `E_Learning.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=elearning_db;Username=postgres;Password=yourpassword"
  }
}
```

### 2. Apply Migrations
```bash
dotnet ef database update --project E_Learning.API --startup-project E_Learning.API
```

### 3. Launch the Backend API
```bash
dotnet run --project E_Learning.API
```

### 4. Launch the Blazor WebAssembly Client
```bash
dotnet run --project E_learning.Client
```

---

## 👥 Authors & Contributors
* **Mahmoud Hentati** ([LinkedIn](https://www.linkedin.com/in/mahmoud-hentati/) • [GitHub](https://github.com/MahmoudHentati))
* **Jihed Hajeb**
* **Yessine**
