export interface ViewingRequest {
  id: number;
  propertyId: number;
  propertyTitle: string;
  tenantId: number;
  tenantName: string;
  viewingDate: Date;
  status: ViewingStatus;
  createdAt: Date;
}

export interface ViewingRequestCreate {
  propertyId: number;
  viewingDate: Date;
}

export enum ViewingStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected',
  Completed = 'Completed'
} 