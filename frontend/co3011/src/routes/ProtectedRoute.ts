import React from "react";
import { type JSX } from "react";
import { Navigate } from "react-router-dom";

export const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const isLoggedIn = localStorage.getItem("loggedIn") === "true";
  const token = localStorage.getItem("token");

  if (!isLoggedIn || !token) {
    return React.createElement(Navigate, { to: "/login", replace: true });
  }

  return children;
};