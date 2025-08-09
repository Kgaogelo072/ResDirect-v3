import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { ViewingRequestService } from '../../services/viewing-request.service';
import { Property } from '../../models/property.model';

interface GuestViewingRequest {
  propertyId: number;
  guestName: string;
  guestEmail: string;
  guestPhone: string;
  preferredDate: string;
  preferredTime: string;
  message?: string;
}

@Component({
  selector: 'app-property-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './property-detail.component.html',
  styleUrl: './property-detail.component.css'
})
export class PropertyDetailComponent implements OnInit {
  property = signal<Property | null>(null);
  isLoading = signal(true);
  currentImageIndex = signal(0);
  showViewingRequestForm = signal(false);
  isSubmittingRequest = signal(false);
  requestSubmitted = signal(false);

  viewingRequestForm: FormGroup = this.fb.group({
    guestName: ['', [Validators.required, Validators.minLength(2)]],
    guestEmail: ['', [Validators.required, Validators.email]],
    guestPhone: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]{10,15}$/)]],
    preferredDate: ['', Validators.required],
    preferredTime: ['', Validators.required],
    message: ['']
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private propertyService: PropertyService,
    private viewingRequestService: ViewingRequestService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = parseInt(params['id']);
      if (id) {
        this.loadProperty(id);
      }
    });
  }

  loadProperty(id: number): void {
    this.isLoading.set(true);
    this.propertyService.getProperty(id).subscribe({
      next: (property) => {
        this.property.set(property);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading property:', error);
        this.isLoading.set(false);
        // Redirect to properties list if property not found
        this.router.navigate(['/properties']);
      }
    });
  }

  getPropertyImages(): string[] {
    const prop = this.property();
    if (!prop?.images || prop.images.length === 0) {
      return ['/assets/placeholder-property.jpg'];
    }
    return prop.images
      .sort((a, b) => {
        if (a.isPrimary) return -1;
        if (b.isPrimary) return 1;
        return a.displayOrder - b.displayOrder;
      })
      .map(img => img.imageUrl);
  }

  getCurrentImage(): string {
    const images = this.getPropertyImages();
    return images[this.currentImageIndex()] || images[0];
  }

  nextImage(): void {
    const images = this.getPropertyImages();
    const nextIndex = (this.currentImageIndex() + 1) % images.length;
    this.currentImageIndex.set(nextIndex);
  }

  previousImage(): void {
    const images = this.getPropertyImages();
    const prevIndex = this.currentImageIndex() === 0
      ? images.length - 1
      : this.currentImageIndex() - 1;
    this.currentImageIndex.set(prevIndex);
  }

  selectImage(index: number): void {
    this.currentImageIndex.set(index);
  }

  toggleViewingRequestForm(): void {
    this.showViewingRequestForm.set(!this.showViewingRequestForm());
    if (this.showViewingRequestForm()) {
      // Set minimum date to tomorrow
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      const minDate = tomorrow.toISOString().split('T')[0];
      
      const dateControl = this.viewingRequestForm.get('preferredDate');
      if (dateControl) {
        dateControl.setValue('');
        // Set min attribute on the input (will be handled in template)
      }
    }
  }

  onSubmitViewingRequest(): void {
    if (this.viewingRequestForm.valid && this.property()) {
      this.isSubmittingRequest.set(true);
      
      const formData = this.viewingRequestForm.value;
      const request = {
        propertyId: this.property()!.id,
        guestName: formData.guestName,
        guestEmail: formData.guestEmail,
        guestPhone: formData.guestPhone,
        viewingDate: formData.preferredDate + 'T' + (formData.preferredTime || '10:00'),
        preferredTime: formData.preferredTime,
        message: formData.message || ''
      };

      this.viewingRequestService.createGuestViewingRequest(request).subscribe({
        next: (response) => {
          this.isSubmittingRequest.set(false);
          this.requestSubmitted.set(true);
          this.showViewingRequestForm.set(false);
          this.viewingRequestForm.reset();
        },
        error: (error) => {
          this.isSubmittingRequest.set(false);
          console.error('Error submitting viewing request:', error);
          // Show error message (you could add an error signal here)
          alert('Failed to submit viewing request. Please try again.');
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.viewingRequestForm.controls).forEach(key => {
        this.viewingRequestForm.get(key)?.markAsTouched();
      });
    }
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-ZA', {
      style: 'currency',
      currency: 'ZAR',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  }

  goBack(): void {
    this.router.navigate(['/properties']);
  }

  shareProperty(): void {
    if (navigator.share) {
      navigator.share({
        title: this.property()?.title,
        text: `Check out this property: ${this.property()?.title}`,
        url: window.location.href
      });
    } else {
      // Fallback: copy URL to clipboard
      navigator.clipboard.writeText(window.location.href).then(() => {
        // Show success message (could use a toast service)
        alert('Property link copied to clipboard!');
      });
    }
  }

  getMinDate(): string {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    return tomorrow.toISOString().split('T')[0];
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.viewingRequestForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.viewingRequestForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${fieldName} is required`;
      if (field.errors['email']) return 'Please enter a valid email address';
      if (field.errors['minlength']) return `${fieldName} is too short`;
      if (field.errors['pattern']) return 'Please enter a valid phone number';
    }
    return '';
  }
} 