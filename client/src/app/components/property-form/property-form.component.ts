import { Component, ChangeDetectionStrategy, Input, Output, EventEmitter, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PropertyService } from '../../services/property.service';
import { Property, PropertyCreateRequest, PropertyUpdateRequest } from '../../models/property.model';

@Component({
  selector: 'app-property-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './property-form.component.html',
  styleUrl: './property-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PropertyFormComponent implements OnInit {
  @Input() property: Property | null = null; // For editing
  @Output() onSuccess = new EventEmitter<Property>();
  @Output() onCancel = new EventEmitter<void>();

  propertyForm: FormGroup;
  isSubmitting = signal(false);
  errorMessage = signal('');
  
  // Image handling
  selectedImages = signal<File[]>([]);
  imagePreviews = signal<string[]>([]);
  primaryImageIndex = signal(0);
  dragOver = signal(false);
  
  // Constants
  readonly maxImages = 5;
  readonly maxFileSize = 5 * 1024 * 1024; // 5MB
  readonly allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];

  constructor(
    private fb: FormBuilder,
    private propertyService: PropertyService
  ) {
    this.propertyForm = this.createForm();
  }

  ngOnInit() {
    if (this.property) {
      this.populateForm();
    }
  }

  private createForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]],
      address: ['', [Validators.required, Validators.minLength(5)]],
      rentalAmount: ['', [Validators.required, Validators.min(1), Validators.max(100000)]],
      bedrooms: ['', [Validators.required, Validators.min(0), Validators.max(10)]],
      bathrooms: ['', [Validators.required, Validators.min(1), Validators.max(10)]]
    });
  }

  private populateForm() {
    if (this.property) {
      this.propertyForm.patchValue({
        title: this.property.title,
        description: this.property.description,
        address: this.property.address,
        rentalAmount: this.property.rentalAmount,
        bedrooms: this.property.bedrooms,
        bathrooms: this.property.bathrooms
      });
    }
  }

  // Image handling methods
  onFileSelect(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.handleFiles(Array.from(input.files));
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.dragOver.set(true);
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.dragOver.set(false);
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.dragOver.set(false);
    
    if (event.dataTransfer?.files) {
      this.handleFiles(Array.from(event.dataTransfer.files));
    }
  }

  private handleFiles(files: File[]) {
    const currentImages = this.selectedImages();
    const validFiles: File[] = [];
    let errorMessages: string[] = [];

    for (const file of files) {
      // Check if we've reached max images
      if (currentImages.length + validFiles.length >= this.maxImages) {
        errorMessages.push(`Maximum ${this.maxImages} images allowed`);
        break;
      }

      // Validate file type
      if (!this.allowedTypes.includes(file.type)) {
        errorMessages.push(`${file.name}: Only JPEG, PNG, and WebP images are allowed`);
        continue;
      }

      // Validate file size
      if (file.size > this.maxFileSize) {
        errorMessages.push(`${file.name}: File size must be less than 5MB`);
        continue;
      }

      validFiles.push(file);
    }

    if (errorMessages.length > 0) {
      this.errorMessage.set(errorMessages.join('. '));
      setTimeout(() => this.errorMessage.set(''), 5000);
    }

    if (validFiles.length > 0) {
      const newImages = [...currentImages, ...validFiles];
      this.selectedImages.set(newImages);
      this.generatePreviews(newImages);
    }
  }

  private generatePreviews(files: File[]) {
    const previews: string[] = [];
    let loadedCount = 0;

    files.forEach((file, index) => {
      const reader = new FileReader();
      reader.onload = (e) => {
        previews[index] = e.target?.result as string;
        loadedCount++;
        
        if (loadedCount === files.length) {
          this.imagePreviews.set(previews);
        }
      };
      reader.readAsDataURL(file);
    });
  }

  removeImage(index: number) {
    const images = this.selectedImages();
    const previews = this.imagePreviews();
    
    images.splice(index, 1);
    previews.splice(index, 1);
    
    this.selectedImages.set([...images]);
    this.imagePreviews.set([...previews]);
    
    // Adjust primary image index if necessary
    const currentPrimary = this.primaryImageIndex();
    if (index === currentPrimary && images.length > 0) {
      this.primaryImageIndex.set(0);
    } else if (index < currentPrimary) {
      this.primaryImageIndex.set(currentPrimary - 1);
    } else if (currentPrimary >= images.length) {
      this.primaryImageIndex.set(Math.max(0, images.length - 1));
    }
  }

  setPrimaryImage(index: number) {
    this.primaryImageIndex.set(index);
  }

  moveImage(fromIndex: number, toIndex: number) {
    const images = this.selectedImages();
    const previews = this.imagePreviews();
    
    // Move in both arrays
    const movedImage = images.splice(fromIndex, 1)[0];
    const movedPreview = previews.splice(fromIndex, 1)[0];
    
    images.splice(toIndex, 0, movedImage);
    previews.splice(toIndex, 0, movedPreview);
    
    this.selectedImages.set([...images]);
    this.imagePreviews.set([...previews]);
    
    // Adjust primary image index
    const currentPrimary = this.primaryImageIndex();
    if (fromIndex === currentPrimary) {
      this.primaryImageIndex.set(toIndex);
    } else if (fromIndex < currentPrimary && toIndex >= currentPrimary) {
      this.primaryImageIndex.set(currentPrimary - 1);
    } else if (fromIndex > currentPrimary && toIndex <= currentPrimary) {
      this.primaryImageIndex.set(currentPrimary + 1);
    }
  }

  // Form submission
  onSubmit() {
    if (this.propertyForm.valid) {
      // For new properties, images are required
      if (!this.property && this.selectedImages().length === 0) {
        this.errorMessage.set('At least one image is required');
        return;
      }

      this.isSubmitting.set(true);
      this.errorMessage.set('');

      const formData = this.propertyForm.value;
      
      if (this.property) {
        // Update existing property
        this.updateProperty(formData);
      } else {
        // Create new property
        this.createProperty(formData);
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  private createProperty(formData: any) {
    const images = this.selectedImages();
    const imageOrders = images.map((_, index) => index + 1);

    const propertyData: PropertyCreateRequest = {
      ...formData,
      images,
      imageOrders,
      primaryImageIndex: this.primaryImageIndex()
    };

    this.propertyService.createProperty(propertyData).subscribe({
      next: (property) => {
        this.isSubmitting.set(false);
        this.onSuccess.emit(property);
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(error.error?.message || 'Failed to create property. Please try again.');
      }
    });
  }

  private updateProperty(formData: any) {
    if (!this.property) return;

    const propertyData: PropertyUpdateRequest = {
      ...formData
    };

    // Only include images if new ones were selected
    if (this.selectedImages().length > 0) {
      const images = this.selectedImages();
      const imageOrders = images.map((_, index) => index + 1);
      
      propertyData.images = images;
      propertyData.imageOrders = imageOrders;
      propertyData.primaryImageIndex = this.primaryImageIndex();
    }

    this.propertyService.updateProperty(this.property.id, propertyData).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        // For updates, we don't get the updated property back, so we emit the original
        this.onSuccess.emit(this.property!);
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(error.error?.message || 'Failed to update property. Please try again.');
      }
    });
  }

  cancel() {
    this.onCancel.emit();
  }

  // Utility methods
  private markFormGroupTouched() {
    Object.keys(this.propertyForm.controls).forEach(key => {
      const control = this.propertyForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.propertyForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['maxlength']) {
        return `${this.getFieldDisplayName(fieldName)} must be no more than ${field.errors['maxlength'].requiredLength} characters`;
      }
      if (field.errors['min']) {
        return `${this.getFieldDisplayName(fieldName)} must be at least ${field.errors['min'].min}`;
      }
      if (field.errors['max']) {
        return `${this.getFieldDisplayName(fieldName)} must be no more than ${field.errors['max'].max}`;
      }
    }
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      title: 'Title',
      description: 'Description',
      address: 'Address',
      rentalAmount: 'Rental amount',
      bedrooms: 'Bedrooms',
      bathrooms: 'Bathrooms'
    };
    return displayNames[fieldName] || fieldName;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.propertyForm.get(fieldName);
    return !!(field?.invalid && field.touched);
  }

  get isEditMode(): boolean {
    return !!this.property;
  }

  get formTitle(): string {
    return this.isEditMode ? 'Edit Property' : 'Add New Property';
  }
} 