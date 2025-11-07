import Widget from "../../../components/widget/widget";
import Button from "../../../components/button/button"; // ✅ correct import

export default function UserProfile() {
  return (
    <>
      <h2 style={{ fontWeight: 600, marginBottom: "20px" }}>Book Detail</h2>
      <div
        style={{
          display: "flex",
          alignItems: "flex-start",
          gap: "40px",
        }}
      >
        {/* Left column */}
        <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
          <Widget title="Username">
            <p>Adoft Hitless</p>
          </Widget>
          <Widget title="Email">
            <p>adoft.hitless@hcmut.edu.vn</p>
          </Widget>
          <Widget title="Joined At">
            <p>16/04/2021</p>
          </Widget>
          <Widget title="Total Points">
            <p>30,712</p>
          </Widget>
          <Widget title="Borrowed Books">
            <p>311</p>
          </Widget>
        </div>

        {/* Right column */}
        <div style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
          <Widget title="Contact Info">
            <div className="card" style={{ background: "var(--color-muted)" }}>
              123 đường ABC
            </div>
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Phone: 0190234569
            </div>
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Business Management
            </div>
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Fax: 019277455
            </div>
          </Widget>

          <Button
            label="Change"
            color="var(--color-primary)"
            roundness={8}
            onClick={() => alert("Change clicked")}
          />
          <Button
            label="Delete Account"
            color="var(--color-danger)"
            roundness={8}
            onClick={() => alert("Account deleted")}
          />
        </div>
      </div>
    </>
  );
}
