import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { AgentDashboardComponent } from './components/agent-dashboard/agent-dashboard.component';
import { PublicPropertyListingComponent } from './components/public-property-listing/public-property-listing.component';
import { PropertyDetailComponent } from './components/property-detail/property-detail.component';
import { AgentGuard } from './guards/auth.guard';

export const routes: Routes = [
  // Public routes (no authentication required)
  { path: '', component: PublicPropertyListingComponent },
  { path: 'properties', component: PublicPropertyListingComponent },
  { path: 'property/:id', component: PropertyDetailComponent },
  
  // Authentication routes
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  
  // Protected routes
  { 
    path: 'agent-dashboard', 
    component: AgentDashboardComponent,
    canActivate: [AgentGuard],
    data: { role: 'Agent' }
  },
  
  // Status pages
  {
    path: 'pending-approval',
    component: LoginComponent, // Temporary - should be a dedicated component
    data: { message: 'Your account is pending approval. Please wait for admin approval.' }
  },
  {
    path: 'unauthorized',
    component: LoginComponent, // Temporary - should be a dedicated component
    data: { message: 'You do not have permission to access this page.' }
  },
  
  // Fallback
  { path: '**', redirectTo: '/properties' }
]; 