import { ProgressBar } from "../../../../../components/progessbar";
import Widget from "../../../../../components/widget/widget";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

export function UserDashboardData() {
  const navigate = useNavigate();

  const [borrowCount, setBorrowCount] = useState(0);
  const [onlineMinutes, setOnlineMinutes] = useState(0);

  useEffect(() => {
    const savedBorrow = Number(localStorage.getItem("borrowCount") || 0);
    const savedOnlineMinutes = Number(localStorage.getItem("onlineMinutes") || 0);

    setBorrowCount(savedBorrow);
    setOnlineMinutes(savedOnlineMinutes);

    const interval = setInterval(() => {
      setOnlineMinutes((prev) => {
        const updated = prev + 1;
        localStorage.setItem("onlineMinutes", updated.toString());
        return updated;
      });
    }, 60000);

    return () => clearInterval(interval);
  }, []);

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>

      <Widget title="Recently Added Books" alignItems="flex-start" textAlign="start">
        <p style={{ margin: 0, color: "rgba(0,0,0,0.7)" }}>📗 Introduction to Machine Learning</p>
        <p style={{ margin: 0, color: "rgba(0,0,0,0.7)" }}>📘 Data Structures Using Java</p>
      </Widget>

      <Widget title="Weekly Challenge" alignItems="flex-start" textAlign="start">
        <ProgressBar
          progressTitle="Borrow 1 book"
          progressValue={Math.min(borrowCount / 1, 1)}
          progressDisplayValue="+10pts"
          progressValueDescription={`${borrowCount}/1`}
          color={borrowCount >= 1 ? "green" : "rgba(0,0,0,0.25)"}
        />

        <ProgressBar
          progressTitle="Borrow 5 books"
          progressValue={Math.min(borrowCount / 5, 1)}
          progressDisplayValue="+20pts"
          progressValueDescription={`${borrowCount}/5`}
          color={borrowCount >= 5 ? "green" : "rgba(0,0,0,0.25)"}
        />

        <ProgressBar
          progressTitle="Online 180 minutes"
          progressValue={Math.min(onlineMinutes / 180, 1)}
          progressDisplayValue="+50pts"
          progressValueDescription={`${onlineMinutes}/180`}
          color={onlineMinutes >= 180 ? "green" : "rgba(0,0,0,0.25)"}
        />
      </Widget>

      <Widget alignItems="flex-start" textAlign="start">
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            fontWeight: 600,
            marginBottom: 8,
            width: "100%",
          }}
        >
          <h3 style={{ color: "rgba(0,0,0,0.5)" }}>Leaderboard</h3>
          <h3
            onClick={() => navigate("/user/leaderboard")}
            style={{ color: "blue", cursor: "pointer", textDecoration: "underline" }}
          >
            See all »
          </h3>
        </div>
      </Widget>

      <Widget title="Rare Collections" alignItems="flex-start" textAlign="start">
        <p style={{ margin: "5px 0" }}>📙 Ancient Civil Engineering Manuscript</p>
        <p style={{ margin: "5px 0" }}>📕 1956 Printed - Thermodynamics</p>
      </Widget>
    </div>
  );
}

export default UserDashboardData;
