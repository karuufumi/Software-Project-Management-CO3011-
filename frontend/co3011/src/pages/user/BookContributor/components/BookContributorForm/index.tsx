import Button from "../../../../../components/button/button";
import Widget from "../../../../../components/widget/widget";
import { Plus } from "lucide-react";
import { BookPoint } from "../BookPoint";

export function BookContributorForm() {
  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "auto auto",
        gap: 30,
      }}
    >
      <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
        <Widget height={308} justifyContent="center">
          <input
            type="file"
            accept=".png,.jpeg"
            id="bookImage"
            style={{ display: "none" }}
          />
          <Plus
            style={{
              color: "white",
              backgroundColor: "var(--color-primary)",
              borderRadius: 7,
            }}
            onClick={() => document.getElementById("bookImage")?.click()}
          />

          <h3 style={{ fontWeight: 600 }}>Upload Book Cover</h3>
        </Widget>
        <BookPoint />
      </div>

      <div style={{ display: "flex", flexDirection: "column", gap: 30 }}>
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "auto auto",
            gap: 10,
          }}
        >
          <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
            <Widget title="Author:">
              <input
                type="text"
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>
            <Widget title="Published Year:">
              <input
                type="text"
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>
            <Widget title="Publisher:">
              <input
                type="text"
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>
            <Widget title="Available Copy:">
              <input
                type="text"
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>
          </div>
          <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
            <Widget title="Genre:" height={284}>
              <input
                type="text"
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>

            <Widget title="Description:" height={284}>
              <textarea
                style={{
                  width: "100%",
                  border: "1px solid rgba(0,0,0,.2)",
                  borderRadius: 10,
                }}
              />
            </Widget>
          </div>
        </div>
        <Button
          label="Upload"
          color="var(--color-primary)"
          roundness={8}
          onClick={() => alert("Change clicked")}
        />
      </div>
    </div>
  );
}
