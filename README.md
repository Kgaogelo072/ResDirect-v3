# ResDirect - Property Management System

A comprehensive full-stack property management application with Angular frontend and ASP.NET Core backend, featuring role-based dashboards, multiple image uploads, and real-time property management.

## 🚀 Recent Updates

- ✅ **Agent Dashboard** - Complete property management interface
- ✅ **Multiple Image Support** - Upload up to 5 images per property
- ✅ **Database Schema Fixed** - Clean migrations with PropertyImages table
- ✅ **Role-Based Authentication** - Agent, Tenant, and Admin roles
- ✅ **Image Upload Integration** - Cloudinary storage with proper error handling
- ✅ **Responsive UI** - Modern design with Tailwind CSS and DaisyUI

## Project Structure

```
ResDirectThirdVersion/
├── client/                     # Angular Frontend Application
│   ├── src/                   # Source code
│   │   ├── app/              # Angular application
│   │   │   ├── components/   # UI components
│   │   │   │   ├── login/           # User authentication
│   │   │   │   ├── signup/          # User registration with role selection
│   │   │   │   ├── agent-dashboard/ # Complete agent management interface
│   │   │   │   ├── property-list/   # Property listing with search/filter
│   │   │   │   ├── property-form/   # Property creation/editing with images
│   │   │   │   ├── viewing-requests/ # Viewing request management
│   │   │   │   └── dashboard-overview/ # Statistics and overview
│   │   │   ├── services/     # Business logic services
│   │   │   │   ├── auth.service.ts      # Authentication with JWT
│   │   │   │   ├── property.service.ts  # Property CRUD operations
│   │   │   │   └── viewing-request.service.ts # Viewing requests
│   │   │   ├── models/       # TypeScript interfaces
│   │   │   ├── guards/       # Route protection (AuthGuard, AgentGuard)
│   │   │   └── app.routes.ts # Routing configuration
│   │   ├── styles.css        # Global styles
│   │   ├── index.html        # Main HTML file
│   │   └── main.ts           # Application bootstrap
│   ├── package.json          # Frontend dependencies
│   ├── angular.json          # Angular configuration
│   ├── tailwind.config.js    # Tailwind CSS configuration
│   └── README.md             # Frontend documentation
├── PropertyListingAPI/        # ASP.NET Core Backend API
│   ├── Controllers/          # API controllers
│   │   ├── AuthController.cs        # Authentication endpoints
│   │   ├── PropertiesController.cs  # Property management with images
│   │   ├── ViewingRequestsController.cs # Viewing request handling
│   │   ├── AdminController.cs       # Admin operations
│   │   ├── UploadController.cs      # Image upload endpoints
│   │   └── TestController.cs        # Testing utilities
│   ├── Models/               # Data models
│   │   ├── User.cs                  # User entity with roles
│   │   ├── Property.cs              # Property entity (no single image fields)
│   │   ├── PropertyImage.cs         # Multiple images per property
│   │   └── ViewingRequest.cs        # Viewing request entity
│   ├── DTOs/                 # Data Transfer Objects
│   │   ├── PropertyCreateDto.cs     # Property creation with multiple images
│   │   ├── PropertyImageDto.cs      # Image data transfer
│   │   └── UserRegisterDto.cs       # Registration with first/last name
│   ├── Services/             # Business logic
│   │   ├── PhotoService.cs          # Cloudinary image upload (up to 5 images)
│   │   ├── TokenService.cs          # JWT token management
│   │   ├── EmailService.cs          # Email notifications
│   │   └── WhatsAppService.cs       # WhatsApp notifications
│   ├── Data/                 # Database context
│   │   └── ApplicationDbContext.cs  # EF Core context with PropertyImages
│   ├── Migrations/           # Database migrations
│   │   └── InitialCreateWithMultipleImages.cs # Clean schema migration
│   ├── Interfaces/           # Service interfaces
│   ├── Mappings/             # AutoMapper profiles
│   ├── appsettings.json      # Configuration (Cloudinary, JWT, DB)
│   └── Program.cs            # API startup with all services registered
└── ResDirectThirdVersion.sln  # Visual Studio solution
```

## Getting Started

### Prerequisites
- **Frontend**: Node.js (v18+), npm, Angular CLI
- **Backend**: .NET 9.0 SDK, SQL Server
- **Image Storage**: Cloudinary account (for property images)

### Quick Start

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd ResDirectThirdVersion
   ```

2. **Configure the backend:**
   ```bash
   cd PropertyListingAPI
   # Update appsettings.json with your:
   # - SQL Server connection string
   # - Cloudinary credentials (CloudName, ApiKey, ApiSecret)
   # - JWT secret key
   ```

3. **Set up the database:**
   ```bash
   dotnet ef database update
   # This creates the database with the correct schema including PropertyImages table
   ```

4. **Start the backend API:**
   ```bash
   dotnet restore
   dotnet run
   ```
   The API will be available at `https://localhost:7063`

5. **Start the frontend application:**
   ```bash
   cd client
   npm install
   npm start
   ```
   The frontend will be available at `http://localhost:4200`

## 🌟 Features

### Authentication & Authorization
- ✅ **Role-Based Registration** - Users select their role during signup (Agent, Tenant, Admin)
- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **Route Guards** - Protected routes based on user roles and approval status
- ✅ **User Approval System** - Admin approval required for agents

### Agent Dashboard
- 📊 **Dashboard Overview** - Property statistics and recent activity
- 🏠 **Property Management** - Complete CRUD operations for properties
- 🖼️ **Multiple Image Upload** - Upload up to 5 images per property with drag-and-drop
- 🔍 **Advanced Search & Filter** - Search properties by title, filter by bedrooms/bathrooms
- 📋 **Viewing Requests** - Manage property viewing requests from tenants
- 📈 **Analytics** - Property performance metrics (coming soon)

### Property Management
- ✅ **Multiple Images** - Each property can have up to 5 images
- ✅ **Primary Image Selection** - Designate which image appears first
- ✅ **Image Reordering** - Drag and drop to reorder images
- ✅ **Cloudinary Integration** - Reliable cloud storage for images
- ✅ **Responsive Grid/List Views** - Switch between display modes
- ✅ **Pagination** - Efficient handling of large property lists

### Frontend (Angular)
- ✨ **Modern Angular 18+** - Signals and standalone components
- 🎨 **Beautiful UI** - Tailwind CSS and DaisyUI components
- 🔐 **Secure Authentication** - JWT token management with auto-refresh
- 📱 **Fully Responsive** - Mobile-first design approach
- 🚀 **Zoneless Architecture** - Better performance with Angular signals
- 🎯 **Type Safety** - Full TypeScript integration

### Backend (ASP.NET Core)
- 🔒 **JWT Authentication** - Secure API endpoints
- 📊 **Entity Framework Core** - Clean database architecture
- 🖼️ **Image Upload** - Cloudinary integration with error handling
- 📧 **Notification Services** - Email and WhatsApp (placeholder implementations)
- 🏠 **Property API** - Complete CRUD with multiple image support
- 👥 **User Management** - Role-based access control
- 📝 **Comprehensive Logging** - Detailed logs for debugging and monitoring

## 🛠️ Development

### Database Schema
The application uses a clean, normalized database schema:

```sql
Users
├── Id (Primary Key)
├── FullName, Email (Unique), PhoneNumber
├── PasswordHash, PasswordSalt
├── Role (Agent/Tenant/Admin)
└── IsApproved (Boolean)

Properties
├── Id (Primary Key)
├── Title, Description, Address
├── RentalAmount, Bedrooms, Bathrooms
├── AgentId (Foreign Key to Users)
└── UserId (Optional Foreign Key)

PropertyImages
├── Id (Primary Key)
├── PropertyId (Foreign Key to Properties, Cascade Delete)
├── ImageUrl, ImagePublicId (Cloudinary)
├── DisplayOrder, IsPrimary
└── CreatedAt

ViewingRequests
├── Id (Primary Key)
├── PropertyId, TenantId (Foreign Keys)
├── ViewingDate
└── Status (Pending/Approved/Rejected/Completed)
```

### Frontend Development
```bash
cd client
npm start            # Start development server
npm run build        # Build for production
npm run lint         # Run linter
```

### Backend Development
```bash
cd PropertyListingAPI
dotnet run           # Start API server
dotnet ef database update    # Apply migrations
dotnet ef migrations add <name>  # Create new migration
dotnet build         # Build project
```

### Testing Image Upload
Use the test endpoint to verify Cloudinary integration:
```bash
POST /api/test/upload-test
Content-Type: multipart/form-data
# Upload a single image file
```

## 📡 API Endpoints

### Authentication
- `POST /api/auth/register` - User registration with role selection
- `POST /api/auth/login` - User authentication (returns user data + token)

### Properties
- `GET /api/properties` - Get all properties (with images)
- `GET /api/properties/{id}` - Get property by ID
- `GET /api/properties/by-agent` - Get properties for authenticated agent
- `POST /api/properties` - Create property with multiple images
- `PUT /api/properties/{id}` - Update property
- `DELETE /api/properties/{id}` - Delete property (cascades to images)

### Viewing Requests
- `GET /api/viewing-requests` - Get viewing requests for agent
- `POST /api/viewing-requests` - Create viewing request
- `PUT /api/viewing-requests/{id}/status` - Update request status

### Image Management
- `POST /api/upload` - Upload property images
- `POST /api/test/upload-test` - Test single image upload

### Statistics
- `GET /api/properties/stats` - Property statistics for agent
- `GET /api/viewing-requests/stats` - Viewing request statistics

## 🔧 Configuration

### Backend Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PropertyListingDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
  },
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key", 
    "ApiSecret": "your-api-secret"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "https://resdirect.com"
  }
}
```

### Frontend Environment
The frontend automatically connects to `https://localhost:7063` for the API.

## 🚀 Technology Stack

### Frontend
- **Angular 18+** - Modern framework with signals
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first styling
- **DaisyUI** - Component library
- **RxJS** - Reactive programming
- **Angular Signals** - State management

### Backend  
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM with migrations
- **SQL Server** - Relational database
- **JWT** - Authentication and authorization
- **Cloudinary** - Cloud image storage
- **AutoMapper** - Object-to-object mapping
- **Dependency Injection** - Service container

### Database
- **SQL Server** - Primary database
- **Entity Framework Migrations** - Schema management
- **Foreign Key Constraints** - Data integrity
- **Unique Indexes** - Performance optimization

## 🔍 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   ```bash
   # Update connection string in appsettings.json
   # Ensure SQL Server is running
   dotnet ef database update
   ```

2. **Image Upload Failures**
   ```bash
   # Verify Cloudinary credentials in appsettings.json
   # Test with /api/test/upload-test endpoint
   # Check logs for detailed error messages
   ```

3. **Frontend Build Errors**
   ```bash
   # Clear node_modules and reinstall
   rm -rf node_modules package-lock.json
   npm install
   ```

4. **Migration Issues**
   ```bash
   # If migrations are corrupted, reset database:
   dotnet ef database drop
   dotnet ef database update
   ```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test both frontend and backend thoroughly
5. Ensure database migrations work correctly
6. Update documentation if needed
7. Commit your changes (`git commit -m 'Add amazing feature'`)
8. Push to the branch (`git push origin feature/amazing-feature`)
9. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Angular team for the amazing framework
- ASP.NET Core team for the robust backend framework
- Tailwind CSS and DaisyUI for the beautiful UI components
- Cloudinary for reliable image storage
- Entity Framework team for excellent ORM capabilities 