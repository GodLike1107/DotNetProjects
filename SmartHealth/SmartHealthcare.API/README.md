# SmartHealthcareSolution

A hospital and healthcare management system built with **.NET 8** in **Visual Studio 2022**.  
This system helps manage **patients, doctors, appointments, prescriptions, billing, and feedback** efficiently, with a clean architecture and scalable design.

---

## 🚀 Features

### 🧑‍⚕️ Patient Management
- Add, update, and delete patient records
- Store patient medical history and contact details

### 👨‍⚕️ Doctor Management
- Maintain doctor profiles, specializations, and availability schedules

### 📅 Appointment Scheduling
- Book, update, and cancel appointments
- Link appointments to both patients and doctors
- Track appointment status (Booked, Completed, Cancelled)

### 💊 Prescription Management
- Doctors can create prescriptions after appointments
- Patients can view/download prescriptions as PDF
- Option to send prescriptions via email

### 💵 Billing System
- Generate patient bills based on services provided
- View and update payment status
- Maintain history of transactions

### ⭐ Feedback & Ratings
- Patients can rate doctors (1–5) and add comments
- Calculate and display average doctor ratings
- Optional admin moderation for reviews

### 🏗 Extensible Architecture
- Follows **Clean Architecture** principles:
  - **API Layer** – Controllers & Endpoints
  - **Application Layer** – Business logic & services
  - **Infrastructure Layer** – EF Core, database access
  - **Core Layer** – Entities & DTOs
- DTOs are separate from Entities for clean API boundaries

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


