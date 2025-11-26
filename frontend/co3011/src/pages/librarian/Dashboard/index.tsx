import React from 'react';

// --- Icon Components (Simplified Inline SVGs) ---
const DashboardIcon: React.FC = () => (
  <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <rect x="3" y="3" width="7" height="9" rx="1" ry="1" /><rect x="13" y="3" width="8" height="5" rx="1" ry="1" /><rect x="13" y="9" width="8" height="11" rx="1" ry="1" /><rect x="3" y="15" width="7" height="6" rx="1" ry="1" />
  </svg>
);

const BookIcon: React.FC = () => (
  <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20" />
  </svg>
);

const HistoryIcon: React.FC = () => (
  <svg xmlns="http://www.w3.org/2000/svg" className="w-5 h-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
    <circle cx="12" cy="12" r="10" /><polyline points="12 6 12 12 16 14" />
  </svg>
);

// --- Dummy Data ---
interface StatsCardProps {
  title: string;
  value: number | string;
  isLarge?: boolean;
}

interface Request {
  name: string;
  book: string;
  from: string;
  to: string;
  avatar: string;
}

const statsData: StatsCardProps[] = [
  { title: 'Overdue books', value: 0 },
  { title: 'Returned books', value: 27 },
  { title: 'Monthly', value: 11, isLarge: true },
  { title: 'Annually', value: 9, isLarge: true },
];

const pendingRequests: Request[] = [
  { name: 'Genghis Khan', book: 'Calculus 3', from: '15/10/2025', to: '15/11/2025', avatar: 'https://placehold.co/40x40/4F46E5/ffffff?text=GK' },
  { name: 'Arthur Dent', book: 'Probability & Sta...', from: '15/10/2025', to: '15/11/2025', avatar: 'https://placehold.co/40x40/3B82F6/ffffff?text=AD' },
  { name: 'Jane Doe', book: 'Mein Kaft', from: '15/10/2025', to: '15/11/2025', avatar: 'https://placehold.co/40x40/EC4899/ffffff?text=JD' },
];

//const chartData = [50, 80, 120, 90, 150, 180, 220, 280, 310, 250, 350, 400];
//const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];

// --- Components ---

const NavItem: React.FC<{ icon: React.ReactNode; label: string; isActive?: boolean }> = ({ icon, label, isActive }) => (
  <div className={`flex items-center p-3 rounded-xl cursor-pointer transition-colors ${isActive ? 'bg-indigo-50 text-indigo-700 font-semibold' : 'text-gray-500 hover:bg-gray-100'}`}>
    {icon}
    <span className="ml-3 text-sm">{label}</span>
  </div>
);

const StatsCard: React.FC<StatsCardProps> = ({ title, value, isLarge = false }) => (
  <div className={`p-6 bg-white rounded-xl shadow-lg transition-shadow hover:shadow-xl ${isLarge ? 'h-full flex flex-col justify-center' : ''}`}>
    <div className={`text-3xl font-bold ${value === 0 ? 'text-red-500' : 'text-gray-900'}`}>{value}</div>
    <div className="text-gray-500 mt-1">{title}</div>
  </div>
);

const BarChartPlaceholder: React.FC = () => {
 // const maxValue = 400; // Max value for scaling bars

  return (
    <div className="p-6 bg-white rounded-xl shadow-lg transition-shadow hover:shadow-xl h-[400px] md:h-[350px] flex flex-col">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-lg font-semibold text-gray-800">Total book borrowed</h2>
        <select className="px-3 py-1 text-sm border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500">
          <option>Year</option>
        </select>
      </div>

      <div className="flex-grow flex items-end h-full">
        {/* Y-Axis labels (approximate) */}

        
        {/* Chart Bars */}

      </div>
    </div>
  );
};

const PendingRequestItem: React.FC<Request> = ({ name, book, from, to, avatar }) => (
  <div className="flex items-center justify-between p-3 border-b border-gray-100 last:border-b-0">
    <div className="flex items-center flex-grow min-w-0">
      <img src={avatar} alt={name} className="w-10 h-10 rounded-full object-cover mr-3" onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src='https://placehold.co/40x40/cccccc/000000?text=U'; }}/>
      <div className="min-w-0 flex-1">
        <div className="font-medium text-sm text-gray-800 truncate">{name}</div>
        <div className="text-xs text-gray-500 truncate">{book}</div>
      </div>
    </div>
    <div className="flex flex-col items-start mx-4 text-xs text-gray-500 hidden sm:block">
      <div className="whitespace-nowrap">From: <span className="font-medium text-gray-700">{from}</span></div>
      <div className="whitespace-nowrap">To: <span className="font-medium text-gray-700">{to}</span></div>
    </div>
    <div className="flex space-x-2 flex-shrink-0">
      <button className="px-3 py-1 text-xs font-semibold text-white bg-indigo-500 rounded-lg hover:bg-indigo-600 transition-colors shadow-md">
        Accept
      </button>
      <button className="px-3 py-1 text-xs font-semibold text-gray-700 bg-gray-200 rounded-lg hover:bg-gray-300 transition-colors shadow-md">
        Reject
      </button>
    </div>
  </div>
);

const ContactCard: React.FC<{ title: string; value: string; className?: string }> = ({ title, value, className = '' }) => (
  <div className={`p-4 bg-white rounded-xl shadow-lg text-center ${className}`}>
    <div className="text-sm font-medium text-gray-500 mb-2">{title}</div>
    <div className="text-base font-semibold text-gray-800 break-words">{value}</div>
  </div>
);


const GeneralBooks: React.FC = () => {
  const handleLogout = () => {
    // Implement logout logic here
    console.log('Logging out...');
  };

  return (
    <div className="min-h-screen bg-gray-50 p-4 md:p-8 font-sans antialiased">
      <div className="flex max-w-7xl mx-auto bg-white rounded-3xl shadow-2xl overflow-hidden">
        
        {/* --- Sidebar (Left Panel) --- */}
        <aside className="w-full max-w-[280px] p-6 hidden md:flex flex-col justify-between border-r border-gray-100">
          <div>
            {/* Logo */}
            <h1 className="text-2xl font-extrabold text-gray-800 mb-10">
              BK Library
            </h1>

            {/* Navigation */}
            <nav className="space-y-2">
              <NavItem icon={<DashboardIcon />} label="Dashboard" isActive />
              <NavItem icon={<BookIcon />} label="Book Management" />
              <NavItem icon={<HistoryIcon />} label="History" />
            </nav>
          </div>

          {/* User Profile and Logout */}
          <div className="mt-8 p-4 bg-indigo-50 rounded-xl">
            <div className="flex items-center mb-4">
              <img
                src="https://placehold.co/40x40/FCA5A5/ffffff?text=L"
                alt="Librarian Avatar"
                className="w-10 h-10 rounded-full object-cover mr-3"
                onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src='https://placehold.co/40x40/FCA5A5/ffffff?text=L'; }}
              />
              <div>
                <div className="font-semibold text-gray-800 text-sm">Mr. Librarian</div>
                <div className="text-xs text-gray-500">librarian@edu.vn</div>
              </div>
            </div>
            <button
              onClick={handleLogout}
              className="w-full py-2 text-sm font-semibold text-white bg-rose-500 rounded-xl hover:bg-rose-600 transition-colors shadow-md"
            >
              Log out
            </button>
          </div>
        </aside>

        {/* --- Main Dashboard Content (Right Panel) --- */}
        <main className="flex-1 p-6 lg:p-10">
          <div className="flex flex-col space-y-8">
            
            {/* Header / Date & Time */}
            <header className="flex items-center justify-between">
              <h2 className="text-2xl font-bold text-gray-900">Dashboard</h2>
              <div className="flex items-center space-x-4 text-sm text-gray-600 font-medium">
                <span>30/9/2025</span>
                <span className="w-1.5 h-1.5 bg-gray-300 rounded-full"></span>
                <span>09:00</span>
              </div>
            </header>

            {/* Top Row: Statistics and Chart */}
            <div className="grid grid-cols-12 gap-6">
              
              {/* Stats Cards (Left) */}
              <div className="col-span-12 lg:col-span-4 space-y-6">
                <div className="text-lg font-semibold text-gray-800">Today statistic</div>
                <div className="grid grid-cols-2 gap-4">
                  {statsData.slice(0, 2).map((item, index) => (
                    <StatsCard key={index} {...item} />
                  ))}
                </div>
                
                <div className="text-lg font-semibold text-gray-800 pt-2">Membership Registered Today</div>
                <div className="grid grid-cols-2 gap-4">
                  {statsData.slice(2, 4).map((item, index) => (
                    <StatsCard key={index} {...item} />
                  ))}
                </div>
              </div>

              {/* Chart (Right) */}
              <div className="col-span-12 lg:col-span-8">
                <BarChartPlaceholder />
              </div>
            </div>

            {/* Middle Row: Requests and Total Members */}
            <div className="grid grid-cols-12 gap-6">
              
              {/* Pending Requests (Left) */}
              <div className="col-span-12 lg:col-span-8">
                <div className="p-6 bg-white rounded-xl shadow-lg transition-shadow hover:shadow-xl">
                  <div className="flex justify-between items-center mb-4">
                    <h2 className="text-lg font-semibold text-gray-800">Pending extend borrowing period request</h2>
                    <a href="#" className="text-sm font-medium text-indigo-500 hover:text-indigo-600 transition-colors">See All {'>'}</a>
                  </div>
                  <div className="divide-y divide-gray-100">
                    {pendingRequests.map((req, index) => (
                      <PendingRequestItem key={index} {...req} />
                    ))}
                  </div>
                </div>

                {/* Notifications (Stacked below requests on the left) */}
                <div className="mt-6 p-6 bg-white rounded-xl shadow-lg transition-shadow hover:shadow-xl">
                  <h2 className="text-lg font-semibold text-gray-800 mb-4">Notification</h2>
                  <div className="space-y-3">
                    <p className="flex items-start text-sm text-gray-700">
                      <span className="text-rose-500 font-bold mr-2">!</span>
                      <span className="flex-1">
                        <span className="font-medium">23h59 29/9/2025 - Calculus 3 - Genghis Khan:</span> Overdue
                      </span>
                    </p>
                    <p className="flex items-start text-sm text-gray-700">
                      <span className="text-amber-500 font-bold mr-2">!</span>
                      <span className="flex-1">
                        <span className="font-medium">23h59 29/9/2025 - Server:</span> Server downed for maintenance
                      </span>
                    </p>
                    <p className="flex items-start text-sm text-gray-700">
                      <span className="text-green-500 font-bold mr-2">!</span>
                      <span className="flex-1">
                        <span className="font-medium">12h30 30/9/2025 - Calculus 3 - Genghis Khan:</span> Returned
                      </span>
                    </p>
                  </div>
                </div>
              </div>
              
              {/* Total Members and Contact (Right) */}
              <div className="col-span-12 lg:col-span-4 space-y-6">
                <div className="text-lg font-semibold text-gray-800">Total Register Member</div>
                <div className="p-6 h-[150px] flex items-center justify-center bg-white rounded-xl shadow-lg transition-shadow hover:shadow-xl">
                  <span className="text-3xl lg:text-4xl font-extrabold text-indigo-700">
                    69,420 Members
                  </span>
                </div>

                <div className="text-lg font-semibold text-gray-800 pt-2">Contact Us</div>
                <div className="space-y-4">
                  <ContactCard title="Address:" value="A2 block, 268 Ly Thuong Kiet, Phuong 14, Quan 10" />
                  <ContactCard title="Phone number:" value="028 3864 7256" />
                  <ContactCard title="Email:" value="thuvien@hcmut.edu.vn" />
                </div>
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default GeneralBooks;