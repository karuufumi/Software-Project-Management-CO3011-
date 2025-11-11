import { ProgressBar } from "../../../../../components/progessbar";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";
import { useNavigate } from "react-router-dom";
import paths from "../../../../../routes/paths";

export function UserDashboardData() {
    const navigate = useNavigate();

  const handleSeeAll = () => {
    navigate(paths.USER.LEADERBOARD);
  }
  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>
      <Widget title="New Book" alignItems="flex-start" textAlign="start">
        <div
          style={{
            display: "flex",
            justifyContent: "space-around",
            width: "100%",
          }}
        >
          <div style={{ textAlign: "center" }}>
            <img src={viteLogo} className="logo" alt="Vite logo" />
            <p style={{ color: "rgba(0,0,0,0.7)" }}>
              Snow White and the 7 dwarfs
            </p>
          </div>
          <div style={{ textAlign: "center" }}>
            <img src={viteLogo} className="logo" alt="Vite logo" />
            <p style={{ color: "rgba(0,0,0,0.7)" }}>Mein Kaft</p>
          </div>
        </div>
      </Widget>

      <Widget
        title="Weekly challenge"
        alignItems="flex-start"
        textAlign="start"
      >
        <ProgressBar
          progressTitle="Borrow 1 book"
          progressValue={1}
          progressDisplayValue="+15pts"
          progressValueDescription="1/1"
          color="rgba(0,0,0,0.25)"
        />
        <ProgressBar
          progressTitle="Borrow 5 book"
          progressValue={0.6}
          progressDisplayValue="+40pts"
          progressValueDescription="3/5"
          color="rgba(0,0,0,0.25)"
        />
        <ProgressBar
          progressTitle="Online 180 minutes"
          progressValue={0.5}
          progressDisplayValue="+50pts"
          progressValueDescription="90/180"
          color="rgba(0,0,0,0.25)"
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
            onClick={handleSeeAll}
            style={{
              color: "blue",
              cursor: "pointer",
              textDecoration: "underline",
            }}
          >
            See all {">>"}
          </h3>

        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={viteLogo}
            className="logo"
            alt="Vite logo"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle="Genghis Khan"
            progressValue={1}
            progressDisplayValue="60,031"
            color="pink"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={viteLogo}
            className="logo"
            alt="Vite logo"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle="Adoflt Hitless"
            progressValue={1}
            progressDisplayValue="30,712"
            color="pink"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={viteLogo}
            className="logo"
            alt="Vite logo"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle="Joseph Stalin"
            progressValue={1}
            progressDisplayValue="9,864"
            color="pink"
          />
        </div>
      </Widget>

      <Widget title="Uncommon book" alignItems="flex-start" textAlign="start">
        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={viteLogo}
            className="logo"
            alt="Vite logo"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle="Probability & Statistic Answer keys"
            progressValue={1}
            color="green"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={viteLogo}
            className="logo"
            alt="Vite logo"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle="Calculus 3"
            progressValue={1}
            color="#0088FF"
          />
        </div>
      </Widget>
    </div>
  );
}
