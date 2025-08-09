import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-public-property-listing',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './public-property-listing.component.html',
  styleUrl: './public-property-listing.component.css'
})
export class PublicPropertyListingComponent implements OnInit {
  // Signals for reactive state management
  properties = signal<Property[]>([]);
  isLoading = signal(true);
  searchTerm = signal('');
  selectedBedrooms = signal('');
  selectedBathrooms = signal('');
  minPrice = signal('');
  maxPrice = signal('');
  viewMode = signal<'grid' | 'list'>('grid');
  
  // Computed filtered properties
  filteredProperties = computed(() => {
    let filtered = this.properties();
    
    // Search by title or address
    const search = this.searchTerm().toLowerCase();
    if (search) {
      filtered = filtered.filter(p => 
        p.title.toLowerCase().includes(search) ||
        p.address.toLowerCase().includes(search)
      );
    }
    
    // Filter by bedrooms
    if (this.selectedBedrooms()) {
      const bedrooms = parseInt(this.selectedBedrooms());
      filtered = filtered.filter(p => p.bedrooms === bedrooms);
    }
    
    // Filter by bathrooms
    if (this.selectedBathrooms()) {
      const bathrooms = parseInt(this.selectedBathrooms());
      filtered = filtered.filter(p => p.bathrooms === bathrooms);
    }
    
    // Filter by price range
    if (this.minPrice()) {
      const min = parseFloat(this.minPrice());
      filtered = filtered.filter(p => p.rentalAmount >= min);
    }
    
    if (this.maxPrice()) {
      const max = parseFloat(this.maxPrice());
      filtered = filtered.filter(p => p.rentalAmount <= max);
    }
    
    return filtered;
  });

  constructor(private propertyService: PropertyService) {}

  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(): void {
    this.isLoading.set(true);
    // Use the public endpoint that doesn't require authentication
    this.propertyService.getAllProperties().subscribe({
      next: (properties) => {
        this.properties.set(properties);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading properties:', error);
        this.isLoading.set(false);
      }
    });
  }

  onSearchChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm.set(target.value);
  }

  onBedroomFilterChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedBedrooms.set(target.value);
  }

  onBathroomFilterChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedBathrooms.set(target.value);
  }

  onMinPriceChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.minPrice.set(target.value);
  }

  onMaxPriceChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.maxPrice.set(target.value);
  }

  toggleViewMode(): void {
    this.viewMode.set(this.viewMode() === 'grid' ? 'list' : 'grid');
  }

  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedBedrooms.set('');
    this.selectedBathrooms.set('');
    this.minPrice.set('');
    this.maxPrice.set('');
  }

  getPropertyImageUrl(property: Property): string {
    if (property.images && property.images.length > 0) {
      const primaryImage = property.images.find(img => img.isPrimary);
      return primaryImage?.imageUrl || property.images[0].imageUrl;
    }
    return '/assets/placeholder-property.jpg';
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-ZA', {
      style: 'currency',
      currency: 'ZAR',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0
    }).format(amount);
  }
} 