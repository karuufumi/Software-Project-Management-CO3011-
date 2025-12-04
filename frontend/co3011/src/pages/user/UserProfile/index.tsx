import Widget from "../../../components/widget/widget";
import Button from "../../../components/button/button";
import { useEffect, useState } from "react";

export default function UserProfile() {
  const [username, setUsername] = useState("Unknown");
  const [email, setEmail] = useState("Unknown");
  const [role, setRole] = useState("User");
  const [joined, setJoined] = useState("N/A");

  useEffect(() => {
    const storedUsername = localStorage.getItem("username");
    const storedEmail = localStorage.getItem("email");
    const storedRole = localStorage.getItem("role");
    const storedJoinDate = localStorage.getItem("createdAt"); // Optional if you add this later

    if (storedUsername) setUsername(storedUsername);
    if (storedEmail) setEmail(storedEmail);
    if (storedRole) setRole(storedRole);
    if (storedJoinDate) setJoined(new Date(storedJoinDate).toLocaleDateString());
  }, []);

  return (
    <>
      <h2 style={{ fontWeight: 600, marginBottom: "20px" }}>User Profile</h2>

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
            <p>{username}</p>
          </Widget>
          <Widget title="Email">
            <p>{email}</p>
          </Widget>
          <Widget title="Role">
            <p style={{ textTransform: "capitalize" }}>{role}</p>
          </Widget>
          <Widget title="Joined At">
            <p>{joined}</p>
          </Widget>
        </div>

        {/* Right column */}
        <div style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
          <Widget title="Contact Info">
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Address: Updating...
            </div>
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Phone: Updating...
            </div>
            <div className="card" style={{ background: "var(--color-muted)" }}>
              Department: Updating...
            </div>
          </Widget>

          <Button
            label="Edit Profile"
            color="var(--color-primary)"
            roundness={8}
            onClick={() => alert("Profile editing coming soon!")}
          />
          <Button
            label="Delete Account"
            color="var(--color-danger)"
            roundness={8}
            onClick={() => alert("Delete Feature will be integrated later")}
          />
        </div>
      </div>
    </>
  );
}
