export interface PropertyImage {
  id: number;
  imageUrl: string;
  displayOrder: number;
  isPrimary: boolean;
}

export interface Property {
  id: number;
  title: string;
  description: string;
  rentalAmount: number;
  address: string;
  bedrooms: number;
  bathrooms: number;
  agentName: string;
  images: PropertyImage[];
  primaryImageUrl: string;
}

export interface PropertyCreateRequest {
  title: string;
  description: string;
  rentalAmount: number;
  address: string;
  bedrooms: number;
  bathrooms: number;
  images: File[];
  imageOrders: number[];
  primaryImageIndex: number;
}

export interface PropertyUpdateRequest {
  title: string;
  description: string;
  rentalAmount: number;
  address: string;
  bedrooms: number;
  bathrooms: number;
  images?: File[];
  imageOrders?: number[];
  primaryImageIndex?: number;
} 