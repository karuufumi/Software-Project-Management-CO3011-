// src/pages/user/Dashboard/components/UserDashboardGeneralInfo/index.tsx
import Widget from "../../../../../components/widget/widget";

export function UserDashboardGeneralInfo() {
  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>

      <h3 style={{ fontWeight: 600 }}>Library Stats</h3>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>📚 52,400 Books Available</h2>
      </Widget>

      <Widget>
        <h2 style={{ fontWeight: 600 }}>👥 21,875 Registered Members</h2>
      </Widget>

      <h3 style={{ fontWeight: 600 }}>Contact Information</h3>

      <Widget>
        <h2 style={{ fontWeight: 600 }}>Address: A2 block, 268 Ly Thuong Kiet, District 10</h2>
      </Widget>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>Phone: 028 3864 7256</h2>
      </Widget>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>Email: thuvien@hcmut.edu.vn</h2>
      </Widget>
    </div>
  );
}

export default UserDashboardGeneralInfo;
