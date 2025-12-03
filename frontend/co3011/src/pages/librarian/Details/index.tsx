import React, { useState } from 'react';
import { LayoutDashboard, History, BookOpen, User, Minus, Plus, LogOut, ChevronDown } from 'lucide-react';

// --- 1. TYPE DEFINITIONS ---

type ActivePage = 'dashboard' | 'book-management' | 'history';

interface NavMenuItem {
  id: ActivePage;
  label: string;
  icon: React.ElementType;
}

interface BookDetail {
  title: string;
  author: string;
  publishedYear: string;
  publisher: string;
  genres: string[];
  description: string;
  availableCopyCount: number;
  rarity: string;
  imageUrl: string;
  requestQueue: number[];
  currentDate: string;
  currentTime: string;
}

interface UserProfile {
  name: string;
  role: string;
  email: string;
}

// --- 2. MOCK DATA ---

const MOCK_BOOK_DATA: BookDetail = {
  title: "My Struggle",
  author: "Aldof Hitler",
  publishedYear: "18/07/1925",
  publisher: "Franz Eher Nachfolger",
  genres: ["Political Theory", "Autobiography", "Propaganda"],
  description: "A political manifesto combining autobiography with Hitler's ideology and future plans for Germany.",
  availableCopyCount: 92,
  rarity: "Specialized",
  // Placeholder URL matching the dark background/red title aesthetic
  imageUrl: "https://placehold.co/150x220/8b0000/ffffff?text=My+Struggle",
  requestQueue: [36, 69, 3005, 1964, 18, 8],
  currentDate: "30/9/2025",
  currentTime: "09:00",
};

const MOCK_USER_PROFILE: UserProfile = {
  name: "Mr. Librarian",
  role: "Librarian",
  email: "librian@hcmut.edu.vn",
};

const NAV_ITEMS: NavMenuItem[] = [
  { id: 'dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { id: 'book-management', label: 'Book Management', icon: BookOpen },
  { id: 'history', label: 'History', icon: History },
];

// --- 3. HELPER COMPONENTS ---

/** Renders the navigation links in the sidebar. */
const SidebarNav: React.FC<{ active: ActivePage; onSelect: (id: ActivePage) => void }> = ({ active, onSelect }) => (
  <nav className="flex flex-col space-y-2 mt-8">
    {NAV_ITEMS.map((item) => {
      const isActive = active === item.id;
      const Icon = item.icon;
      return (
        <button
          key={item.id}
          onClick={() => onSelect(item.id)}
          className={`flex items-center space-x-3 p-3 rounded-xl transition-all duration-200 text-sm font-medium ${
            isActive
              ? 'bg-blue-500 text-white shadow-lg shadow-blue-200'
              : 'text-gray-700 hover:bg-gray-100'
          }`}
        >
          <Icon className="w-5 h-5" />
          <span>{item.label}</span>
        </button>
      );
    })}
  </nav>
);

/** Renders the book property fields (Author, Publisher, Rarity). */
const BookProperty: React.FC<{ label: string; value: string; isRarity?: boolean }> = ({ label, value, isRarity = false }) => (
  <div className="p-3 bg-gray-50 rounded-xl shadow-inner border border-gray-100">
    <p className="text-sm font-semibold text-gray-500 mb-0.5">{label}:</p>
    <p className={`text-gray-800 ${isRarity ? 'font-bold' : ''}`}>{value}</p>
  </div>
);

/** Renders the core Book Detail content area. */
const BookDetailContent: React.FC<{ book: BookDetail }> = ({ book }) => {
  const [copies, setCopies] = useState(book.availableCopyCount);

  const handleCopyChange = (amount: 1 | -1) => {
    setCopies(prev => Math.max(0, prev + amount));
  };

  const RarityDisplay = () => (
    <div className="col-span-1 md:col-span-2 space-y-4">
      <BookProperty label="Rarity" value={book.rarity} isRarity={true} />
    </div>
  );

  return (
    <div className="p-6 md:p-10">
      <h2 className="text-2xl font-bold text-gray-800 mb-6">Book Detail</h2>
      
      {/* Date and Time Header */}
      <div className="flex items-center text-sm text-gray-500 mb-8 border-b pb-4 space-x-4">
        <p className="flex items-center space-x-1">
          <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"></path></svg>
          <span>{book.currentDate}</span>
        </p>
        <p className="flex items-center space-x-1">
          <svg className="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
          <span>{book.currentTime}</span>
        </p>
      </div>

      {/* Detail Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-4 xl:grid-cols-5 gap-8">
        
        {/* Column 1: Image, Action, Queue (lg:col-span-1) */}
        <div className="lg:col-span-1 space-y-6">
          <div className="flex justify-center">
            <div className="w-[150px] h-[220px] rounded-lg overflow-hidden shadow-2xl relative">
              <img src={book.imageUrl} alt={book.title} className="w-full h-full object-cover" onError={(e) => { e.currentTarget.src = "https://placehold.co/150x220/333333/ffffff?text=Book+Cover"; }}/>
            </div>
          </div>
          
          <button className="w-full bg-red-600 text-white font-semibold py-3 rounded-xl shadow-md hover:bg-red-700 transition-colors flex items-center justify-center space-x-2">
            Remove From Catalogue
          </button>
          
          {/* Request Queue */}
          <div className="p-4 bg-gray-50 rounded-2xl shadow-inner border border-gray-100">
            <h3 className="font-bold text-gray-800 mb-3 border-b pb-2">Request queue</h3>
            <div className="grid grid-cols-3 gap-2 text-center">
              {book.requestQueue.map((id, index) => (
                <span key={index} className="text-sm font-medium text-gray-700 bg-white p-2 rounded-lg shadow-sm border">
                  {String(id).padStart(2, '0')}
                </span>
              ))}
            </div>
          </div>
        </div>

        {/* Column 2 & 3: Properties (lg:col-span-2) */}
        <div className="lg:col-span-2 space-y-6">
          <BookProperty label="Author" value={book.author} />
          <BookProperty label="Published Year" value={book.publishedYear} />
          <BookProperty label="Publisher" value={book.publisher} />
          
          {/* Available Copy Counter */}
          <div className="p-3 bg-gray-50 rounded-xl shadow-inner border border-gray-100 flex flex-col">
            <p className="text-sm font-semibold text-gray-500 mb-2">Available Copy</p>
            <div className="flex items-center space-x-2">
              <button 
                onClick={() => handleCopyChange(-1)}
                className="bg-blue-500 text-white w-10 h-10 rounded-xl text-2xl font-bold hover:bg-blue-600 transition-colors shadow-md flex items-center justify-center"
              >
                <Minus className="w-5 h-5" />
              </button>
              <span className="text-3xl font-extrabold w-16 text-center text-gray-800">{copies}</span>
              <button 
                onClick={() => handleCopyChange(1)}
                className="bg-blue-500 text-white w-10 h-10 rounded-xl text-2xl font-bold hover:bg-blue-600 transition-colors shadow-md flex items-center justify-center"
              >
                <Plus className="w-5 h-5" />
              </button>
            </div>
          </div>
          <RarityDisplay />
        </div>

        {/* Column 4: Genre and Description (lg:col-span-1, xl:col-span-2) */}
        <div className="lg:col-span-1 xl:col-span-2 space-y-6">
          {/* Genre Tags */}
          <div className="p-4 bg-gray-50 rounded-2xl shadow-inner border border-gray-100">
            <h3 className="text-sm font-bold text-gray-600 mb-3">Genre</h3>
            <div className="flex flex-wrap gap-2">
              {book.genres.map((genre, index) => (
                <span 
                  key={index} 
                  className="bg-gray-200 text-gray-700 px-4 py-2 rounded-full text-sm font-medium shadow-sm hover:bg-gray-300 transition-colors"
                >
                  {genre}
                </span>
              ))}
            </div>
          </div>

          {/* Description */}
          <div className="p-4 bg-gray-50 rounded-2xl shadow-lg border border-gray-100 min-h-[250px] flex flex-col">
            <h3 className="text-sm font-bold text-gray-600 mb-3">Description</h3>
            <p className="text-gray-700 leading-relaxed text-base flex-grow">
              {book.description}
            </p>
          </div>
        </div>

      </div>
    </div>
  );
};

// --- 4. MAIN APP COMPONENT ---

const LibrarianBookDetailx: React.FC = () => {
  return (
  <iframe 
    src="https://smiling-orange-s2c4oybaq3.edgeone.dev" 
    width="100%" 
    height="100%" 
    title="Library Dashboard"
></iframe>
)
}

const LibrarianBookDetaily: React.FC = () => {
  const [activePage, setActivePage] = useState<ActivePage>('book-management');

  return (
    <div className="min-h-screen bg-gray-100 font-sans flex antialiased">
      
      {/* Sidebar (Fixed width, full height) */}
      <aside className="w-64 bg-white p-6 flex flex-col justify-between border-r shadow-lg rounded-r-2xl">
        <div>
          <h1 className="text-xl font-extrabold text-blue-800 mb-8">BK Library</h1>
          <SidebarNav active={activePage} onSelect={setActivePage} />
        </div>

        {/* User Profile and Logout */}
        <div className="space-y-4">
          <div className="flex items-center space-x-3 p-3 bg-gray-100 rounded-xl">
            <div className="w-10 h-10 bg-blue-100 rounded-full flex items-center justify-center">
              <User className="w-5 h-5 text-blue-600" />
            </div>
            <div>
              <p className="text-sm font-semibold text-gray-800">{MOCK_USER_PROFILE.name}</p>
              <p className="text-xs text-gray-500 truncate">{MOCK_USER_PROFILE.email}</p>
            </div>
            <ChevronDown className="w-4 h-4 text-gray-500 ml-auto" />
          </div>
          
          <button className="w-full flex items-center justify-center space-x-2 bg-red-400 text-white font-semibold py-2 rounded-xl hover:bg-red-500 transition-colors shadow-md">
            <LogOut className="w-5 h-5" />
            <span>Log out</span>
          </button>
        </div>
      </aside>

      {/* Main Content Area */}
      <main className="flex-1 overflow-auto p-4 md:p-8">
        {/* Main Content Card (The Book Detail Section) */}
        <div className="bg-white rounded-3xl shadow-xl">
          <BookDetailContent book={MOCK_BOOK_DATA} />
        </div>
      </main>
    </div>
  );
};

export default LibrarianBookDetailx;