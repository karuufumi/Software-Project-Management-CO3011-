import Sidebar from "../components/sidebar/sidebar";
import Button from "../components/button/button";
import { navFocused } from "../data/navbarListData";
import type { PropsWithChildren } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

interface MainLayoutProps extends PropsWithChildren {
  navFocusedElem?: string;
}

export function MainLayout({ navFocusedElem = "", children }: MainLayoutProps) {
  const itemsNav = navFocused("user", navFocusedElem);
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      {/* Sidebar */}
      <Sidebar
        header={<h2 style={{ fontWeight: 700 }}>BK Library</h2>}
        items={itemsNav}
        footer={
          <div className="sidebar-footer">
            <div className="profile-card">
              <div style={{ fontWeight: 600 }}>{user?.name || "User"}</div>
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--color-text-secondary)",
                }}
              >
                {user?.email || "user@example.com"}
              </div>
            </div>
            <Button
              label="Log out"
              color="var(--color-danger)"
              roundness={10}
              onClick={handleLogout}
            />
          </div>
        }
      />

      {/* Main content */}
      <div className="main">{children ? children : <Outlet />}</div>
    </div>
  );
}