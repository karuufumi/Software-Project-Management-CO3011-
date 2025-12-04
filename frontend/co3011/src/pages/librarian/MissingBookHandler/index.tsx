import React, { useState } from 'react';
import { Search, AlertCircle, Ban, DollarSign } from 'lucide-react';

interface Book {
  id: string;
  title: string;
  dueDate: string;
  price: number;
}

interface Member {
  id: string;
  name: string;
  email: string;
  lostCount: number;
  status: string;
  borrowedBooks: Book[];
}

interface MissingBookHandlerProps {
  // Define any props if needed
}

const MissingBookHandler: React.FC<MissingBookHandlerProps> = () => {
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [selectedMember, setSelectedMember] = useState<Member | null>(null);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [fineAmount, setFineAmount] = useState<string>('');
  const [showConfirmation, setShowConfirmation] = useState<boolean>(false);

  // Sample data
  const members: Member[] = [
    {
      id: 'M001',
      name: 'John Anderson',
      email: 'john.anderson@email.com',
      lostCount: 0,
      status: 'Active',
      borrowedBooks: [
        { id: 'B101', title: 'To Kill a Mockingbird', dueDate: '2025-11-20', price: 15.99 },
        { id: 'B102', title: 'The Great Gatsby', dueDate: '2025-11-25', price: 12.50 }
      ]
    },
    {
      id: 'M002',
      name: 'Sarah Mitchell',
      email: 'sarah.mitchell@email.com',
      lostCount: 2,
      status: 'Active',
      borrowedBooks: [
        { id: 'B201', title: 'Pride and Prejudice', dueDate: '2025-11-18', price: 14.99 }
      ]
    },
    {
      id: 'M003',
      name: 'Michael Chen',
      email: 'michael.chen@email.com',
      lostCount: 1,
      status: 'Active',
      borrowedBooks: [
        { id: 'B301', title: 'Computer Architecture', dueDate: '2025-11-22', price: 89.99 },
        { id: 'B302', title: 'Data Structures', dueDate: '2025-11-28', price: 75.00 }
      ]
    }
  ];

  const handleSearch = (): void => {
    const member = members.find(m => 
      m.id.toLowerCase().includes(searchQuery.toLowerCase()) ||
      m.name.toLowerCase().includes(searchQuery.toLowerCase())
    );
    setSelectedMember(member || null);
    setSelectedBook(null);
    setFineAmount('');
  };

  const handleBookSelect = (book: Book): void => {
    setSelectedBook(book);
    setFineAmount(book.price.toFixed(2));
  };

  const handleMarkAsLost = (): void => {
    if (selectedMember && selectedBook && fineAmount) {
      setShowConfirmation(true);
    }
  };

  const confirmMarkAsLost = (): void => {
    if (selectedMember && selectedBook) {
      alert(`Book marked as lost!\nMember: ${selectedMember.name}\nBook: ${selectedBook.title}\nFine: $${fineAmount}\nNew Lost Count: ${selectedMember.lostCount + 1}\n${selectedMember.lostCount + 1 >= 3 ? 'Account will be SUSPENDED!' : ''}`);
      setShowConfirmation(false);
      setSelectedMember(null);
      setSelectedBook(null);
      setFineAmount('');
      setSearchQuery('');
    }
  };

  const styles = {
    container: {
      flex: 1,
      overflowY: 'auto' as const,
      backgroundColor: '#f9fafb',
      padding: '2rem'
    },
    header: {
      marginBottom: '2rem'
    },
    title: {
      fontSize: '2rem',
      fontWeight: 'bold' as const,
      color: '#1f2937',
      marginBottom: '0.5rem'
    },
    subtitle: {
      color: '#6b7280',
      fontSize: '1rem'
    },
    card: {
      backgroundColor: 'white',
      borderRadius: '0.5rem',
      boxShadow: '0 1px 3px 0 rgba(0, 0, 0, 0.1)',
      padding: '1.5rem',
      marginBottom: '1.5rem'
    },
    cardTitle: {
      fontSize: '1.125rem',
      fontWeight: '600' as const,
      color: '#1f2937',
      marginBottom: '1rem'
    },
    searchContainer: {
      display: 'flex',
      gap: '0.75rem'
    },
    inputWrapper: {
      flex: 1,
      position: 'relative' as const
    },
    input: {
      width: '100%',
      padding: '0.75rem 1rem 0.75rem 2.5rem',
      border: '1px solid #d1d5db',
      borderRadius: '0.5rem',
      fontSize: '1rem',
      outline: 'none'
    },
    searchIcon: {
      position: 'absolute' as const,
      left: '0.75rem',
      top: '50%',
      transform: 'translateY(-50%)',
      color: '#9ca3af'
    },
    button: {
      padding: '0.75rem 1.5rem',
      backgroundColor: '#2563eb',
      color: 'white',
      border: 'none',
      borderRadius: '0.5rem',
      fontSize: '1rem',
      fontWeight: '500' as const,
      cursor: 'pointer'
    },
    memberInfo: {
      display: 'flex',
      justifyContent: 'space-between',
      marginBottom: '1.5rem'
    },
    infoLabel: {
      fontWeight: '500' as const,
      color: '#4b5563'
    },
    badge: {
      display: 'inline-flex',
      alignItems: 'center',
      gap: '0.5rem',
      padding: '0.5rem 1rem',
      borderRadius: '9999px',
      fontWeight: '500' as const
    },
    badgeGreen: {
      backgroundColor: '#dcfce7',
      color: '#166534'
    },
    badgeRed: {
      backgroundColor: '#fee2e2',
      color: '#991b1b'
    },
    warning: {
      color: '#dc2626',
      fontSize: '0.875rem',
      fontWeight: '500' as const,
      marginTop: '0.5rem',
      textAlign: 'right' as const
    },
    bookCard: {
      padding: '1rem',
      border: '2px solid #e5e7eb',
      borderRadius: '0.5rem',
      cursor: 'pointer',
      marginBottom: '0.75rem',
      transition: 'all 0.2s'
    },
    bookCardSelected: {
      border: '2px solid #2563eb',
      backgroundColor: '#eff6ff'
    },
    bookInfo: {
      display: 'flex',
      justifyContent: 'space-between',
      alignItems: 'center'
    },
    bookTitle: {
      fontWeight: '500' as const,
      color: '#1f2937'
    },
    bookId: {
      fontSize: '0.875rem',
      color: '#6b7280'
    },
    bookPrice: {
      fontSize: '1.125rem',
      fontWeight: '600' as const,
      color: '#1f2937',
      textAlign: 'right' as const
    },
    alertBox: {
      backgroundColor: '#fef3c7',
      border: '1px solid #fde68a',
      borderRadius: '0.5rem',
      padding: '1rem',
      marginBottom: '1.5rem',
      display: 'flex',
      gap: '0.75rem'
    },
    alertText: {
      fontWeight: '500' as const,
      color: '#92400e'
    },
    label: {
      display: 'block',
      fontSize: '0.875rem',
      fontWeight: '500' as const,
      color: '#374151',
      marginBottom: '0.5rem'
    },
    dangerButton: {
      width: '100%',
      padding: '0.75rem',
      backgroundColor: '#dc2626',
      color: 'white',
      border: 'none',
      borderRadius: '0.5rem',
      fontSize: '1rem',
      fontWeight: '500' as const,
      cursor: 'pointer'
    },
    modal: {
      position: 'fixed' as const,
      inset: 0,
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      zIndex: 50
    },
    modalContent: {
      backgroundColor: 'white',
      borderRadius: '0.5rem',
      boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.1)',
      padding: '1.5rem',
      maxWidth: '28rem',
      width: '100%',
      margin: '1rem'
    },
    modalHeader: {
      display: 'flex',
      alignItems: 'center',
      gap: '0.75rem',
      marginBottom: '1rem'
    },
    modalIconBg: {
      width: '3rem',
      height: '3rem',
      borderRadius: '9999px',
      backgroundColor: '#fee2e2',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center'
    },
    modalTitle: {
      fontSize: '1.25rem',
      fontWeight: 'bold' as const,
      color: '#1f2937'
    },
    modalInfo: {
      marginBottom: '1.5rem'
    },
    modalInfoItem: {
      color: '#374151',
      marginBottom: '0.75rem'
    },
    suspendWarning: {
      backgroundColor: '#fef2f2',
      border: '2px solid #fecaca',
      borderRadius: '0.5rem',
      padding: '1rem',
      marginTop: '1rem'
    },
    suspendTitle: {
      display: 'flex',
      alignItems: 'center',
      gap: '0.5rem',
      fontWeight: 'bold' as const,
      color: '#991b1b'
    },
    suspendText: {
      fontSize: '0.875rem',
      color: '#b91c1c',
      marginTop: '0.25rem'
    },
    modalButtons: {
      display: 'flex',
      gap: '0.75rem'
    },
    cancelButton: {
      flex: 1,
      padding: '0.75rem',
      border: '1px solid #d1d5db',
      color: '#374151',
      backgroundColor: 'white',
      borderRadius: '0.5rem',
      fontSize: '1rem',
      fontWeight: '500' as const,
      cursor: 'pointer'
    },
    confirmButton: {
      flex: 1,
      padding: '0.75rem',
      backgroundColor: '#dc2626',
      color: 'white',
      border: 'none',
      borderRadius: '0.5rem',
      fontSize: '1rem',
      fontWeight: '500' as const,
      cursor: 'pointer'
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <h2 style={styles.title}>Missing Book Management</h2>
        <p style={styles.subtitle}>Process lost books and apply fines to member accounts</p>
      </div>

      {/* Search Section */}
      <div style={styles.card}>
        <h3 style={styles.cardTitle}>Search Member</h3>
        <div style={styles.searchContainer}>
          <div style={styles.inputWrapper}>
            <Search style={styles.searchIcon} size={20} />
            <input
              type="text"
              placeholder="Enter member ID or name..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
              style={styles.input}
            />
          </div>
          <button onClick={handleSearch} style={styles.button}>
            Search
          </button>
        </div>
      </div>

      {/* Member Information */}
      {selectedMember && (
        <div style={styles.card}>
          <div style={styles.memberInfo}>
            <div>
              <h3 style={styles.cardTitle}>Member Information</h3>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>ID:</span> {selectedMember.id}
              </p>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>Name:</span> {selectedMember.name}
              </p>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>Email:</span> {selectedMember.email}
              </p>
            </div>
            <div>
              <div style={{
                ...styles.badge,
                ...(selectedMember.lostCount >= 2 ? styles.badgeRed : styles.badgeGreen)
              }}>
                <AlertCircle size={16} />
                <span>Lost Books: {selectedMember.lostCount}/3</span>
              </div>
              {selectedMember.lostCount >= 2 && (
                <p style={styles.warning}>⚠️ Warning: One more loss will suspend account!</p>
              )}
            </div>
          </div>

          {/* Borrowed Books */}
          <div>
            <h4 style={styles.cardTitle}>Currently Borrowed Books</h4>
            {selectedMember.borrowedBooks.map((book) => (
              <div
                key={book.id}
                onClick={() => handleBookSelect(book)}
                style={{
                  ...styles.bookCard,
                  ...(selectedBook?.id === book.id ? styles.bookCardSelected : {})
                }}
              >
                <div style={styles.bookInfo}>
                  <div>
                    <p style={styles.bookTitle}>{book.title}</p>
                    <p style={styles.bookId}>Book ID: {book.id}</p>
                  </div>
                  <div>
                    <p style={styles.bookId}>Due: {book.dueDate}</p>
                    <p style={styles.bookPrice}>${book.price}</p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Fine Processing */}
      {selectedBook && (
        <div style={styles.card}>
          <h3 style={styles.cardTitle}>Process Lost Book</h3>
          
          <div style={styles.alertBox}>
            <AlertCircle color="#92400e" size={20} style={{ flexShrink: 0, marginTop: '2px' }} />
            <div>
              <p style={styles.alertText}>Selected Book: {selectedBook.title}</p>
              <p style={{ fontSize: '0.875rem', color: '#78350f', marginTop: '0.25rem' }}>
                Book ID: {selectedBook.id} • Recommended Fine: ${selectedBook.price}
              </p>
            </div>
          </div>

          <div style={{ marginBottom: '1.5rem' }}>
            <label style={styles.label}>Fine Amount ($)</label>
            <div style={styles.inputWrapper}>
              <DollarSign style={styles.searchIcon} size={20} />
              <input
                type="number"
                step="0.01"
                value={fineAmount}
                onChange={(e) => setFineAmount(e.target.value)}
                style={styles.input}
                placeholder="Enter fine amount"
              />
            </div>
          </div>

          <button
            onClick={handleMarkAsLost}
            disabled={!fineAmount}
            style={{
              ...styles.dangerButton,
              opacity: !fineAmount ? 0.5 : 1,
              cursor: !fineAmount ? 'not-allowed' : 'pointer'
            }}
          >
            Mark as Lost & Apply Fine
          </button>
        </div>
      )}

      {/* Confirmation Modal */}
      {showConfirmation && selectedMember && selectedBook && (
        <div style={styles.modal}>
          <div style={styles.modalContent}>
            <div style={styles.modalHeader}>
              <div style={styles.modalIconBg}>
                <AlertCircle color="#dc2626" size={24} />
              </div>
              <h3 style={styles.modalTitle}>Confirm Lost Book</h3>
            </div>
            
            <div style={styles.modalInfo}>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>Member:</span> {selectedMember.name}
              </p>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>Book:</span> {selectedBook.title}
              </p>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>Fine Amount:</span> ${fineAmount}
              </p>
              <p style={styles.modalInfoItem}>
                <span style={styles.infoLabel}>New Lost Count:</span> {selectedMember.lostCount + 1}/3
              </p>
              
              {selectedMember.lostCount + 1 >= 3 && (
                <div style={styles.suspendWarning}>
                  <div style={styles.suspendTitle}>
                    <Ban color="#991b1b" size={20} />
                    <span>Account will be SUSPENDED</span>
                  </div>
                  <p style={styles.suspendText}>
                    This member has reached the maximum limit of lost books.
                  </p>
                </div>
              )}
            </div>

            <div style={styles.modalButtons}>
              <button onClick={() => setShowConfirmation(false)} style={styles.cancelButton}>
                Cancel
              </button>
              <button onClick={confirmMarkAsLost} style={styles.confirmButton}>
                Confirm
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MissingBookHandler;