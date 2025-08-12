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
├── SmartHealthcare.API/           # API Layer - Controllers, endpoints, request handling
│   ├── Controllers/                # Patient, Doctor, Appointment, Billing, Prescription, Feedback controllers
│   ├── Program.cs                  # Application startup configuration
│   └── appsettings.json            # Application configuration (DB connection strings, etc.)
│
├── SmartHealthcare.Application/    # Application Layer - Business logic
│   ├── Interfaces/                  # Service interfaces
│   ├── Services/                     # Implementation of business logic
│   ├── DTOs/                         # Data Transfer Objects
│   └── Exceptions/                   # Custom exception handling
│
├── SmartHealthcare.Infrastructure/ # Infrastructure Layer - Database & EF Core
│   ├── Data/                         # DbContext & Migrations
│   ├── Repositories/                 # Data access implementations
│   └── Migrations/                   # EF Core migrations
│
└── SmartHealthcare.Core/            # Core Layer - Domain models & constants
    ├── Entities/                     # Patient, Doctor, Appointment, etc.
    ├── Enums/                        # Appointment status, payment status, etc.
    └── Constants/                    # Static values


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
