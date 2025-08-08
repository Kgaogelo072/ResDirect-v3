import { Component, ChangeDetectionStrategy, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PropertyService } from '../../services/property.service';
import { ViewingRequestService } from '../../services/viewing-request.service';
import { Property } from '../../models/property.model';
import { ViewingRequest } from '../../models/viewing-request.model';

@Component({
  selector: 'app-dashboard-overview',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard-overview.component.html',
  styleUrl: './dashboard-overview.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardOverviewComponent implements OnInit {
  @Input() stats: any;

  recentProperties = signal<Property[]>([]);
  recentRequests = signal<ViewingRequest[]>([]);
  isLoading = signal(true);

  constructor(
    private propertyService: PropertyService,
    private viewingRequestService: ViewingRequestService
  ) {}

  ngOnInit() {
    this.loadRecentData();
  }

  private loadRecentData() {
    this.isLoading.set(true);

    // Load recent properties
    this.propertyService.getAgentProperties().subscribe({
      next: (properties) => {
        // Sort by newest first and take top 5
        const recent = properties
          .sort((a, b) => b.id - a.id)
          .slice(0, 5);
        this.recentProperties.set(recent);
      },
      error: (error) => console.error('Error loading recent properties:', error)
    });

    // Load recent viewing requests
    this.viewingRequestService.getAgentViewingRequests().subscribe({
      next: (requests) => {
        // Sort by newest first and take top 5
        const recent = requests
          .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
          .slice(0, 5);
        this.recentRequests.set(recent);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading recent viewing requests:', error);
        this.isLoading.set(false);
      }
    });
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending': return 'bg-yellow-100 text-yellow-800';
      case 'approved': return 'bg-green-100 text-green-800';
      case 'rejected': return 'bg-red-100 text-red-800';
      case 'completed': return 'bg-blue-100 text-blue-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-ZA', {
      style: 'currency',
      currency: 'ZAR'
    }).format(amount);
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-ZA', {
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
} 