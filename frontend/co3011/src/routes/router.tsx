import { Suspense } from "react";
import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { NotFound } from "../components/notfound";

import { AdminLayout } from "../layout/AdminLayout";
import { MainLayout } from "../layout/MainLayout";
import { LibrarianLayout } from "../layout/LibrarianLayout";
import { GuestLayout } from "../layout/GuestLayout";

import Login from "../pages/Login";
import Register from "../pages/Register";

import { GuestDashboard } from "../pages/guest/GuestDashboard";
import { MemberDashboard } from "../pages/user/Dashboard";
import { UserLibraryCatalog } from "../pages/user/LibraryCatalog";
import { BookDetail } from "../pages/user/BookDetail";
import { BookContributor } from "../pages/user/BookContributor";
import { History } from "../pages/user/History";
import PointLeaderboardPage from "../pages/user/pointLeaderboard";
import Rankmap from "../pages/user/Rankmap";
import UserProfile from "../pages/user/UserProfile";

import GeneralBooks from "../pages/librarian/Dashboard";
import LibrarianBookDetailx from "../pages/librarian/Details";
import LibrarianAppx from "../pages/librarian/BetterDashboard";
import MissingBookHandler from "../pages/librarian/MissingBookHandler";

import { AdminDashboard } from "../pages/admin/Dashboard";

const router = createBrowserRouter([
  {
    element: (
      <Suspense>
        <App />
      </Suspense>
    ),
    children: [
      // GUEST ROUTES
      {
        path: "/",
        children: [
          {
            index: true,
            element: (
              <GuestLayout navFocusedElem="dashboard">
                <GuestDashboard />
              </GuestLayout>
            ),
          },
        ],
      },

      // AUTH
      { path: "/login", element: <Login /> },
      { path: "/register", element: <Register /> },

      // USER ROUTES
      {
        path: "/user",
        children: [
          {
            index: true,
            element: (
              <MainLayout navFocusedElem="dashboard">
                <MemberDashboard />
              </MainLayout>
            ),
          },
          {
            path: "profile",
            element: (
              <MainLayout>
                <UserProfile />
              </MainLayout>
            ),
          },
          {
            path: "catalog",
            element: (
              <MainLayout navFocusedElem="library catalog">
                <UserLibraryCatalog />
              </MainLayout>
            ),
          },
          {
            path: "catalog/book/:bookid",
            element: (
              <MainLayout navFocusedElem="library catalog">
                <BookDetail />
              </MainLayout>
            ),
          },
          {
            path: "catalog/contribute",
            element: (
              <MainLayout navFocusedElem="library catalog">
                <BookContributor />
              </MainLayout>
            ),
          },
          {
            path: "leaderboard",
            element: (
              <MainLayout navFocusedElem="leaderboard">
                <PointLeaderboardPage />
              </MainLayout>
            ),
          },
          {
            path: "rankmap",
            element: (
              <MainLayout navFocusedElem="progress">
                <Rankmap />
              </MainLayout>
            ),
          },
          {
            path: "history",
            element: (
              <MainLayout navFocusedElem="history">
                <History />
              </MainLayout>
            ),
          },
        ],
      },

      // LIBRARIAN ROUTES
      {
        path: "/librarian/book",
        element: (
          <LibrarianLayout navFocusedElem="management">
            <LibrarianAppx />
          </LibrarianLayout>
        ),
      },
      {
        path: "/librarian/management",
        element: (
          <LibrarianLayout navFocusedElem="management">
            <LibrarianBookDetailx />
          </LibrarianLayout>
        ),
      },
      {
        path: "/librarian",
        element: (
          <LibrarianLayout navFocusedElem="management">
            <GeneralBooks />
          </LibrarianLayout>
        ),
      },
      {
        path: "/librarian/missing-book",
        element: (
          <LibrarianLayout navFocusedElem="missing book">
            <MissingBookHandler />
          </LibrarianLayout>
        ),
      },

      // ADMIN ROUTES
      {
        path: "/admin",
        element: (
          <AdminLayout navFocusedElem="dashboard">
            <AdminDashboard />
          </AdminLayout>
        ),
      },

      // 404
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
