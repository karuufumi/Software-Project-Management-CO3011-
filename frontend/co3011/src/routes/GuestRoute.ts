import { type JSX } from "react";
import { Navigate } from "react-router-dom";
import { rootPaths } from "../routes/paths";

export const GuestRoute = ({ children }: { children: JSX.Element }) => {
  //const isLoggedIn = localStorage.getItem("loggedIn") === "true";

   {
    return Navigate({ to: rootPaths.userRoot, replace: true });
  }

  return children;
};