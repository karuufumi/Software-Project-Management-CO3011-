import { ProgressBar } from "../../../../../components/progessbar";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";
import { booksData } from "../../../booksData";

export function GuestDashboardData() {
  const leaderboard = [
    { name: "Genghis Khan", score: "80,972", avatar: viteLogo, progress: 1 },
    { name: "Adoflt Hitless", score: "30,712", avatar: viteLogo, progress: 1 },
    { name: "Joseph Stalin", score: "9,864", avatar: viteLogo, progress: 1 },
  ];

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>
      {/* === New Book === */}
      <Widget title="New Book" alignItems="flex-start" textAlign="start">
        <div
          style={{
            display: "flex",
            justifyContent: "space-around",
            width: "100%",
          }}
        >
          <div style={{ textAlign: "center" }}>
            <img src={viteLogo} className="logo" alt="Snow White" />
            <p style={{ color: "rgba(0,0,0,0.7)" }}>
              Snow White and the 7 dwarfs
            </p>
          </div>
          <div style={{ textAlign: "center" }}>
            <img src={viteLogo} className="logo" alt="Mein Kaft" />
            <p style={{ color: "rgba(0,0,0,0.7)" }}>Mein Kaft</p>
          </div>
        </div>
      </Widget>

      {/* === Membership Register Price === */}
      <Widget
        title="Membership Register Price"
        alignItems="flex-start"
        textAlign="start"
      >
        <div
          style={{
            display: "flex",
            justifyContent: "space-around",
            width: "100%",
            textAlign: "center",
          }}
        >
          <div>
            <h3 style={{ fontSize: 18, marginBottom: 4 }}>$1.99</h3>
            <p style={{ color: "rgba(0,0,0,0.5)" }}>Monthly</p>
          </div>
          <div>
            <h3 style={{ fontSize: 18, marginBottom: 4 }}>$15.99</h3>
            <p style={{ color: "rgba(0,0,0,0.5)" }}>Annually</p>
          </div>
        </div>
      </Widget>

      {/* === Leaderboard === */}
      <Widget title="Leaderboard" alignItems="flex-start" textAlign="start">
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            gap: 18,
            width: "100%",
          }}
        >
          {leaderboard.map((item, idx) => (
            <div
              key={idx}
              style={{
                display: "flex",
                alignItems: "flex-start",
                gap: 12,
                width: "100%",
              }}
            >
              {/* Avatar */}
              <img
                src={item.avatar}
                alt={item.name}
                style={{
                  width: 56,
                  height: 56,
                  borderRadius: 6,
                  objectFit: "cover",
                  flexShrink: 0,
                }}
              />

              {/* Content area fills all remaining space */}
              <div
                style={{
                  flex: 1,
                  width: "100%",
                }}
              >
                {/* Name + Score row */}
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "flex-start",
                    fontWeight: 700,
                    marginBottom: 6,
                    width: "100%",
                  }}
                >
                  {/* Allow wrapping for long names */}
                  <span
                    style={{
                      fontSize: 15,
                      textAlign: "left",
                      wordBreak: "break-word",
                      lineHeight: 1.3,
                    }}
                  >
                    {item.name}
                  </span>
                  <span
                    style={{
                      fontSize: 15,
                      textAlign: "right",
                      flexShrink: 0,
                    }}
                  >
                    {item.score}
                  </span>
                </div>

                {/* Progress bar fills 100% width */}
                <div
                  style={{
                    height: 10,
                    background: "rgba(0,0,0,0.08)",
                    borderRadius: 6,
                    width: "100%",
                  }}
                >
                  <div
                    style={{
                      width: `${item.progress * 100}%`,
                      height: "100%",
                      background: "#FCA5A5",
                      borderRadius: 6,
                    }}
                  />
                </div>
              </div>
            </div>
          ))}
        </div>
      </Widget>



      {/* === Popular Uncommon Books === */}
      <Widget title="Popular Uncommon Books" alignItems="flex-start">
        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={booksData.academic[3].image}
            className="logo"
            alt="Book"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle={booksData.academic[2].title}
            progressValue={1}
            color="green"
          />
        </div>

        <div style={{ display: "flex", width: "100%", gap: 10 }}>
          <img
            src={booksData.academic[3].image}
            className="logo"
            alt="Book"
            style={{ maxHeight: 55 }}
          />
          <ProgressBar
            progressTitle={booksData.academic[3].title}
            progressValue={0.8}
            color="#0088FF"
          />
        </div>
      </Widget>
    </div>
  );
}

export default GuestDashboardData;
