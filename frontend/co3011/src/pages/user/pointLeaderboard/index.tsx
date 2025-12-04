import LeaderboardTable from "./components/LeaderboardTable";
import "./leaderboard.css";

export default function PointLeaderboardPage() {
  const now = new Date();
  const formattedDate = now.toLocaleDateString("en-GB"); // 30/09/2025 format
  const formattedTime = now.toLocaleTimeString("en-US", {
    hour: "2-digit",
    minute: "2-digit",
  });

  return (
    <div className="leaderboard-page">
      <div className="leaderboard-container">
        <div className="leaderboard-header">
          <div className="leaderboard-meta">
            <span>📅 {formattedDate}</span>
            <span>⏰ {formattedTime}</span>
          </div>
          <h1>Member Leaderboard</h1>
        </div>

        <LeaderboardTable />
      </div>
    </div>
  );
}


