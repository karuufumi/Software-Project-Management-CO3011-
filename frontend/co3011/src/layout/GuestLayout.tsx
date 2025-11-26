import Sidebar from "../components/sidebar/sidebar";
import Button from "../components/button/button";
import { navFocused } from "../data/navbarListData";
import type { PropsWithChildren } from "react";
import { Outlet } from "react-router-dom";
import { useNavigate } from "react-router-dom";
interface GuestLayoutProps extends PropsWithChildren {
  navFocusedElem?: string;
}

export function GuestLayout({
  navFocusedElem = "",
  children,
}: GuestLayoutProps) {
  // use the guest menu key (was "lib")
  const items = navFocused("guest", navFocusedElem) || [];
  const navigate = useNavigate();

  // fallback if navFocused returns nothing
  const fallbackItems = [
    {
      id: "dashboard",
      label: "Dashboard",  
      title: "Dashboard",
      path: "/",
      icon: undefined,     
    },
    {
      id: "history",
      label: "History",     
      title: "History",
      path: "/history",
      icon: undefined,
    },
  ];
  const itemsNav = items.length ? items : fallbackItems;

/*
  const handleLoginPlaceholder = () => {
    alert("Log in not implemented yet.");
  };
  

  
  const handleSignup = () => {
    navigate("/signup");
  }
    */
   const handleLogin = () => {
    navigate("/login");
  }
const handleSignupPlaceholder = () => {
    alert("Sign up not implemented yet.");
  };

  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      {/* Sidebar */}
      <Sidebar
        header={<h2 style={{ fontWeight: 700 }}>BK Library</h2>}
        items={itemsNav}
        footer={
          // ensure footer sits at the bottom by using marginTop: 'auto'
          <div className="sidebar-footer" style={{ marginTop: "auto", padding: 12 }}>
            <div className="profile-card" style={{ marginBottom: 8 }}>
              <div style={{ fontWeight: 600 }}>Guest</div>
              <div
                style={{
                  fontSize: "12px",
                  color: "var(--color-text-secondary)",
                }}
              >
                Please log in or sign up
              </div>
            </div>

            <div style={{ display: "flex", gap: 8, flexDirection: "column", alignItems: "stretch" }}>
              <Button
                label="Log in"
                color="var(--color-primary)"
                roundness={10}
                onClick={handleLogin}
              />
              <Button
                label="Sign up"
                color="#ABABAB"
                roundness={10}
                onClick={handleSignupPlaceholder}
              />
            </div>
          </div>
        }
      />

      {/* Main content */}
      <div className="main" style={{ flex: 1 }}>
        {children ? children : <Outlet />}
      </div>
    </div>
  );
}

export default GuestLayout;
