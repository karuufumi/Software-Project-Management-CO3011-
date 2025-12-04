import { Suspense } from "react";
import { createBrowserRouter, Navigate } from "react-router-dom";
import App from "../App";
import { ProtectedRoute } from "./ProtectedRoute";
import { GuestRoute } from "./GuestRoute";

// Layouts
import { GuestLayout } from "../layout/GuestLayout";
import { MainLayout } from "../layout/MainLayout";
import { LibrarianLayout } from "../layout/LibrarianLayout";
import { AdminLayout } from "../layout/AdminLayout";

// Public Pages
import { GuestDashboard } from "../pages/guest/GuestDashboard";
import Login from "../pages/Login";
import Register from "../pages/Register";

// User Pages
import UserMainPage from "../pages/user";
import UserProfile from "../pages/user/UserProfile";
import { UserLibraryCatalog } from "../pages/user/LibraryCatalog";
import { BookDetail } from "../pages/user/BookDetail";
import { BookContributor } from "../pages/user/BookContributor";
import { History } from "../pages/user/History";
import PointLeaderboardPage from "../pages/user/pointLeaderboard";
import Rankmap from "../pages/user/Rankmap";

// Librarian Pages
import GeneralBooks from "../pages/librarian/Dashboard";
import LibrarianAppx from "../pages/librarian/BetterDashboard";
import LibrarianBookDetailx from "../pages/librarian/Details";
import MissingBookHandler from "../pages/librarian/MissingBookHandler";

// Admin Pages
import { AdminDashboard } from "../pages/admin/Dashboard";

// Other
import { NotFound } from "../components/notfound";
import { rootPaths } from "./paths";

const router = createBrowserRouter([
  {
    element: (
      <Suspense fallback={<div>Loading...</div>}>
        <App />
      </Suspense>
    ),
    children: [
      // ============================================
      // ROOT - Redirect to Login
      // ============================================
      {
        path: "/",
        element: <Navigate to="/login" replace />,
      },

      // ============================================
      // GUEST DASHBOARD (if you still want it accessible)
      // ============================================
      {
        path: "/guest",
        element: (
          <GuestLayout navFocusedElem="dashboard">
            <GuestDashboard />
          </GuestLayout>
        ),
      },

      // ============================================
      // AUTH ROUTES
      // ============================================
      {
        path: "/login",
        element: (
          <GuestRoute>
            <Login />
          </GuestRoute>
        ),
      },
      {
        path: "/register",
        element: (
          <GuestRoute>
            <Register />
          </GuestRoute>
        ),
      },

      // ============================================
      // USER ROUTES
      // ============================================
      {
        path: "/user",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="dashboard">
              <UserMainPage />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/profile",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="profile">
              <UserProfile />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/library-catalog",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="library catalog">
              <UserLibraryCatalog />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/library-catalog/book/:bookid",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="library catalog">
              <BookDetail />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/library-catalog/contribute",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="library catalog">
              <BookContributor />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/history",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="history">
              <History />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/leaderboard",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="leaderboard">
              <PointLeaderboardPage />
            </MainLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: "/user/rankmap",
        element: (
          <ProtectedRoute>
            <MainLayout navFocusedElem="progress">
              <Rankmap />
            </MainLayout>
          </ProtectedRoute>
        ),
      },

      // ============================================
      // LIBRARIAN ROUTES
      // ============================================
      {
        path: rootPaths.libRoot,
        element: (
          <ProtectedRoute>
            <LibrarianLayout navFocusedElem="management">
              <GeneralBooks />
            </LibrarianLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: `${rootPaths.libRoot}/books`,
        element: (
          <ProtectedRoute>
            <LibrarianLayout navFocusedElem="books">
              <LibrarianAppx />
            </LibrarianLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: `${rootPaths.libRoot}/management`,
        element: (
          <ProtectedRoute>
            <LibrarianLayout navFocusedElem="management">
              <LibrarianBookDetailx />
            </LibrarianLayout>
          </ProtectedRoute>
        ),
      },
      {
        path: `${rootPaths.libRoot}/missing-book`,
        element: (
          <ProtectedRoute>
            <LibrarianLayout navFocusedElem="missing book">
              <MissingBookHandler />
            </LibrarianLayout>
          </ProtectedRoute>
        ),
      },

      // ============================================
      // ADMIN ROUTES
      // ============================================
      {
        path: rootPaths.adminRoot,
        element: (
          <ProtectedRoute>
            <AdminLayout navFocusedElem="dashboard">
              <AdminDashboard />
            </AdminLayout>
          </ProtectedRoute>
        ),
      },

      // ============================================
      // 404 NOT FOUND
      // ============================================
      {
        path: "*",
        element: (
          <MainLayout>
            <NotFound />
          </MainLayout>
        ),
      },
    ],
  },
]);

export default router;