import { type ReactNode } from "react";
import { Navigate } from "react-router-dom";

interface ProtectedRouteProps {
  children: ReactNode;
}

export const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
  const token = localStorage.getItem("token");
  const isLoggedIn = localStorage.getItem("loggedIn") === "true";

  if (!token || !isLoggedIn) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};