import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ViewingRequest, ViewingRequestCreate, ViewingStatus } from '../models/viewing-request.model';

export interface GuestViewingRequestCreate {
  propertyId: number;
  guestName: string;
  guestEmail: string;
  guestPhone: string;
  viewingDate: string; // ISO date string
  preferredTime?: string;
  message?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ViewingRequestService {
  private apiUrl = 'http://localhost:5179/api/viewingrequests';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth_token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  // Get viewing requests for current agent
  getAgentViewingRequests(): Observable<ViewingRequest[]> {
    return this.http.get<ViewingRequest[]>(`${this.apiUrl}/by-agent`, {
      headers: this.getAuthHeaders()
    });
  }

  // Update viewing request status
  updateViewingRequestStatus(id: number, status: ViewingStatus): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status?status=${status}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  // Create viewing request (for tenants)
  createViewingRequest(request: ViewingRequestCreate): Observable<any> {
    return this.http.post(this.apiUrl, request, {
      headers: this.getAuthHeaders()
    });
  }

  // Create guest viewing request (no authentication required)
  createGuestViewingRequest(request: GuestViewingRequestCreate): Observable<any> {
    return this.http.post(`${this.apiUrl}/guest`, request);
  }

  // Get viewing request statistics
  getViewingRequestStats(): Observable<any> {
    return this.http.get(`${this.apiUrl}/stats`, {
      headers: this.getAuthHeaders()
    });
  }
} 