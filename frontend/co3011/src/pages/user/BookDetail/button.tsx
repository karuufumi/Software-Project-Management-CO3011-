import React from "react";

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children?: React.ReactNode; 
}

const Button: React.FC<ButtonProps> = ({ children, ...props }) => {
  return (
    <button
      {...props}
      style={{
        padding: "8px 16px",
        borderRadius: "8px",
        cursor: "pointer",
        ...props.style, // allow custom styles
      }}
    >
      {children}
    </button>
  );
};

export default Button;
