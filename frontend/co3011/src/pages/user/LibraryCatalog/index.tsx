// index.tsx
import { Link } from "react-router-dom";
import { Plus } from "lucide-react";
import { BookCarousel } from "./components/BookCarousel";
import { booksData } from "../../user/booksData";
import paths from "../../../routes/paths";

export function UserLibraryCatalog() {
  return (
    <>
      <div style={{ marginBottom: 20, paddingBottom: 5, display: "flex", justifyContent: "space-between", borderBottom: "1px solid rgba(0,0,0,0.2)" }}>
        <h2 style={{ fontWeight: 600 }}>Library Catalog</h2>
        <Link to={paths.USER.BOOK_CONTRIBUTE} style={{ display: "flex", alignItems: "center", gap: 2 }}>
          Contribute Book
          <Plus style={{ color: "white", backgroundColor: "var(--color-primary)", borderRadius: 7 }} />
        </Link>
      </div>

      <h3 style={{ fontWeight: 600, marginBottom: 10 }}>Currently Available</h3>

      <BookCarousel title="Academic Books" books={booksData.academic} />
      <BookCarousel title="General Readings" books={booksData.general} />
      <BookCarousel title="Research Papers & Journals" books={booksData.research} />
      <BookCarousel title="Engineering & Applied Sciences" books={booksData.science} />
    </>
  );
}
