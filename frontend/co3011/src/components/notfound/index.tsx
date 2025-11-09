import { Calendar, Clock } from "lucide-react";
import { Link } from "react-router-dom";
import paths from "../../routes/paths";

export function NotFound() {
  const today = new Date();

  return (
    <>
      <h2
        style={{
          fontWeight: 600,
          marginBottom: 20,
          paddingBottom: 5,
          borderBottom: "1px solid rgba(0,0,0,0.2)",
        }}
      >
        Not Found Page
      </h2>

      <div
        style={{
          display: "flex",
          marginBottom: 20,
          gap: 10,
        }}
      >
        <div style={{ display: "flex", alignItems: "center", gap: 5 }}>
          <Calendar /> {today.getDate()}/{today.getMonth() + 1}/
          {today.getFullYear()}
        </div>
        <div style={{ display: "flex", alignItems: "center", gap: 5 }}>
          <Clock /> {today.getHours()}:{today.getMinutes()}
        </div>
      </div>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <img
          src="https://anhdepbonphuong.com/wp-content/uploads/2024/03/anh-meme-gau-truc-46.png"
          className="logo"
          style={{ height: 408, width: 408 }}
        />
        <Link to={paths.USER.DASHBOARD}>Go to Home</Link>
      </div>
    </>
  );
}
