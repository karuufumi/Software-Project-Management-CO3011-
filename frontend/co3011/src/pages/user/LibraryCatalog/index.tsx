import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Plus } from "lucide-react";
import { BookCarousel } from "./components/BookCarousel";
import { fetchAllBooks } from "./bookService";
import paths from "../../../routes/paths";

interface Book {
  bookId: string;
  title: string;
  genre: string;
}

export function UserLibraryCatalog() {
  const [books, setBooks] = useState<Book[]>([]);

  useEffect(() => {
    fetchAllBooks().then((data) => {
      console.log("📚 Books from backend:", data);
      setBooks(data);
    });
  }, []);

  // Group books by genre dynamically
  const groupedBooks = books.reduce((acc, book) => {
    const genreKey = book.genre?.toLowerCase() || "others";
    if (!acc[genreKey]) acc[genreKey] = [];
    acc[genreKey].push(book);
    return acc;
  }, {} as Record<string, Book[]>);

  return (
    <>
      <div
        style={{
          marginBottom: 20,
          paddingBottom: 5,
          display: "flex",
          justifyContent: "space-between",
          borderBottom: "1px solid rgba(0,0,0,0.2)",
        }}
      >
        <h2 style={{ fontWeight: 600 }}>Library Catalog</h2>
        <Link
          to={paths.USER.BOOK_CONTRIBUTE}
          style={{ display: "flex", alignItems: "center", gap: 2 }}
        >
          Contribute Book
          <Plus
            style={{
              color: "white",
              backgroundColor: "var(--color-primary)",
              borderRadius: 7,
            }}
          />
        </Link>
      </div>

      <h3 style={{ fontWeight: 600, marginBottom: 10 }}>
        Currently Available
      </h3>

      {Object.keys(groupedBooks).map((genre) => (
        <BookCarousel
          key={genre}
          title={genre.replace(/^\w/, (c) => c.toUpperCase())} // Capitalize
          books={groupedBooks[genre]}
        />
      ))}
    </>
  );
}
