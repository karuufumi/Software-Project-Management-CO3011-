import { UserDashboardData } from "./components/UserDashboardData";
import { UserDashboardGeneralInfo } from "./components/UserDashboardGeneralInfo";

export function MemberDashboard() {
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
        Dashboard
      </h2>
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "1fr 1fr",
          gap: 30,
        }}
      >
        <UserDashboardData />
        <UserDashboardGeneralInfo />
      </div>
    </>
  );
}
