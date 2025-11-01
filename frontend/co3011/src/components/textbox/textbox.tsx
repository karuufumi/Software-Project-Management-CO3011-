import React, { useState } from "react";
import { Eye, EyeOff } from "lucide-react"; // optional icon library

export interface TextBoxProps {
  placeholder?: string;
  type?: "text" | "password" | "email";
  roundness?: number;
  width?: string;
  showToggle?: boolean; // whether to show "eye" icon for passwords
}

const TextBox: React.FC<TextBoxProps> = ({
  placeholder = "Enter text",
  type = "text",
  roundness = 10,
  width = "100%",
  showToggle = false,
}) => {
  const [visible, setVisible] = useState(false);

  const inputType =
    type === "password" && visible ? "text" : type;

  return (
    <div
      style={{
        position: "relative",
        width,
      }}
    >
      <input
        type={inputType}
        placeholder={placeholder}
        style={{
          width: "100%",
          padding: "12px 40px 12px 16px",
          fontSize: "16px",
          border: "1px solid #e5e7eb",
          borderRadius: `${roundness}px`,
          outline: "none",
          backgroundColor: "#f9f9f9",
          transition: "border 0.2s, box-shadow 0.2s",
        }}
        onFocus={(e) => {
          e.currentTarget.style.border = "1px solid var(--color-primary)";
          e.currentTarget.style.boxShadow = "0 0 4px rgba(79, 119, 255, 0.4)";
        }}
        onBlur={(e) => {
          e.currentTarget.style.border = "1px solid #e5e7eb";
          e.currentTarget.style.boxShadow = "none";
        }}
      />

      {type === "password" && showToggle && (
        <button
          type="button"
          onClick={() => setVisible(!visible)}
          style={{
            position: "absolute",
            right: "10px",
            top: "50%",
            transform: "translateY(-50%)",
            background: "none",
            border: "none",
            cursor: "pointer",
            color: "#555",
          }}
        >
          {visible ? <EyeOff size={20} /> : <Eye size={20} />}
        </button>
      )}
    </div>
  );
};

export default TextBox;