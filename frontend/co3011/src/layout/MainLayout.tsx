import Sidebar from "../components/sidebar/sidebar";
import Button from "../components/button/button";
import { navFocused } from "../data/navbarListData";
import type { PropsWithChildren } from "react";

interface MainLayoutProps extends PropsWithChildren {
  navFocusedElem?: string;
}

export function MainLayout({ navFocusedElem = "", children }: MainLayoutProps) {
  const itemsNav = navFocused("user", navFocusedElem);

  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      {/* Sidebar */}
      <Sidebar
        header={<h2 style={{ fontWeight: 700 }}>BK Library</h2>}
        items={itemsNav}
        footer={
          <div className="sidebar-footer">
            <div className="profile-card">
              <div style={{ fontWeight: 600 }}>Adoft Hitless</div>
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--color-text-secondary)",
                }}
              >
                adoft.hitless@hcmut.edu.vn
              </div>
            </div>
            <Button
              label="Log out"
              color="var(--color-danger)"
              roundness={10}
              onClick={() => alert("Logged out")}
            />
          </div>
        }
      />

      {/* Main content */}
      <div className="main">{children}</div>
    </div>
  );
}
