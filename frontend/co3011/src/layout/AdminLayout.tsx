import Sidebar from "../components/sidebar/sidebar";
import Button from "../components/button/button";
import { navFocused } from "../data/navbarListData";
import type { PropsWithChildren } from "react";
import { Outlet } from "react-router-dom";

interface AdminLayoutProps extends PropsWithChildren {
  navFocusedElem?: string;
}

export function AdminLayout({
  navFocusedElem = "",
  children,
}: AdminLayoutProps) {
  const itemsNav = navFocused("admin", navFocusedElem);
  const navigate = useNavigate();
  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      {/* Sidebar */}
      <Sidebar
        header={<h2 style={{ fontWeight: 700 }}>BK Library</h2>}
        items={itemsNav}
        footer={
          <div className="sidebar-footer">
            <div className="profile-card">
              <div style={{ fontWeight: 600 }}>Admin</div>
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--color-text-secondary)",
                }}
              >
                Administator@hcmut.edu.vn
              </div>
            </div>
            <Button
              label="Log out"
              color="var(--color-danger)"
              roundness={10}
              onClick={() => navigate("/login")}
            />
          </div>
        }
      />

      {/* Main content */}
      <div className="main">{children ? children : <Outlet />}</div>
    </div>
  );
}
