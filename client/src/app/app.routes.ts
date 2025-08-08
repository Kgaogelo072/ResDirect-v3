import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { AgentDashboardComponent } from './components/agent-dashboard/agent-dashboard.component';
import { AgentGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { 
    path: 'agent-dashboard', 
    component: AgentDashboardComponent,
    canActivate: [AgentGuard],
    data: { role: 'Agent' }
  },
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
  { path: '**', redirectTo: '/login' }
]; 