# MagicBricks Clone - Property Search Website

A full-stack Indian property search website inspired by [MagicBricks](https://www.magicbricks.com/), built with **.NET Core Web API**, **Angular**, and **Microsoft SQL Server**.

## Features

- 🔍 **Property Search** — Filter by city, listing type (Buy/Rent), budget, BHK, property type, furnishing, keyword
- 💰 **Budget Filters** — Indian currency labels (₹ Lac / ₹ Cr for Buy, ₹/month for Rent)
- 🔀 **Sort** — By price, date posted, or area
- ♥ **Favorites** — Session-based property shortlisting with dedicated page
- 📞 **Contact Form** — Submit inquiries directly from property detail pages
- 📱 **Responsive Design** — Mobile-first dark theme with glassmorphism UI
- 🏙️ **2 Cities** — Bangalore and Mumbai with 10 mock properties each

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Angular 20 |
| Backend | .NET Core 10 Web API |
| Database | Microsoft SQL Server (SQLEXPRESS) |
| ORM | Entity Framework Core |

## Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js 24+
- Angular CLI
- SQL Server (SQLEXPRESS instance)

### Run Backend
```bash
cd MagicBricks.API
dotnet run --urls "https://localhost:5001;http://localhost:5000"
```
> The database `MagicBricksDB` is auto-created and seeded on first run.

### Run Frontend
```bash
cd magicbricks-ui
npm install
ng serve
```
> Open http://localhost:4200

## Project Structure

```
Magicbricks/
├── MagicBricks.API/          # .NET Core Web API
│   ├── Controllers/          # REST API endpoints
│   ├── Models/               # Entity models
│   ├── DTOs/                 # Data transfer objects
│   ├── Data/                 # DbContext + seed data
│   ├── Services/             # Business logic
│   └── Program.cs            # App startup
│
├── magicbricks-ui/           # Angular Frontend
│   └── src/app/
│       ├── components/       # Reusable UI components
│       ├── pages/            # Page components
│       ├── services/         # HTTP services
│       └── models/           # TypeScript interfaces
│
└── .gitignore
```

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/properties/search` | Search with filters + pagination |
| GET | `/api/properties/{id}` | Property detail |
| GET | `/api/properties/cities` | Available cities |
| POST | `/api/properties/favorites/{id}` | Toggle favorite |
| GET | `/api/properties/favorites` | Get favorites |
| POST | `/api/properties/contact` | Submit contact inquiry |

## Mock Properties

**Bangalore**: Koramangala, Whitefield, Jayanagar, Indiranagar, HSR Layout, Electronic City, Marathahalli, BTM Layout, Sarjapur Road, Hebbal

**Mumbai**: Andheri West, Powai, Worli, Malad West, Thane West, Goregaon East, Juhu, Kandivali East, Bandra West, Borivali West
