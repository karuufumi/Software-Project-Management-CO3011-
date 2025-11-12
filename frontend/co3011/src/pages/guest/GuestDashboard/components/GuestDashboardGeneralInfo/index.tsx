import bkLogo from "/bk-logo.png"; 
import Widget from "../../../../../components/widget/widget";

export function GuestDashboardGeneralInfo() {
  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 24 }}>
      {/* Big BK logo + total members */}
      <Widget alignItems="center" textAlign="center">
        <img
          src={bkLogo}
          alt="BK logo"
          style={{ width: 160, height: 160, objectFit: "contain", marginBottom: 8 }}
        />
        <div style={{ color: "rgba(0,0,0,0.6)", fontSize: 13, marginBottom: 8 }}>
          Total Registered Members
        </div>

        {/* pill-style count */}
        <div
          style={{
            marginTop: 6,
            padding: "10px 22px",
            borderRadius: 999,
            background: "#fff",
            boxShadow: "0 6px 14px rgba(0,0,0,0.06)",
            fontWeight: 700,
            fontSize: 18,
            alignSelf: "center",
          }}
        >
          69,420 Members
        </div>
      </Widget>

      {/* Contact title */}
      <div style={{ fontWeight: 600, textTransform: "capitalize", color: "var(--color-text-primary)" }}>
        Contact Us
      </div>

      {/* Contact cards */}
      <Widget alignItems="flex-start" textAlign="center">
        <div
          style={{
            width: "100%",
            borderRadius: 12,
            padding: 12,
            background: "#fff",
            boxShadow: "none",
            textAlign: "center",
            fontWeight: 700,
          }}
        >
          Address: A2 block, 268 Ly Thuong Kiet, Phuong 14, Quan 10
        </div>
      </Widget>

      <Widget alignItems="flex-start" textAlign="center">
        <div
          style={{
            width: "100%",
            borderRadius: 12,
            padding: 12,
            background: "#fff",
            boxShadow: "none",
            textAlign: "center",
            fontWeight: 700,
          }}
        >
          Phone number: 028 3864 7256
        </div>
      </Widget>

      <Widget alignItems="flex-start" textAlign="center">
        <div
          style={{
            width: "100%",
            borderRadius: 12,
            padding: 12,
            background: "#fff",
            boxShadow: "none",
            textAlign: "center",
            fontWeight: 700,
          }}
        >
          Email: thuvien@hcmut.edu.vn
        </div>
      </Widget>
    </div>
  );
}

export default GuestDashboardGeneralInfo;
