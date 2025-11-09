import { Suspense } from "react";
import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { MainLayout } from "../layout/MainLayout";
import UserProfile from "../pages/user/UserProfile";
import { MemberDashboard } from "../pages/user/Dashboard";
import { UserLibraryCatalog } from "../pages/user/LibraryCatalog";
import { BookContributor } from "../pages/user/BookContributor";
import { History } from "../pages/user/History";
import { LibrarianLayout } from "../layout/LibrarianLayout";
import { LibrarianDashboard } from "../pages/librarian/Dashboard";
import { AdminLayout } from "../layout/AdminLayout";
import { AdminDashboard } from "../pages/admin/Dashboard";
import { NotFound } from "../components/notfound";
import paths, { rootPaths } from "./paths";

const router = createBrowserRouter([
  {
    element: (
      <Suspense>
        <App />
      </Suspense>
    ),
    children: [
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
            element: <MainLayout navFocusedElem="library catalog" />,
            children: [
              {
                index: true,
                element: <UserLibraryCatalog />,
              },
              {
                path: paths.USER.BOOK_CONTRIBUTE.replace(
                  `${paths.USER.LIBRARY_CATALOG}/`,
                  ""
                ),
                element: <BookContributor />,
              },
            ],
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
        path: rootPaths.libRoot,
        children: [
          {
            index: true,
            element: (
              <LibrarianLayout navFocusedElem="dashboard">
                <LibrarianDashboard />
              </LibrarianLayout>
            ),
          },
        ],
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
