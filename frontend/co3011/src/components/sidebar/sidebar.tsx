import React, { type ReactNode } from "react";

interface SidebarProps {
  header?: ReactNode;
  footer?: ReactNode;
  children?: ReactNode;
  width?: string;
  background?: string;
  padding?: string;
  roundness?: number;
  gap?: string;
}

export const Sidebar: React.FC<SidebarProps> = ({
  header,
  footer,
  children,
  width = "240px",
  background = "#fff",
  padding = "16px",
  roundness = 0,
  gap = "12px",
}) => {
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        width,
        minHeight: "100vh",
        backgroundColor: background,
        borderRadius: `${roundness}px`,
        padding,
        boxShadow: "2px 0 8px rgba(0,0,0,0.05)",
      }}
    >
      <div style={{ display: "flex", flexDirection: "column", gap }}>
        {header && <div>{header}</div>}
        {children}
      </div>

      {footer && <div style={{ marginTop: "auto" }}>{footer}</div>}
    </div>
  );
};

export default Sidebar;