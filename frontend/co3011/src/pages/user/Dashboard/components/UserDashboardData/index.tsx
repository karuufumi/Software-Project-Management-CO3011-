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
          
          <ProgressBar
            progressTitle="John Doe"
            progressValue={1}
            progressDisplayValue="1300"
            color="pink"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          
          <ProgressBar
            progressTitle="Adam Jones"
            progressValue={1}
            progressDisplayValue="1230"
            color="pink"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          
          <ProgressBar
            progressTitle="Joseph Gilles"
            progressValue={1}
            progressDisplayValue="1224"
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
            progressTitle="The Lord of the Rings"
            progressValue={1}
            color="#0088FF"
          />
        </div>
      </Widget>
    </div>
  );
}
