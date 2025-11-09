import viteLogo from "/vite.svg";
import Widget from "../../../../../components/widget/widget";

export function UserDashboardGeneralInfo() {
  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>
      <Widget>
        <img src={viteLogo} className="logo" alt="Vite logo" />
      </Widget>

      <h3 style={{ fontWeight: 600, textTransform: "capitalize" }}>
        total register member
      </h3>
      <Widget>
        <h2 style={{ fontWeight: 600, textTransform: "capitalize" }}>
          69,420 members
        </h2>
      </Widget>

      <h3 style={{ fontWeight: 600, textTransform: "capitalize" }}>
        contact us
      </h3>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>
          Address: A2 block, 268 Ly Thuong Kiet, Phuong 14, Quan 10
        </h2>
      </Widget>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>Phone number: 028 3864 7256</h2>
      </Widget>
      <Widget>
        <h2 style={{ fontWeight: 600 }}>Email: thuvien@hcmut.edu.vn</h2>
      </Widget>
    </div>
  );
}
