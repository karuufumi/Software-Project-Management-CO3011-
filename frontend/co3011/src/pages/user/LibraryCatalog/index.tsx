import { Link } from "react-router-dom";
import { Plus } from "lucide-react";
import { AcademicBooks } from "./components/AcademicBooks";
import { GeneralReadings } from "./components/GeneralReadings";
import { Research } from "./components/Research";
import { Science } from "./components/Science";
import paths from "../../../routes/paths";

export function UserLibraryCatalog() {
  return (
    <>
      <div
        style={{
          marginBottom: 20,
          paddingBottom: 5,
          display: "flex",
          justifyContent: "space-between",
          borderBottom: "1px solid rgba(0, 0, 0, 0.2)",
        }}
      >
        <h2 style={{ fontWeight: 600 }}>Library Catalog</h2>
        <Link
          to={paths.USER.BOOK_CONTRIBUTE}
          style={{
            display: "flex",
            alignItems: "center",
            gap: 2,
          }}
        >
          Contribute Book{" "}
          <Plus
            style={{
              color: "white",
              backgroundColor: "var(--color-primary)",
              borderRadius: 7,
            }}
          />
        </Link>
      </div>

      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        currently available
      </h3>

      <AcademicBooks />
      <GeneralReadings />
      <Research />
      <Science />
    </>
  );
}
