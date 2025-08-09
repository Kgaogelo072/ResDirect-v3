import { Component, OnInit, signal, computed, HostListener } from '@angular/core';
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
  
  // Mobile-specific signals
  showMobileFilters = signal(false);
  isMobileView = signal(false);
  showBackToTop = signal(false);
  
  // Placeholder image data URI
  private readonly placeholderImage = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNDAwIiBoZWlnaHQ9IjI1MCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KICA8cmVjdCB3aWR0aD0iNDAwIiBoZWlnaHQ9IjI1MCIgZmlsbD0iI2Y0ZjRmNCIvPgogIDxnIGZpbGw9IiM5Y2EzYWYiPgogICAgPHJlY3QgeD0iMTUwIiB5PSI4MCIgd2lkdGg9IjEwMCIgaGVpZ2h0PSI2MCIgcng9IjQiLz4KICAgIDxwb2x5Z29uIHBvaW50cz0iMTcwLDEwMCAxODAsOTAgMTkwLDEwMCAyMDAsMTAwIDIwMCwxMjAgMTcwLDEyMCIvPgogICAgPGNpcmNsZSBjeD0iMTgwIiBjeT0iMTAwIiByPSIzIiBmaWxsPSIjZjU5ZTBiIi8+CiAgPC9nPgogIDx0ZXh0IHg9IjIwMCIgeT0iMTgwIiBmb250LWZhbWlseT0iQXJpYWwsIHNhbnMtc2VyaWYiIGZvbnQtc2l6ZT0iMTQiIGZpbGw9IiM2YjcyODAiIHRleHQtYW5jaG9yPSJtaWRkbGUiPlByb3BlcnR5IEltYWdlPC90ZXh0Pgo8L3N2Zz4=';
  
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

  constructor(private propertyService: PropertyService) {
    this.checkMobileView();
  }

  ngOnInit(): void {
    this.loadProperties();
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    this.checkMobileView();
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    this.showBackToTop.set(scrollTop > 300);
  }

  private checkMobileView(): void {
    this.isMobileView.set(window.innerWidth < 768);
    // Auto-close mobile filters on desktop
    if (!this.isMobileView()) {
      this.showMobileFilters.set(false);
    }
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

  // Mobile filter toggle
  toggleMobileFilters(): void {
    this.showMobileFilters.set(!this.showMobileFilters());
  }

  // Check if mobile view
  isMobile(): boolean {
    return this.isMobileView();
  }

  // Scroll to top
  scrollToTop(): void {
    window.scrollTo({ top: 0, behavior: 'smooth' });
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
    // Close mobile filters after clearing
    if (this.isMobile()) {
      this.showMobileFilters.set(false);
    }
  }

  getPropertyImageUrl(property: Property): string {
    if (property.images && property.images.length > 0) {
      const primaryImage = property.images.find(img => img.isPrimary);
      return primaryImage?.imageUrl || property.images[0].imageUrl;
    }
    return this.placeholderImage;
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