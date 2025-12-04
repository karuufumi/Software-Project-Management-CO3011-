
export interface User{
    id: string;
    email:string;
    name: string;
    role : "user" | "librarian" | "admin";

    totalPoints?: number;
  borrowedBooks?: number;
  token?: string;
}