import { Component, ChangeDetectionStrategy, signal, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../services/property.service';
import { Property } from '../../models/property.model';

@Component({
  selector: 'app-property-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './property-list.component.html',
  styleUrl: './property-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PropertyListComponent implements OnInit {
  @Output() editProperty = new EventEmitter<Property>();

  properties = signal<Property[]>([]);
  filteredProperties = signal<Property[]>([]);
  isLoading = signal(true);
  searchTerm = signal('');
  selectedFilter = signal('all');
  viewMode = signal<'grid' | 'list'>('grid');

  // Pagination
  currentPage = signal(1);
  itemsPerPage = signal(12);
  totalItems = signal(0);

  // Modal states
  showDeleteModal = signal(false);
  propertyToDelete = signal<Property | null>(null);
  isDeleting = signal(false);

  constructor(private propertyService: PropertyService) {}

  ngOnInit() {
    this.loadProperties();
  }

  loadProperties() {
    this.isLoading.set(true);
    this.propertyService.getAgentProperties().subscribe({
      next: (properties) => {
        this.properties.set(properties);
        this.filteredProperties.set(properties);
        this.totalItems.set(properties.length);
        this.isLoading.set(false);
        this.applyFilters();
      },
      error: (error) => {
        console.error('Error loading properties:', error);
        this.isLoading.set(false);
      }
    });
  }

  onSearchChange(event: Event) {
    const target = event.target as HTMLInputElement;
    const searchTerm = target.value;
    this.searchTerm.set(searchTerm);
    this.applyFilters();
  }

  onFilterChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const filter = target.value;
    this.selectedFilter.set(filter);
    this.applyFilters();
  }

  applyFilters() {
    let filtered = this.properties();

    // Apply search filter
    const search = this.searchTerm().toLowerCase();
    if (search) {
      filtered = filtered.filter(property => 
        property.title.toLowerCase().includes(search) ||
        property.address.toLowerCase().includes(search) ||
        property.description.toLowerCase().includes(search)
      );
    }

    // Apply category filter
    const filter = this.selectedFilter();
    if (filter !== 'all') {
      switch (filter) {
        case 'studio':
          filtered = filtered.filter(p => p.bedrooms === 0);
          break;
        case '1-bed':
          filtered = filtered.filter(p => p.bedrooms === 1);
          break;
        case '2-bed':
          filtered = filtered.filter(p => p.bedrooms === 2);
          break;
        case '3-bed':
          filtered = filtered.filter(p => p.bedrooms >= 3);
          break;
        case 'high-price':
          filtered = filtered.filter(p => p.rentalAmount > 15000);
          break;
        case 'low-price':
          filtered = filtered.filter(p => p.rentalAmount <= 15000);
          break;
      }
    }

    this.filteredProperties.set(filtered);
    this.totalItems.set(filtered.length);
    this.currentPage.set(1);
  }

  toggleViewMode() {
    this.viewMode.set(this.viewMode() === 'grid' ? 'list' : 'grid');
  }

  get paginatedProperties() {
    const start = (this.currentPage() - 1) * this.itemsPerPage();
    const end = start + this.itemsPerPage();
    return this.filteredProperties().slice(start, end);
  }

  get totalPages() {
    return Math.ceil(this.totalItems() / this.itemsPerPage());
  }

  getEndIndex(): number {
    return Math.min(this.currentPage() * this.itemsPerPage(), this.totalItems());
  }

  getPaginationPages(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage.set(page);
    }
  }

  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.set(this.currentPage() - 1);
    }
  }

  nextPage() {
    if (this.currentPage() < this.totalPages) {
      this.currentPage.set(this.currentPage() + 1);
    }
  }

  onEditProperty(property: Property) {
    this.editProperty.emit(property);
  }

  confirmDelete(property: Property) {
    this.propertyToDelete.set(property);
    this.showDeleteModal.set(true);
  }

  cancelDelete() {
    this.showDeleteModal.set(false);
    this.propertyToDelete.set(null);
  }

  deleteProperty() {
    const property = this.propertyToDelete();
    if (!property) return;

    this.isDeleting.set(true);
    this.propertyService.deleteProperty(property.id).subscribe({
      next: () => {
        this.loadProperties();
        this.cancelDelete();
        this.isDeleting.set(false);
      },
      error: (error) => {
        console.error('Error deleting property:', error);
        this.isDeleting.set(false);
      }
    });
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-ZA', {
      style: 'currency',
      currency: 'ZAR'
    }).format(amount);
  }

  getPropertyImageUrl(property: Property): string {
    return property.primaryImageUrl || 'assets/placeholder-property.jpg';
  }
}
