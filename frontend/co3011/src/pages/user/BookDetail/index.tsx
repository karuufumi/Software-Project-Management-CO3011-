import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Button from "./button";
import "./BookDetail.css";
import { fetchBookById, requestBorrow } from "./bookDetailService";

type Book = {
  bookId: string;
  title: string;
  author: string;
  isbn: string;
  genre: string;
  availableCopies: number;
  description: string;
  addedDate?: string;
  time?: string;
};

export function BookDetail() {
  const { bookid } = useParams<{ bookid: string }>();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const [borrowMessage, setBorrowMessage] = useState("");

  useEffect(() => {
    if (!bookid) return;

    const now = new Date();
    const formattedDate = now.toLocaleDateString("en-GB");
    const formattedTime = now.toLocaleTimeString("en-GB", {
      hour: "2-digit",
      minute: "2-digit",
    });

    fetchBookById(bookid).then((data) => {
      setBook(data ? { addedDate: formattedDate, time: formattedTime, ...data } : null);
      setLoading(false);
    });
  }, [bookid]);

  const handleBorrow = async () => {
    const userId = localStorage.getItem("userId");

    if (!userId) {
      setBorrowMessage("⚠ You must be logged in to borrow.");
      return;
    }

    if (!book) {
      setBorrowMessage("❌ Book data unavailable.");
      return;
    }

    const response = await requestBorrow(userId, book.bookId);

    if (response?.success) {
      setBorrowMessage("✅ Borrow request submitted!");
    } else {
      setBorrowMessage("❌ " + (response?.message || "Failed to request borrow"));
    }
  };

  if (loading) return <p>Loading book details...</p>;
  if (!book) return <p>❌ Book not found.</p>;

  return (
    <div className="book-detail-container">
      <h1 className="book-detail-title">{book.title}</h1>

      <div className="book-detail-date">
        <span>📅 {book.addedDate}</span>
        <span>🕘 {book.time}</span>
      </div>

      <div className="book-detail-content">
        <div className="book-left">
          <div
            style={{
              width: 160,
              height: 220,
              backgroundColor: "#d9d9d9",
              borderRadius: 6,
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              padding: 10,
              fontWeight: 600,
              textAlign: "center",
            }}
          >
            {book.title}
          </div>

          <Button className="btn-borrow" onClick={handleBorrow}>
            Borrow
          </Button>

          {borrowMessage && (
            <p
              style={{
                marginTop: 10,
                color: borrowMessage.startsWith("❌") ? "red" : "green",
                fontWeight: 600,
              }}
            >
              {borrowMessage}
            </p>
          )}
        </div>

        <div className="book-right">
          <div className="book-info-grid">
            <div className="book-info-box"><strong>Author:</strong> {book.author}</div>
            <div className="book-info-box"><strong>ISBN:</strong> {book.isbn}</div>
            <div className="book-info-box"><strong>Genre:</strong> {book.genre}</div>
            <div className="book-info-box"><strong>Available Copies:</strong> {book.availableCopies}</div>
          </div>

          <div className="book-description">
            <h3>Description</h3>
            <p>{book.description}</p>
          </div>
        </div>
      </div>
    </div>
  );
}
