import { Suspense } from "react";
import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { MainLayout } from "../layout/MainLayout";
import UserProfile from "../pages/user/UserProfile";
import { MemberDashboard } from "../pages/user/Dashboard";
import { UserLibraryCatalog } from "../pages/user/LibraryCatalog";
import { BookContributor } from "../pages/user/BookContributor";
import { History } from "../pages/user/History";

const router = createBrowserRouter([
  {
    element: (
      <Suspense>
        <App />
      </Suspense>
    ),
    children: [
      {
        path: "/",
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
            element: <MainLayout navFocusedElem="library catalog" />,
            children: [
              {
                index: true,
                element: <UserLibraryCatalog />,
              },
              {
                path: "contribute",
                element: <BookContributor />,
              },
            ],
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
    ],
  },
]);

export default router;
