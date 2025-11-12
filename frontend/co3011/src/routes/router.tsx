import { Suspense } from "react";
import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { NotFound } from "../components/notfound";
import { AdminLayout } from "../layout/AdminLayout";
import { LibrarianLayout } from "../layout/LibrarianLayout";
import { MainLayout } from "../layout/MainLayout";
import { AdminDashboard } from "../pages/admin/Dashboard";
import { LibrarianDashboard } from "../pages/librarian/Dashboard";
import Login from "../pages/Login";
import Register from "../pages/Register";
import { BookContributor } from "../pages/user/BookContributor";
import { BookDetail } from "../pages/user/BookDetail/index";
import { MemberDashboard } from "../pages/user/Dashboard";
import { History } from "../pages/user/History";
// <<<<<<< Login
// import { UserLibraryCatalog } from "../pages/user/LibraryCatalog";
// import PointLeaderboardPage from "../pages/user/pointLeaderboard/index";
// import Rankmap from "../pages/user/Rankmap/index";
// import UserProfile from "../pages/user/UserProfile";
// import paths, { rootPaths } from "./paths";
// =======
import { LibrarianLayout } from "../layout/LibrarianLayout";
import GeneralBooks  from "../pages/librarian/Dashboard";
import { AdminLayout } from "../layout/AdminLayout";
import { AdminDashboard } from "../pages/admin/Dashboard";
import { GuestLayout } from "../layout/GuestLayout"; 
import { GuestDashboard } from "../pages/guest/GuestDashboard"; 
import { NotFound } from "../components/notfound";
import paths, { rootPaths } from "./paths";
import PointLeaderboardPage from "../pages/user/pointLeaderboard/index";
import Rankmap from "../pages/user/Rankmap/index";
import UserProfile from "../pages/user/UserProfile";

import {BookDetail} from "../pages/user/BookDetail/index";
import LibrarianBookDetailx from "../pages/librarian/Details";
import LibrarianAppx from "../pages/librarian/BetterDashboard";

import Login from "../pages/Login";


const router = createBrowserRouter([
  {
    element: (
      <Suspense>
        <App />
      </Suspense>
    ),
    children: [
      {
        path: rootPaths.guestRoot,
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
      {
        path: "/login",
        element: <Login />,
      },
      {
        path: "/register",
        element: <Register/>,
      },
      {
        path: rootPaths.userRoot,
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
            path: paths.USER.PROFILE.replace(rootPaths.userRoot, ""),
            element: (
              <MainLayout>
                <UserProfile />
              </MainLayout>
            ),
          },
         {
  path: paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, ""),
  element: (
    <MainLayout navFocusedElem="library catalog">
      <UserLibraryCatalog />
    </MainLayout>
  ),
},
{
  path: `${paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, "")}/book/:bookid`,
  element: (
    <MainLayout navFocusedElem="library catalog">
      <BookDetail />
    </MainLayout>
  ),
},

{
  path: `${paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, "")}/contribute`,
  element: (
    <MainLayout navFocusedElem="library catalog">
      <BookContributor />
    </MainLayout>
  ),
},



          {
            path: paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, ""),
            element: (
              <MainLayout navFocusedElem="library catalog">
                <UserLibraryCatalog />
              </MainLayout>
            ),
          },
          {
            path: `${paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, "")}/book/:bookid`,
            element: (
              <MainLayout navFocusedElem="library catalog">
                <BookDetail />
              </MainLayout>
            ),
          },
          {
            path: `${paths.USER.LIBRARY_CATALOG.replace(rootPaths.userRoot, "")}/contribute`,
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
            path: paths.USER.HISTORY.replace(rootPaths.userRoot, ""),
            element: (
              <MainLayout navFocusedElem="history">
                <History />
              </MainLayout>
            ),
          },
        ],
      },
      {
        path: `${rootPaths.libRoot}/books`,
        children: [
          {
            index: true,
            element: (
              <LibrarianLayout navFocusedElem="management">
                <LibrarianAppx />
              </LibrarianLayout>
            ),
          },
        ],
      },
      {
        path: `${rootPaths.libRoot}/management`,
        children: [
          {
            index: true,
            element: (
              <LibrarianLayout navFocusedElem="management">
                <LibrarianBookDetailx />
              </LibrarianLayout>
            ),
          },
        ]
      },
            {
        path: `${rootPaths.libRoot}`,
        children: [
          {
            index: true,
            element: (
              <LibrarianLayout navFocusedElem="management">
                <GeneralBooks />
              </LibrarianLayout>
            ),
          },
        ]
      },
      {
        path: rootPaths.adminRoot,
        children: [
          {
            index: true,
            element: (
              <AdminLayout navFocusedElem="dashboard">
                <AdminDashboard />
              </AdminLayout>
            ),
          },
        ],
      },
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