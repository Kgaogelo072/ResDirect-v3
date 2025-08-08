import { Component, ChangeDetectionStrategy, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PropertyService } from '../../services/property.service';
import { ViewingRequestService } from '../../services/viewing-request.service';
import { DashboardOverviewComponent } from '../dashboard-overview/dashboard-overview.component';
import { PropertyListComponent } from '../property-list/property-list.component';
import { PropertyFormComponent } from '../property-form/property-form.component';
import { ViewingRequestsComponent } from '../viewing-requests/viewing-requests.component';
import { AnalyticsDashboardComponent } from '../analytics-dashboard/analytics-dashboard.component';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-agent-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, DashboardOverviewComponent, PropertyListComponent, PropertyFormComponent, ViewingRequestsComponent, AnalyticsDashboardComponent],
  templateUrl: './agent-dashboard.component.html',
  styleUrl: './agent-dashboard.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AgentDashboardComponent implements OnInit {
  currentUser = this.authService.currentUser;
  activeTab = signal('overview');
  sidebarOpen = signal(true);
  
  // Property editing
  editingProperty = signal<Property | null>(null);

  // Dashboard stats
  stats = signal({
    totalProperties: 0,
    activeListings: 0,
    pendingRequests: 0,
    totalViews: 0,
    monthlyRevenue: 0
  });

  constructor(
    private authService: AuthService,
    private propertyService: PropertyService,
    private viewingRequestService: ViewingRequestService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadDashboardStats();
  }

  setActiveTab(tab: string) {
    this.activeTab.set(tab);
  }

  toggleSidebar() {
    this.sidebarOpen.set(!this.sidebarOpen());
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Property management methods
  onEditProperty(property: Property) {
    this.editingProperty.set(property);
    this.setActiveTab('edit-property');
  }

  onPropertyFormSuccess(property: Property) {
    this.editingProperty.set(null);
    this.setActiveTab('properties');
    this.loadDashboardStats(); // Refresh stats
  }

  onPropertyFormCancel() {
    this.editingProperty.set(null);
    this.setActiveTab('properties');
  }

  private loadDashboardStats() {
    // Load properties count
    this.propertyService.getAgentProperties().subscribe({
      next: (properties) => {
        this.stats.update(current => ({
          ...current,
          totalProperties: properties.length,
          activeListings: properties.length // Assuming all are active for now
        }));
      },
      error: (error) => console.error('Error loading properties:', error)
    });

    // Load viewing requests count
    this.viewingRequestService.getAgentViewingRequests().subscribe({
      next: (requests) => {
        const pendingRequests = requests.filter(r => r.status === 'Pending').length;
        this.stats.update(current => ({
          ...current,
          pendingRequests
        }));
      },
      error: (error) => console.error('Error loading viewing requests:', error)
    });
  }
} 