import { Calendar, Clock } from "lucide-react";
import { BookContributorForm } from "./components/BookContributorForm";

export function BookContributor() {
  const today = new Date();

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
        Book Details
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

      <BookContributorForm />
    </>
  );
}
