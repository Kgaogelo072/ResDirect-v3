import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Property, PropertyCreateRequest, PropertyUpdateRequest } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = 'http://localhost:5179/api/properties';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth_token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  // Get all properties
  getAllProperties(): Observable<Property[]> {
    return this.http.get<Property[]>(this.apiUrl);
  }

  // Get properties by current agent
  getAgentProperties(): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.apiUrl}/by-agent`, {
      headers: this.getAuthHeaders()
    });
  }

  // Get single property by ID
  getProperty(id: number): Observable<Property> {
    return this.http.get<Property>(`${this.apiUrl}/${id}`);
  }

  // Create new property with multiple images
  createProperty(propertyData: PropertyCreateRequest): Observable<Property> {
    const formData = new FormData();
    
    // Add property fields
    formData.append('title', propertyData.title);
    formData.append('description', propertyData.description);
    formData.append('rentalAmount', propertyData.rentalAmount.toString());
    formData.append('address', propertyData.address);
    formData.append('bedrooms', propertyData.bedrooms.toString());
    formData.append('bathrooms', propertyData.bathrooms.toString());
    formData.append('primaryImageIndex', propertyData.primaryImageIndex.toString());

    // Add images
    propertyData.images.forEach((image, index) => {
      formData.append('images', image);
    });

    // Add image orders
    propertyData.imageOrders.forEach((order, index) => {
      formData.append(`imageOrders[${index}]`, order.toString());
    });

    return this.http.post<Property>(this.apiUrl, formData, {
      headers: this.getAuthHeaders()
    });
  }

  // Update property
  updateProperty(id: number, propertyData: PropertyUpdateRequest): Observable<any> {
    const formData = new FormData();
    
    // Add property fields
    formData.append('title', propertyData.title);
    formData.append('description', propertyData.description);
    formData.append('rentalAmount', propertyData.rentalAmount.toString());
    formData.append('address', propertyData.address);
    formData.append('bedrooms', propertyData.bedrooms.toString());
    formData.append('bathrooms', propertyData.bathrooms.toString());

    // Add images if provided
    if (propertyData.images && propertyData.images.length > 0) {
      formData.append('primaryImageIndex', (propertyData.primaryImageIndex || 0).toString());
      
      propertyData.images.forEach((image, index) => {
        formData.append('images', image);
      });

      if (propertyData.imageOrders) {
        propertyData.imageOrders.forEach((order, index) => {
          formData.append(`imageOrders[${index}]`, order.toString());
        });
      }
    }

    return this.http.put(`${this.apiUrl}/${id}`, formData, {
      headers: this.getAuthHeaders()
    });
  }

  // Delete property
  deleteProperty(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  // Get property statistics for dashboard
  getPropertyStats(): Observable<any> {
    return this.http.get(`${this.apiUrl}/stats`, {
      headers: this.getAuthHeaders()
    });
  }
} 