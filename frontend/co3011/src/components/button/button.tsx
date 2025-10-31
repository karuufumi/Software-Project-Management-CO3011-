import React from "react";

export interface ButtonProps {
  label: string;
  onClick?: () => void;
  color?: string;
  roundness?: number;
}

export const Button: React.FC<ButtonProps> = ({
  label,
  onClick,
  color = "#4F77FF",
  roundness = 9999,
}) => {
  return (
    <button
      onClick={onClick}
      style={{
        backgroundColor: color,
        color: "white",
        padding: "10px 30px",
        border: "none",
        borderRadius: `${roundness}px`,
        cursor: "pointer",
        fontSize: "16px",
        fontWeight: 500,
        boxShadow: "0 2px 6px rgba(0,0,0,0.15)",
        transition: "background-color 0.2s, transform 0.1s",
      }}
      onMouseDown={(e) => (e.currentTarget.style.transform = "scale(0.97)")}
      onMouseUp={(e) => (e.currentTarget.style.transform = "scale(1)")}
    >
      {label}
    </button>
  );
};

export default Button;