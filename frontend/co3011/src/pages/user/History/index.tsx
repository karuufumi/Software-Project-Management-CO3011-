import { Calendar, Clock } from "lucide-react";
import { useState } from "react";
import { Tab, TabList, TabPanel, Tabs } from "react-tabs";
import "react-tabs/style/react-tabs.css";
import styles from "./History.module.css";
import { BookQueue } from "./components/BookQueue";
import { PointHistory } from "./components/Point";

export function History() {
  const today = new Date();
  const [tabSelected, setTab] = useState(0);

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
        History
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
      <Tabs selectedIndex={tabSelected} onSelect={(idx) => setTab(idx)}>
        <TabList style={{ border: "none" }}>
          <Tab
            style={{ border: "none" }}
            className={{
              [styles.selected_tabs]: tabSelected === 0,
              "react-tabs__tab": tabSelected !== 0,
            }}
          >
            Book Queue
          </Tab>
          <Tab
            style={{ border: "none" }}
            className={{
              [styles.selected_tabs]: tabSelected === 1,
              "react-tabs__tab": tabSelected !== 1,
            }}
          >
            Points
          </Tab>
        </TabList>
        <TabPanel>
          <BookQueue />
        </TabPanel>
        <TabPanel>
          <PointHistory />
        </TabPanel>
      </Tabs>
    </>
  );
}
