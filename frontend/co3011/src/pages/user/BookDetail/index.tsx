import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Button from "./button";
import "./BookDetail.css";

type Book = {
  bookId: string;
  title: string;
  author?: string;
  tier?: number;
  isbn?: string;
  genre?: string;
  publishedDate?: string;
  totalCopies?: number;
  availableCopies?: number;
  description?: string;
  borrowRecords?: unknown;
};

export function BookDetail() {
  const { bookid } = useParams<{ bookid: string }>(); 
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const currentDate = new Date();
  const formattedDate = currentDate.toLocaleDateString("en-GB"); 
  const formattedTime = currentDate.toLocaleTimeString("en-GB", {
    hour: "2-digit",
    minute: "2-digit",
  });

  useEffect(() => {
    const fetchBookDetails = async () => {
      if (!bookid) return;

      try {
        setLoading(true);
        const response = await fetch(
          `https://lms-server-nc7w.onrender.com/api/Book/${bookid}`,
          {
            method: "GET",
            headers: {
              "accept": "text/plain",
            },
          }
        );

        if (!response.ok) {
          throw new Error("Failed to fetch book details");
        }

        const data = await response.json();
        
        if (data.success && data.data) {
          setBook(data.data);
        } else {
          setError("Book not found");
        }
      } catch (err) {
        setError("Error loading book details");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchBookDetails();
  }, [bookid]);

  if (loading) {
    return <p>Loading book details...</p>;
  }

  if (error || !book) {
    return <p>{error || "Book not found"}</p>;
  }

  return (
    <div className="book-detail-container">
      <h1 className="book-detail-title">Book Detail</h1>

      <div className="book-detail-date">
        <span>📅 {formattedDate}</span>
        <span>🕘 {formattedTime}</span>
      </div>

      <div className="book-detail-content">
        <div className="book-left">
          <img
            src="/vite.svg"
            alt={book.title}
            className="book-cover"
          />

          <Button className="btn-borrow">Borrow</Button>
          <Button className="btn-queue">Add to Borrow Queue</Button>
        </div>

        <div className="book-right">
          <div className="book-info-grid">
            <div className="book-info-box">
              <strong>Title:</strong> {book.title}
            </div>
            <div className="book-info-box">
              <strong>Author:</strong> {book.author || "Unknown"}
            </div>
            <div className="book-info-box">
              <strong>ISBN:</strong> {book.isbn || "N/A"}
            </div>
            <div className="book-info-box">
              <strong>Published Date:</strong> {book.publishedDate ? new Date(book.publishedDate).toLocaleDateString() : "N/A"}
            </div>
            <div className="book-info-box">
              <strong>Available Copies:</strong> {book.availableCopies || 0}
            </div>
            <div className="book-info-box">
              <strong>Total Copies:</strong> {book.totalCopies || 0}
            </div>
            <div className="book-info-box">
              <strong>Tier:</strong> {book.tier || "N/A"}
            </div>
          </div>

          <div className="book-genre-description">
            <div className="book-genre">
              <h3>Genre</h3>
              <div className="genre-tags">
                <span className="genre-tag">{book.genre || "General"}</span>
              </div>
            </div>

            <div className="book-description">
              <h3>Description</h3>
              <p>{book.description || "No description available."}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}