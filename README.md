# Employee Attendance Tracker

## Overview

**Employee Attendance Tracker** is an ASP.NET MVC web application for managing employees, departments, and attendance records. It uses Entity Framework Core (Code-First) with an in-memory database and follows a strict N-tier architecture:

- **Presentation Layer:** ASP.NET MVC (Views & Controllers)
- **Business Layer:** Services (all business logic)
- **Data Layer:** Entity Framework Core (repositories, models)

All business logic and validation are enforced in the service layer. The project demonstrates best practices in separation of concerns, dependency injection, and modern ASP.NET MVC development.

---

## Features

- **Department Management:**
  - Create, edit, delete, and list departments
  - Department code and name uniqueness validation
  - Employee count per department
- **Employee Management:**
  - Create, edit, delete, and list employees
  - Assign employees to departments
  - Paginated employee list
  - View employee attendance history
- **Attendance Management:**
  - Mark attendance (Present/Absent) for employees
  - Unified Attendance Dashboard: calendar, quick marking, filtering, and records in one page
  - Live filtering/search (by employee name, department, status, date)
  - Partial views for attendance history and employee details
  - Prevent marking attendance for future dates
- **Dynamic UI:**
  - jQuery calendar for date selection
  - AJAX for live updates (attendance marking, filtering)
  - Bootstrap modals for delete confirmations
  - Alert system for user feedback
- **Data Seeding:**
  - Sample departments, employees (12+), and attendance records are seeded on startup
- **Generic Repository Pattern:**
  - Centralized data access with generic `GetByIdAsync`
- **AutoMapper:**
  - Entity/ViewModel mapping

---

## Architecture

```
Presentation Layer (ASP.NET MVC)
│
├── Controllers
├── Views (Razor, Partial Views)
│
├── ViewModels
│
├── Business Layer
│   ├── Services (business logic, validation)
│   ├── DTOs
│   └── Profiles (AutoMapper)
│
└── Data Layer
    ├── Models (entities)
    ├── Repository (generic & specific)
    ├── Interfaces
    └── DbContext (EF Core)
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022+ or VS Code

### Setup

1. **Clone the repository:**
   ```
   git clone <your-repo-url>
   cd CodeZone_Mohamed_Ramadan
   ```
2. **Restore NuGet packages:**
   ```
   dotnet restore
   ```
3. **Build the solution:**
   ```
   dotnet build
   ```
4. **Run the application:**

   ```
   dotnet run --project Mohamed_Ramadan_Code_Zone_Task/Mohamed_Ramadan_Code_Zone_Task.csproj
   ```

   Or use Visual Studio: Set `Mohamed_Ramadan_Code_Zone_Task` as the startup project and press F5.

5. **Access the app:**
   - Open your browser and navigate to `http://localhost:5000` (or the port shown in the console)

---

## Usage

- **Departments:** Manage from the Departments tab. You can add, edit, or delete departments. Deleting a department with employees is prevented.
- **Employees:** Manage from the Employees tab. Add, edit, or delete employees. View paginated lists and attendance history.
- **Attendance Dashboard:**
  - Unified page for viewing, filtering, and marking attendance
  - Live search for employees (type to filter)
  - Quick marking: select status (Present/Absent) and mark attendance
  - Calendar for date selection
  - Pagination for large record sets
- **Alerts & Modals:** All actions provide user feedback via alerts and confirmation modals.

---

## Technical Details

- **In-Memory Database:** All data is stored in-memory and reset on application restart.
- **Seeding:** Sample data is seeded at startup via `DataSeedingService`.
- **AutoMapper:** Used for mapping between entities and ViewModels/DTOs.
- **Validation:** All validation and business rules are enforced in the service layer.
- **Generic Repository:** Centralized data access with support for includes and reflection-based primary key lookup.
- **AJAX & jQuery:** Used for dynamic UI updates, live filtering, and partial view loading.
- **Bootstrap:** Used for responsive design and modals.

---

## Project Structure

- `Mohamed_Ramadan_Code_Zone_Task/` — Presentation Layer (MVC)
- `Business Layer/` — Business logic, services, DTOs, ViewModels
- `Data Layer/` — EF Core models, repositories, DbContext

---

## Customization

- **Change Seed Data:** Edit `Business Layer/Services/DataSeedingService.cs` to modify initial departments, employees, or attendance records.
- **Switch to SQL Database:** Replace the in-memory provider in `Program.cs` and update connection strings as needed.

---

## Credits

Developed by Mohamed Ramadan as an ASP.NET MVC/EF Core N-tier architecture sample project.

---

## License

This project is for educational/demo purposes. Please contact the author for other uses.
