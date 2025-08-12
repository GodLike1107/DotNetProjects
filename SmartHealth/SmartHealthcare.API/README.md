# 🏥 SmartHealthcareSolution

A full-stack .NET 8-based healthcare management system that enables hospitals, clinics, and medical professionals to efficiently manage patients, appointments, billing, and user authentication.  
Built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**.

---

## 🚀 Features
- **👩‍⚕️ Patient Management** — Create, update, view, and delete patient records.
- **📅 Appointment Scheduling** — Link appointments to patients and doctors with time slot management.
- **💳 Billing System** — Generate and manage billing records for services rendered.
- **🛡 JWT Authentication & Authorization** — Secure endpoints using token-based authentication.
- **📊 Modular Architecture** — Clear separation of concerns for maintainability.
- **🔗 EF Core Relationships** — Doctors, Patients, and Appointments linked with proper relationships.
- **📜 API-First Design** — Well-structured REST API ready for integration with frontend apps.

---

## 📂 Folder Structure
SmartHealthcareSolution/
│
├── SmartHealthcare.API/ # ASP.NET Core Web API project
│ ├── Controllers/ # API controllers for each entity
│ │ ├── PatientsController.cs
│ │ ├── DoctorsController.cs
│ │ ├── AppointmentsController.cs
│ │ ├── BillsController.cs
│ │
│ ├── Models/ # Entity models (Patient, Doctor, Appointment, Bill)
│ ├── Data/ # DbContext and configuration files
│ ├── DTOs/ # Data Transfer Objects
│ ├── Services/ # Business logic services
│ ├── Migrations/ # EF Core migrations
│ ├── appsettings.json # Configuration (DB connection, JWT settings)
│ ├── Program.cs # Entry point
│
└── README.md # Project documentation

yaml
Copy
Edit

---

## ⚙️ Setup Instructions

### 1️⃣ Prerequisites
Make sure you have the following installed:
- **Visual Studio 2022** with .NET 8 SDK
- **SQL Server** (LocalDB or SQL Server Express/Full)
- **EF Core Tools**  
  Install globally:
  ```bash
  dotnet tool install --global dotnet-ef
