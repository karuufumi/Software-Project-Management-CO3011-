import React from "react";
import "./Rankmap.css";

const Rankmap = () => {
  const userPoints = 45000; // example

  const ranks = [
    { min: 0, max: 2000, range: "0–2000", color: "gray", side: "left" },
    { min: 2001, max: 12000, range: "2001–12000", color: "limegreen", side: "right" },
    { min: 12001, max: 30000, range: "12001–30000", color: "cyan", side: "left" },
    { min: 30001, max: 120000, range: "30001–120000", color: "magenta", side: "right" },
    { min: 120001, max: Infinity, range: "120001+", color: "red", side: "left" },
  ];

  const currentIndex = ranks.findIndex(
    (r) => userPoints >= r.min && userPoints <= r.max
  );

  return (
    <div className="rankmap-container">
      <h1 className="rankmap-title">Membership Rankmap</h1>
      <div className="rankmap-line"></div>

      <div className="rankmap-items">
        {ranks.map((rank, index) => (
          <div key={index} className={`rankmap-row ${rank.side}`}>
            <div
              className={`rankmap-box ${index === currentIndex ? "current" : ""}`}
              style={{ backgroundColor: rank.color }}
            >
              {rank.range}
              {index === currentIndex && (
                <span
                  className={`rankmap-current-label ${
                    rank.side === "left" ? "left" : "right"
                  }`}
                >
                  Currently here
                </span>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Rankmap;
