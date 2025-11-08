import type { CSSProperties } from "react";
import styled from "@emotion/styled";

interface ProgressBarProps extends CSSProperties {
  progressTitle?: string;
  progressValue?: number;
  progressDisplayValue?: string;
  progressValueDescription?: string;
}

const CustomProgess = styled.progress`
  width: 100%;
  appearance: none;
  background: transparent !important;
  border-radius: 100px;

  /* Chrome and Safari */
  &::-webkit-progress-bar {
    background: ${(props) =>
      props.style?.backgroundColor ?? "rgba(0, 0, 0, 0.1)"};
    border-radius: 100px;
  }

  &::-webkit-progress-value {
    background-color: ${(props) => props.style?.color ?? undefined};
    border-radius: 6px;
    transition: width 0.3s ease;
  }

  /* Firefox */
  &::-moz-progress-bar {
    background-color: ${(props) => props.style?.color ?? undefined};
    border-top-left-radius: 100px;
    border-bottom-left-radius: 100px;
  }
`;

export const ProgressBar: React.FC<ProgressBarProps> = ({
  progressTitle,
  progressValue,
  progressDisplayValue,
  progressValueDescription,
  ...props
}) => {
  return (
    <div
      style={{
        display: "flex",
        width: "100%",
        alignItems: "end",
        gap: 20,
      }}
    >
      <div style={{ width: "100%" }}>
        <div
          style={{
            display: "flex",
            width: "100%",
            justifyContent: "space-between",
          }}
        >
          <p>{progressTitle}</p>
          <p>{progressDisplayValue}</p>
        </div>
        <CustomProgess value={progressValue ?? 0} style={{ ...props }} />
      </div>
      <p>{progressValueDescription}</p>
    </div>
  );
};
