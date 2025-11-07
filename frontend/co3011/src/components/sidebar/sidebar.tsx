import React, { type ReactNode } from "react";
import { useNavigate } from "react-router-dom";

interface SidebarProps {
  header?: ReactNode;
  footer?: ReactNode;
  items?: SidebarItem[];
  background?: string;
  width?: string;
  roundness?: number;
  padding?: string;
  gap?: string;
}

export interface SidebarItem {
  label: string;
  icon?: ReactNode;
  path?: string;
  onClick?: () => void;
  active?: boolean;
  role?: string;
}

const Sidebar: React.FC<SidebarProps> = ({
  header,
  footer,
  items = [],
  background = "#fff",
  width = "260px",
  padding = "20px",
  roundness = 16,
  gap = "8px",
}) => {
  const navigate = useNavigate();

  const handleClick = (item: SidebarItem) => {
    if (item.onClick) item.onClick();
    else if (item.path) navigate(item.path);
  };

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        backgroundColor: background,
        width,
        minHeight: "100vh",
        borderRadius: `${roundness}px`,
        padding,
        boxShadow: "2px 0 8px rgba(0,0,0,0.05)",
      }}
    >
      <div style={{ display: "flex", flexDirection: "column", gap }}>
        {header && <div style={{ marginBottom: "1rem" }}>{header}</div>}

        {items.map((item, index) => (
          <button
            key={index}
            onClick={() => handleClick(item)}
            style={{
              display: "flex",
              alignItems: "center",
              gap: "10px",
              backgroundColor: item.active ? "#eef2ff" : "transparent",
              color: item.active ? "#3b82f6" : "#000",
              border: "none",
              borderRadius: "10px",
              padding: "8px 12px",
              cursor: "pointer",
              fontSize: "15px",
              fontWeight: 500,
              transition: "background 0.2s",
            }}
          >
            {item.icon && (
              <span
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                {item.icon}
              </span>
            )}
            {item.label}
          </button>
        ))}
      </div>

      {footer && <div style={{ marginTop: "auto" }}>{footer}</div>}
    </div>
  );
};

export default Sidebar;
