# ResDirect - Property Management System

A full-stack property management application with Angular frontend and ASP.NET Core backend.

## Project Structure

```
ResDirectThirdVersion/
├── client/                     # Angular Frontend Application
│   ├── src/                   # Source code
│   │   ├── app/              # Angular application
│   │   │   ├── components/   # UI components
│   │   │   ├── services/     # Business logic services
│   │   │   └── models/       # TypeScript interfaces
│   │   ├── styles.css        # Global styles
│   │   ├── index.html        # Main HTML file
│   │   └── main.ts           # Application bootstrap
│   ├── package.json          # Frontend dependencies
│   ├── angular.json          # Angular configuration
│   ├── tailwind.config.js    # Tailwind CSS configuration
│   └── README.md             # Frontend documentation
├── PropertyListingAPI/        # ASP.NET Core Backend API
│   ├── Controllers/          # API controllers
│   ├── Models/               # Data models
│   ├── Services/             # Business logic
│   ├── Data/                 # Database context
│   └── Program.cs            # API startup
└── ResDirectThirdVersion.sln  # Visual Studio solution
```

## Getting Started

### Prerequisites
- **Frontend**: Node.js (v18+), npm, Angular CLI
- **Backend**: .NET 9.0 SDK, SQL Server

### Quick Start

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd ResDirectThirdVersion
   ```

2. **Start the backend API:**
   ```bash
   cd PropertyListingAPI
   dotnet restore
   dotnet run
   ```
   The API will be available at `https://localhost:7063`

3. **Start the frontend application:**
   ```bash
   cd client
   npm install
   npm run dev
   ```
   The frontend will be available at `http://localhost:4200`

## Features

### Frontend (Angular)
- ✨ Modern Angular 18+ with signals and standalone components
- 🎨 Beautiful UI with Tailwind CSS and DaisyUI
- 🔐 Authentication with JWT token management
- 📱 Fully responsive design
- 🚀 Zoneless architecture for better performance

### Backend (ASP.NET Core)
- 🔒 JWT Authentication and Authorization
- 📊 Entity Framework Core with SQL Server
- 🖼️ Image upload with Cloudinary integration
- 📧 Email and WhatsApp notifications
- 🏠 Property management CRUD operations
- 👥 User management with role-based access

## Development

### Frontend Development
```bash
cd client
npm run dev          # Start development server
npm run build        # Build for production
```

### Backend Development
```bash
cd PropertyListingAPI
dotnet run           # Start API server
dotnet ef database update  # Update database
```

## API Endpoints

The backend API provides the following main endpoints:
- `POST /api/auth/login` - User authentication
- `GET /api/properties` - Get all properties
- `POST /api/properties` - Create new property
- `GET /api/viewing-requests` - Get viewing requests
- `POST /api/upload` - Upload property images

## Technology Stack

### Frontend
- **Angular 18+** - Modern framework with signals
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first styling
- **DaisyUI** - Component library
- **RxJS** - Reactive programming

### Backend
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **JWT** - Authentication
- **Cloudinary** - Image storage
- **AutoMapper** - Object mapping

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test both frontend and backend
5. Submit a pull request

## License

This project is licensed under the MIT License. 