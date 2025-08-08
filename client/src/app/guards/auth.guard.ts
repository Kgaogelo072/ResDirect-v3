import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    
    if (this.authService.isAuthenticated()) {
      const currentUser = this.authService.currentUser();
      
      // Check if user is approved
      if (!currentUser?.isApproved) {
        this.router.navigate(['/pending-approval']);
        return false;
      }
      
      // Check role-based access
      const requiredRole = route.data['role'];
      if (requiredRole && currentUser?.role !== requiredRole) {
        this.router.navigate(['/unauthorized']);
        return false;
      }
      
      return true;
    }
    
    // Redirect to login if not authenticated
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}

@Injectable({
  providedIn: 'root'
})
export class AgentGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return false;
    }
    
    const currentUser = this.authService.currentUser();
    
    // Check if user is approved
    if (!currentUser?.isApproved) {
      this.router.navigate(['/pending-approval']);
      return false;
    }
    
    // Check if user is an agent
    if (currentUser?.role !== 'Agent') {
      this.router.navigate(['/unauthorized']);
      return false;
    }
    
    return true;
  }
} 