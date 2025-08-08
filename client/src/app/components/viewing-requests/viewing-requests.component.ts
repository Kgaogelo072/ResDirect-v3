import { Component, ChangeDetectionStrategy, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewingRequestService } from '../../services/viewing-request.service';
import { ViewingRequest, ViewingStatus } from '../../models/viewing-request.model';

@Component({
  selector: 'app-viewing-requests',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './viewing-requests.component.html',
  styleUrl: './viewing-requests.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ViewingRequestsComponent implements OnInit {
  requests = signal<ViewingRequest[]>([]);
  isLoading = signal(true);

  constructor(private viewingRequestService: ViewingRequestService) {}

  ngOnInit() {
    this.loadViewingRequests();
  }

  loadViewingRequests() {
    this.isLoading.set(true);
    this.viewingRequestService.getAgentViewingRequests().subscribe({
      next: (requests) => {
        this.requests.set(requests);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading viewing requests:', error);
        this.isLoading.set(false);
      }
    });
  }

  updateStatus(requestId: number, status: string) {
    this.viewingRequestService.updateViewingRequestStatus(requestId, status as ViewingStatus).subscribe({
      next: () => {
        this.loadViewingRequests(); // Reload the list
      },
      error: (error) => {
        console.error('Error updating request status:', error);
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

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-ZA', {
      weekday: 'short',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
} 