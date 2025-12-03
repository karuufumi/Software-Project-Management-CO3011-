import { AnimatePresence } from "framer-motion";
import { Calendar, Clock } from "lucide-react";

export function AdminDashboard() {
  const today = new Date();
  

  return (
    <>
      <AnimatePresence>
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
        </AnimatePresence>

    </>
  );

}
