import React, { type CSSProperties, type ReactNode } from "react";

export interface WidgetProps extends CSSProperties {
  title?: string;
  children?: ReactNode;
  background?: string;
  padding?: string;
  roundness?: number;
}

export const Widget: React.FC<WidgetProps> = ({
  title,
  children,
  background = "#fff",
  padding = "16px",
  roundness = 12,
  ...props
}) => {
  return (
    <div
      style={{
        backgroundColor: background,
        borderRadius: `${roundness}px`,
        boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
        padding,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        textAlign: "center",
        gap: "8px",
        ...props,
      }}
    >
      {title && (
        <h3 style={{ fontWeight: 600, marginBottom: "8px" }}>{title}</h3>
      )}
      {children}
    </div>
  );
};

export default Widget;
