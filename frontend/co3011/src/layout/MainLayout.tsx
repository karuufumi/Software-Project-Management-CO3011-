import Sidebar from "../components/sidebar/sidebar";
import Button from "../components/button/button";
import { navFocused } from "../data/navbarListData";
import type { PropsWithChildren } from "react";
import { Outlet, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

interface MainLayoutProps extends PropsWithChildren {
  navFocusedElem?: string;
}

export function MainLayout({ navFocusedElem = "", children }: MainLayoutProps) {
  const itemsNav = navFocused("user", navFocusedElem);
  const navigate = useNavigate();

  const [username, setUsername] = useState("Unknown User");
  const [email, setEmail] = useState("No email");

  useEffect(() => {
    const storedUsername = localStorage.getItem("username");
    const storedEmail = localStorage.getItem("email");

    if (storedUsername) setUsername(storedUsername);
    if (storedEmail) setEmail(storedEmail);
  }, []);

  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      {/* Sidebar */}
      <Sidebar
        header={<h2 style={{ fontWeight: 700 }}>BK Library</h2>}
        items={itemsNav}
        footer={
          <div className="sidebar-footer" style={{ marginTop: "auto" }}>
            <div
              className="profile-card"
              style={{ cursor: "pointer" }}
              onClick={() => navigate("/userprofile")}
            >
              <div style={{ fontWeight: 600 }}>{username}</div>
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--color-text-secondary)",
                }}
              >
                {email}
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

export default MainLayout;
