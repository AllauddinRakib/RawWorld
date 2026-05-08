# Raw World — Mobile E-Commerce Application

**Student:** Md Allauddin Rakib  
**P-Number:** P2837503  
**Programme:** Computer Science  
**Institution:** De Montfort University / Niels Brock  
**Supervisor:** Eugene Louis Batie Badzongoly  

## Project Structure

```
Allauddin-P2837503/
├── RawWorld.API/          → ASP.NET Core Web API (Backend)
├── RawWorld.App/          → .NET MAUI Android App (Frontend)
├── DatabaseScripts/       → MySQL database schema and seed data
├── RawWorld.slnx          → Visual Studio Solution file
├── .gitignore             → Git ignore rules
└── README.md              → This file
```

## Tech Stack

- **Frontend:** .NET MAUI (Android) with MVVM pattern
- **Backend:** ASP.NET Core Web API with Service Layer pattern
- **Database:** MySQL 8.0 via Entity Framework Core 9.0 + Pomelo provider
- **Authentication:** JWT Bearer tokens + BCrypt password hashing
- **IDE:** Visual Studio 2022
- **Target Framework:** .NET 10 (EF Core packages pinned to 9.0.0)

## How to Run

### Prerequisites
- Visual Studio 2022 with .NET MAUI workload
- .NET 10 SDK
- MySQL Server 8.0 (legacy authentication mode)
- Android Emulator (Pixel 5 API 36 recommended)

### Backend (RawWorld.API)
1. Update the connection string in `appsettings.json`
2. Run: `dotnet ef database update`
3. Run: `dotnet run`
4. Swagger UI available at: `https://localhost:7055/swagger`

### Frontend (RawWorld.App)
1. Ensure the API is running
2. Set Android emulator as target device
3. Run from Visual Studio or: `dotnet build -t:Run -f net10.0-android`

### Database
- Schema is auto-created via EF Core migrations on API startup
- Raw SQL script available in `DatabaseScripts/RawWorld_Schema.sql`
- Default admin: `admin@rawworld.com` / `Admin@123`

## GitHub Repository
https://github.com/AllauddinRakib/RawWorld
