import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Button from "./button";
import { booksData } from "../../user/booksData"; 
import "./BookDetail.css";

type Book = {
  bookid: number;
  title: string;
  author?: string;
  publishedYear?: string;
  publisher?: string;
  availableCopy?: number;
  rarity?: string;
  genre?: string[];
  description?: string;
  image: string;
  addedDate?: string;
  time?: string;
};

export function BookDetail() {
  const { bookid } = useParams<{ bookid: string }>(); 
  const [book, setBook] = useState<Book | null>(null); 

  const currentDate = new Date();
  const formattedDate = currentDate.toLocaleDateString("en-GB"); 
  const formattedTime = currentDate.toLocaleTimeString("en-GB", {
    hour: "2-digit",
    minute: "2-digit",
  });

  useEffect(() => {
    if (bookid) {
      //  Simulate fetching from API using local mock data
      //    In a real app, this would be an API call like:
      /*
      fetch(`/api/books/${bookid}`)
        .then((res) => res.json())
        .then((data) => setBook(data));
      */

      //  For now, use mock data from booksData (used in catalog)
      const allBooks = Object.values(booksData).flat(); // combine all categories
      const foundBook = allBooks.find(
        (b) => b.bookid === Number(bookid)
      ) as Book | undefined;

      //  Apply fallback info for missing fields
      if (foundBook) {
        setBook({
          author: "Unknown Author",
          publishedYear: "N/A",
          publisher: "N/A",
          availableCopy: 10,
          rarity: "Common",
          genre: ["General"],
          description: "No detailed description available.",
          addedDate: formattedDate,
          time: formattedTime,
          ...foundBook, // merge with actual mock data
        });
      }
    }
  }, [bookid]);

  if (!book) {
    return <p>Loading book details...</p>;
  }

  return (
    <div className="book-detail-container">
      <h1 className="book-detail-title">Book Detail</h1>

      <div className="book-detail-date">
        <span>📅 {book.addedDate}</span>
        <span>🕘 {book.time}</span>
      </div>

      <div className="book-detail-content">
        <div className="book-left">
          <img
            src={book.image}
            alt={book.title}
            className="book-cover"
          />

          <Button className="btn-borrow">Borrow</Button>

          <Button className="btn-queue">Add to Borrow Queue</Button>
        </div>

        <div className="book-right">
          <div className="book-info-grid">
            <div className="book-info-box">
              <strong>Author:</strong> {book.author}
            </div>
            <div className="book-info-box">
              <strong>Published Year:</strong> {book.publishedYear}
            </div>
            <div className="book-info-box">
              <strong>Publisher:</strong> {book.publisher}
            </div>
            <div className="book-info-box">
              <strong>Available Copy:</strong> {book.availableCopy}
            </div>
            <div className="book-info-box">
              <strong>Rarity:</strong> {book.rarity}
            </div>
          </div>

          <div className="book-genre-description">
            <div className="book-genre">
              <h3>Genre</h3>
              <div className="genre-tags">
                {book.genre?.map((g) => (
                  <span key={g} className="genre-tag">{g}</span>
                ))}
              </div>
            </div>

            <div className="book-description">
              <h3>Description</h3>
              <p>{book.description}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
