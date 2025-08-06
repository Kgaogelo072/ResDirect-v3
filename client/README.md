# ResDirect Frontend

A modern Angular application for property management with a beautiful login interface.

## Features

- ✨ **Zoneless Angular Application** - Built with Angular 18+ signals and modern architecture
- 🎨 **Beautiful UI** - Styled with Tailwind CSS and DaisyUI components
- 🔐 **Authentication** - Complete login functionality with form validation
- 📱 **Responsive Design** - Works perfectly on desktop, tablet, and mobile
- 🚀 **Modern Development** - TypeScript, reactive forms, and standalone components

## Tech Stack

- **Angular 18+** - Latest Angular with standalone components
- **TypeScript** - Type-safe development
- **Tailwind CSS** - Utility-first CSS framework
- **DaisyUI** - Beautiful component library
- **RxJS** - Reactive programming with observables
- **Angular Signals** - Modern state management

## Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Angular CLI

## Installation

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Install Angular CLI globally (if not already installed):**
   ```bash
   npm install -g @angular/cli
   ```

## Development

1. **Start the development server:**
   ```bash
   npm run dev
   # or
   ng serve
   ```

2. **Open your browser and navigate to:**
   ```
   http://localhost:4200
   ```

## Backend Integration

The application is configured to connect to your ASP.NET Core API at `https://localhost:7063/api`.

Make sure your backend API is running and has the following endpoints:
- `POST /api/auth/login` - For user authentication

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   └── login/
│   │       ├── login.component.ts
│   │       ├── login.component.html
│   │       └── login.component.css
│   ├── models/
│   │   └── user.model.ts
│   ├── services/
│   │   └── auth.service.ts
│   ├── app.component.ts
│   └── app.routes.ts
├── styles.css
├── index.html
└── main.ts
```

## Features

### Login Page
- Email and password validation
- Password visibility toggle
- Remember me functionality
- Error handling and display
- Loading states
- Social login buttons (UI only)
- Responsive design with welcome section

### Authentication Service
- JWT token management
- Local storage for persistence
- Reactive state management with signals
- HTTP client integration

## Customization

### Themes
The application supports multiple DaisyUI themes. You can change the theme by modifying the `data-theme` attribute in `src/index.html`:

```html
<html lang="en" data-theme="dark">
```

Available themes: light, dark, cupcake, bumblebee, emerald, corporate, synthwave, retro, cyberpunk, valentine, halloween, garden, forest, aqua, lofi, pastel, fantasy, wireframe, black, luxury, dracula, cmyk, autumn, business, acid, lemonade, night, coffee, winter

### Styling
- Global styles: `src/styles.css`
- Component styles: Individual `.css` files
- Tailwind configuration: `tailwind.config.js`

## Build

```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. 