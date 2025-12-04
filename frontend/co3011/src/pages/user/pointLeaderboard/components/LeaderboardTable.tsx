import { useState } from "react";
import "./leaderboard.css";

export default function LeaderboardTable() {
  const [data] = useState([
  { id: 1, name: "Alice Johnson", points: 60031, avatar: "https://randomuser.me/api/portraits/women/45.jpg" },
  { id: 2, name: "Michael Smith", points: 30712, avatar: "https://randomuser.me/api/portraits/men/46.jpg" },
  { id: 3, name: "Sarah Williams", points: 9864, avatar: "https://randomuser.me/api/portraits/women/47.jpg" },
  { id: 4, name: "David Lee", points: 6363, avatar: "https://randomuser.me/api/portraits/men/12.jpg" },
  { id: 5, name: "Emma Brown", points: 6580, avatar: "https://randomuser.me/api/portraits/women/33.jpg" },
  { id: 6, name: "Chris Wilson", points: 4513, avatar: "https://randomuser.me/api/portraits/men/21.jpg" },
  { id: 7, name: "Olivia Davis", points: 3636, avatar: "https://randomuser.me/api/portraits/women/70.jpg" },
  { id: 8, name: "James Miller", points: 2798, avatar: "https://randomuser.me/api/portraits/men/25.jpg" },
  { id: 9, name: "Sophia Anderson", points: 2036, avatar: "https://randomuser.me/api/portraits/women/26.jpg" },
  { id: 10, name: "William Garcia", points: 1989, avatar: "https://randomuser.me/api/portraits/men/77.jpg" },
]);


  return (
    <div className="leaderboard-wrapper">
      <table className="leaderboard-table">
        <thead>
          <tr>
            <th className="rank-col">Rank</th>
            <th className="name-col">Name</th>
            <th className="points-col">Total Points</th>
          </tr>
        </thead>

        <tbody>
          {data.map((entry, index) => (
            <tr key={entry.id} className="leaderboard-row">
              <td className="rank-cell">
                {index === 0 ? "🥇" : index === 1 ? "🥈" : index === 2 ? "🥉" : index + 1}
              </td>
              <td className="name-cell">
                <img src={entry.avatar} alt={entry.name} className="leaderboard-avatar" />
                <span>{entry.name}</span>
              </td>
              <td className="points-cell">{entry.points.toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
