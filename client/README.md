# ResDirect Frontend

A comprehensive Angular application for property management featuring role-based dashboards, multiple image uploads, and modern UI components.

## 🌟 Features

### Authentication & User Management
- ✨ **Role-Based Registration** - Users select their role during signup (Agent, Tenant, Admin)
- 🔐 **Secure Authentication** - JWT-based login with automatic token refresh
- 👥 **User Approval System** - Admin approval workflow for agents
- 🛡️ **Route Guards** - Protected routes based on user roles and approval status

### Agent Dashboard
- 📊 **Dashboard Overview** - Property statistics, recent activity, and quick actions
- 🏠 **Property Management** - Complete CRUD operations for property listings
- 🖼️ **Multiple Image Upload** - Upload up to 5 images per property with drag-and-drop interface
- 🔍 **Advanced Search & Filter** - Real-time search and filtering by bedrooms/bathrooms
- 📋 **Viewing Requests** - Manage and respond to property viewing requests
- 📈 **Analytics Dashboard** - Performance metrics and insights (coming soon)

### Property Features
- ✅ **Multiple Images** - Support for up to 5 images per property
- ✅ **Primary Image Selection** - Designate which image appears first
- ✅ **Image Reordering** - Drag and drop to reorder images
- ✅ **Image Previews** - Real-time preview during upload
- ✅ **Responsive Gallery** - Beautiful image display on all devices
- ✅ **Grid/List Views** - Switch between display modes

### UI/UX
- ✨ **Zoneless Angular Application** - Built with Angular 18+ signals and modern architecture
- 🎨 **Beautiful UI** - Styled with Tailwind CSS and DaisyUI components
- 📱 **Fully Responsive Design** - Works perfectly on desktop, tablet, and mobile
- 🚀 **Modern Development** - TypeScript, reactive forms, and standalone components
- 🎯 **Intuitive Navigation** - Clean sidebar navigation with collapsible menu
- ⚡ **Fast Performance** - Optimized with Angular signals and lazy loading

## 🛠️ Tech Stack

- **Angular 18+** - Latest Angular with standalone components and signals
- **TypeScript** - Type-safe development with strict mode
- **Tailwind CSS** - Utility-first CSS framework
- **DaisyUI** - Beautiful component library with themes
- **RxJS** - Reactive programming with observables
- **Angular Signals** - Modern state management
- **Angular Router** - Client-side routing with guards
- **Reactive Forms** - Form handling with validation

## 📋 Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Angular CLI (`npm install -g @angular/cli`)

## 🚀 Installation

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Start the development server:**
   ```bash
   npm start
   # or
   ng serve
   ```

3. **Open your browser:**
   Navigate to `http://localhost:4200`

## 🏗️ Project Structure

```
src/
├── app/
│   ├── components/              # UI Components
│   │   ├── login/              # Authentication
│   │   ├── signup/             # User registration with role selection
│   │   ├── agent-dashboard/    # Main agent interface
│   │   ├── dashboard-overview/ # Statistics and overview
│   │   ├── property-list/      # Property listing with search/filter
│   │   ├── property-form/      # Property creation/editing
│   │   ├── viewing-requests/   # Viewing request management
│   │   └── analytics-dashboard/ # Analytics (coming soon)
│   ├── services/               # Business Logic
│   │   ├── auth.service.ts     # Authentication service
│   │   ├── property.service.ts # Property management
│   │   └── viewing-request.service.ts # Viewing requests
│   ├── models/                 # TypeScript Interfaces
│   │   ├── user.model.ts       # User and authentication models
│   │   ├── property.model.ts   # Property and image models
│   │   └── viewing-request.model.ts # Viewing request models
│   ├── guards/                 # Route Protection
│   │   └── auth.guard.ts       # Authentication and role guards
│   ├── app.routes.ts           # Routing configuration
│   └── app.component.ts        # Root component
├── assets/                     # Static Assets
├── styles.css                  # Global Styles
└── index.html                  # Main HTML Template
```

## 🎨 Available Scripts

- `npm start` - Start development server
- `npm run build` - Build for production
- `npm run build:prod` - Build for production with optimizations
- `npm run watch` - Build and watch for changes
- `npm run test` - Run unit tests
- `npm run lint` - Run ESLint
- `npm run serve:ssr` - Serve with server-side rendering

## 🔧 Configuration

### Environment Configuration
The application automatically connects to the backend API at `https://localhost:7063`. 

### Tailwind CSS
Tailwind is configured with DaisyUI themes. You can customize themes in `tailwind.config.js`.

### Angular Configuration
Key configurations in `angular.json`:
- **Zoneless**: Experimental zoneless change detection enabled
- **Standalone**: All components use standalone architecture
- **Strict Mode**: TypeScript strict mode enabled
- **Build Optimization**: Production builds are fully optimized

## 🎯 Key Features Explained

### Agent Dashboard
The agent dashboard is the main interface for property agents, featuring:
- **Sidebar Navigation** - Collapsible menu with role-based access
- **Dashboard Overview** - Key metrics and recent activity
- **Property Management** - Full CRUD operations with image support
- **Viewing Requests** - Manage tenant viewing requests
- **Responsive Design** - Works on all screen sizes

### Multiple Image Upload
Properties support multiple images with:
- **Drag & Drop** - Intuitive file selection
- **Image Previews** - Real-time preview before upload
- **Primary Selection** - Choose which image appears first
- **Reordering** - Drag and drop to reorder images
- **Progress Indicators** - Upload progress feedback

### Authentication System
Comprehensive authentication featuring:
- **Role Selection** - Users choose their role during registration
- **JWT Tokens** - Secure token-based authentication
- **Route Guards** - Protect routes based on authentication and roles
- **Auto Refresh** - Automatic token renewal
- **User Approval** - Admin approval workflow for agents

## 🐛 Troubleshooting

### Common Issues

1. **Development Server Won't Start**
   ```bash
   # Clear node_modules and reinstall
   rm -rf node_modules package-lock.json
   npm install
   npm start
   ```

2. **Build Errors**
   ```bash
   # Check TypeScript errors
   ng build --verbose
   ```

3. **API Connection Issues**
   - Ensure backend is running on `https://localhost:7063`
   - Check browser console for CORS errors
   - Verify API endpoints are responding

4. **Image Upload Issues**
   - Check backend Cloudinary configuration
   - Verify file size limits
   - Check browser console for upload errors

## 🔮 Upcoming Features

- 📊 **Advanced Analytics** - Detailed property performance metrics
- 🔔 **Real-time Notifications** - WebSocket integration for instant updates
- 🌙 **Dark Mode** - Theme switching capability
- 📱 **PWA Support** - Progressive Web App features
- 🗺️ **Map Integration** - Property location visualization
- 📧 **Email Templates** - Rich email notifications

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Follow Angular style guide and best practices
4. Ensure all components are responsive
5. Test on multiple screen sizes
6. Update documentation as needed
7. Submit a pull request

## 📄 License

This project is licensed under the MIT License. 