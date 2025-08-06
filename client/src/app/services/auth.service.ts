import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginRequest, LoginResponse, User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5179/api'; // Your backend API URL
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private tokenKey = 'auth_token';
  
  // Signals for reactive state management
  public isAuthenticated = signal(false);
  public currentUser = signal<User | null>(null);
  public isLoading = signal(false);

  constructor(private http: HttpClient) {
    this.checkAuthStatus();
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    this.isLoading.set(true);
    
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, credentials)
      .pipe(
        tap(response => {
          this.setAuthData(response);
          this.isLoading.set(false);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('user_data');
    this.currentUserSubject.next(null);
    this.isAuthenticated.set(false);
    this.currentUser.set(null);
  }

  private setAuthData(response: LoginResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem('user_data', JSON.stringify(response.user));
    this.currentUserSubject.next(response.user);
    this.isAuthenticated.set(true);
    this.currentUser.set(response.user);
  }

  private checkAuthStatus(): void {
    const token = localStorage.getItem(this.tokenKey);
    const userData = localStorage.getItem('user_data');
    
    if (token && userData) {
      try {
        const user = JSON.parse(userData);
        this.currentUserSubject.next(user);
        this.isAuthenticated.set(true);
        this.currentUser.set(user);
      } catch (error) {
        this.logout();
      }
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }
} 