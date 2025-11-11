import React from "react";
import LeaderboardTable from "./components/LeaderboardTable";
import "./leaderboard.css";

export default function PointLeaderboardPage() {
  return (
    <div className="leaderboard-page">
      <div className="leaderboard-container">
        <div className="leaderboard-header">
          <div className="leaderboard-meta">
            <span>📅 30/9/2025</span>
            <span>⏰ 09:00</span>
          </div>
          <h1>Member Leaderboard</h1>
        </div>

        <LeaderboardTable />
      </div>
    </div>
  );
}
