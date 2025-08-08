export interface LoginRequest {
  email: string;
  password: string;
}

export interface SignupRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  password: string;
  role: string;
}

export interface LoginResponse {
  id: number;
  email: string;
  token: string;
  fullName: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  role: string;
  isApproved: boolean;
}

export interface SignupResponse {
  message: string;
  user?: User;
}

export interface User {
  id: number;
  email: string;
  fullName: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  role: string;
  isApproved: boolean;
} 