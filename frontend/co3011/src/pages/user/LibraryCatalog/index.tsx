import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Plus, Search } from "lucide-react";
import paths from "../../../routes/paths";
import "./LibraryCatalog.css";

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
};

export function UserLibraryCatalog() {
  const navigate = useNavigate();
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedGenre, setSelectedGenre] = useState<string>("all");
  const [availableOnly, setAvailableOnly] = useState(false);

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        setLoading(true);
        const response = await fetch(
          "https://lms-server-nc7w.onrender.com/api/Book",
          {
            method: "GET",
            headers: {
              "accept": "text/plain",
            },
          }
        );

        if (!response.ok) {
          throw new Error("Failed to fetch books");
        }

        const data = await response.json();
        
        if (data.success && data.data) {
          setBooks(data.data);
        } else {
          setError("No books found");
        }
      } catch (err) {
        setError("Error loading books");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, []);

  // Get unique genres
  const genres = Array.from(new Set(books.map(book => book.genre || "General")));

  // Filter books
  const filteredBooks = books.filter(book => {
    const matchesSearch = 
      book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
      (book.author || "").toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesGenre = 
      selectedGenre === "all" || book.genre === selectedGenre;
    
    const matchesAvailability = 
      !availableOnly || (book.availableCopies || 0) > 0;

    return matchesSearch && matchesGenre && matchesAvailability;
  });

  const handleBookClick = (bookId: string) => {
    navigate(`${paths.USER.LIBRARY_CATALOG}/book/${bookId}`);
  };

  if (loading) {
    return (
      <div className="catalog-loading">
        <div className="spinner"></div>
        <p>Loading library catalog...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="catalog-error">
        <p>{error}</p>
        <button onClick={() => window.location.reload()}>Retry</button>
      </div>
    );
  }

  return (
    <div className="library-catalog">
      {/* Header */}
      <div className="catalog-header">
        <h2>Library Catalog</h2>
        <Link to={paths.USER.BOOK_CONTRIBUTE} className="contribute-btn">
          <Plus size={20} />
          Contribute Book
        </Link>
      </div>

      {/* Search and Filters */}
      <div className="catalog-filters">
        <div className="search-box">
          <Search size={20} />
          <input
            type="text"
            placeholder="Search by title or author..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        <select
          value={selectedGenre}
          onChange={(e) => setSelectedGenre(e.target.value)}
          className="genre-filter"
        >
          <option value="all">All Genres</option>
          {genres.map(genre => (
            <option key={genre} value={genre}>{genre}</option>
          ))}
        </select>

        <label className="checkbox-filter">
          <input
            type="checkbox"
            checked={availableOnly}
            onChange={(e) => setAvailableOnly(e.target.checked)}
          />
          Available Only
        </label>
      </div>

      {/* Results Count */}
      <div className="results-info">
        <p>
          Showing <strong>{filteredBooks.length}</strong> of <strong>{books.length}</strong> books
        </p>
      </div>

      {/* Books Grid */}
      <div className="books-grid">
        {filteredBooks.map(book => (
          <div
            key={book.bookId}
            className="book-card"
            onClick={() => handleBookClick(book.bookId)}
          >
            <div className="book-cover-placeholder">
              <span>📚</span>
            </div>
            
            <div className="book-info">
              <h3 className="book-title">{book.title}</h3>
              <p className="book-author">{book.author || "Unknown Author"}</p>
              
              <div className="book-meta">
                <span className="book-genre">{book.genre || "General"}</span>
                <span className={`book-availability ${(book.availableCopies || 0) > 0 ? 'available' : 'unavailable'}`}>
                  {(book.availableCopies || 0) > 0 
                    ? `${book.availableCopies} available` 
                    : 'Unavailable'}
                </span>
              </div>

              {book.tier && (
                <div className="book-tier">Tier {book.tier}</div>
              )}
            </div>
          </div>
        ))}
      </div>

      {/* Empty State */}
      {filteredBooks.length === 0 && (
        <div className="empty-state">
          <p>No books found matching your criteria.</p>
          {searchTerm && (
            <button onClick={() => setSearchTerm("")}>Clear search</button>
          )}
        </div>
      )}
    </div>
  );
}