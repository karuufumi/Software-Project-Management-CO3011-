export interface User {
  id: string;
  email: string;
  name: string;
  role: "user" | "admin" | "librarian";
  username?: string;
  phone?: string;
  address?: string;
  department?: string;
  fax?: string;
  joinedAt?: string;
  totalPoints?: number;
  borrowedBooks?: number;
  avatar?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
  message?: string;
}