import React, { useState } from 'react';

// --- Icon Placeholders (Using simple functional components as a stand-in for lucide-react or similar libraries) ---

const Icon = ({ children, className = "" }: { children: React.ReactNode, className?: string }) => (
  <div className={`p-2 bg-indigo-100 text-indigo-600 rounded-lg flex items-center justify-center ${className}`}>
    {children}
  </div>
);

const LayoutDashboard = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 10h18M3 14h18m-9-4v8m-7-8h14a2 2 0 012 2v8a2 2 0 01-2 2H3a2 2 0 01-2-2v-8a2 2 0 012-2z"></path></svg></Icon>;
const BookOpen = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 6.5l8 4.5l-8 4.5l-8-4.5zM12 6.5v9"></path><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M22 13v6a2 2 0 01-2 2H4a2 2 0 01-2-2v-6"></path></svg></Icon>;
const History = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg></Icon>;
const User = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path></svg></Icon>;
const Clock = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg></Icon>;
const Mail = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 8l7.89 5.26c.71.47 1.42.74 2.11.74s1.4-.27 2.11-.74L21 8m-18 8V6a2 2 0 012-2h14a2 2 0 012 2v10a2 2 0 01-2 2H5a2 2 0 01-2-2z"></path></svg></Icon>;
const Phone = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-11a2 2 0 01-2-2v-14z"></path></svg></Icon>;
const MapPin = () => <Icon><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.828 0l-4.243-4.243a8 8 0 1111.314 0z"></path><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"></path></svg></Icon>;
const Bell = () => <Icon className="!bg-red-100 text-red-600"><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6 6 0 10-12 0v3.159c0 .54.21.945.54 1.258L4 17h5m6 0v1a3 3 0 11-6 0v-1"></path></svg></Icon>;
const Check = () => <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 13l4 4L19 7"></path></svg>;
const X = () => <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12"></path></svg>;

// --- Type Definitions ---

interface Request {
  id: number;
  user: string;
  book: string;
  from: string;
  to: string;
  imageUrl: string;
}

interface Notification {
  id: number;
  time: string;
  message: string;
  type: 'overdue' | 'returned' | 'maintenance';
}

// --- Mock Data ---

const MOCK_REQUESTS: Request[] = [
  { id: 1, user: 'Genghis Khan', book: 'Calculus 3', from: '15/10/2025', to: '15/11/2025', imageUrl: 'https://placehold.co/40x40/f43f5e/ffffff?text=GK' },
  { id: 2, user: 'Adolf Hitler', book: 'Probability & Stat...', from: '15/10/2025', to: '15/11/2025', imageUrl: 'https://placehold.co/40x40/10b981/ffffff?text=AH' },
  { id: 3, user: 'Joseph Stalin', book: 'Mein Kampf', from: '15/10/2025', to: '15/11/2025', imageUrl: 'https://placehold.co/40x40/3b82f6/ffffff?text=JS' },
];

const MOCK_NOTIFICATIONS: Notification[] = [
  { id: 1, time: '23h59 29/9/2025', message: 'Calculus 3 - Genghis Khan: Overdue', type: 'overdue' },
  { id: 2, time: '22h50 28/9/2025', message: 'Server down for maintenance', type: 'maintenance' },
  { id: 3, time: '12h30 30/9/2025', message: 'Calculus 3 - Genghis Khan: Returned', type: 'returned' },
];

// --- Sub-Components ---

const Sidebar = () => (
  <div className="w-64 flex-shrink-0 bg-white p-6 border-r border-gray-100 hidden lg:flex flex-col h-full rounded-l-2xl">
    <div className="text-xl font-extrabold text-gray-800 mb-8">BK Library</div>
    
    {/* Navigation */}
    <nav className="space-y-2 flex-grow">
      <NavItem active><LayoutDashboard /> Dashboard</NavItem>
      <NavItem><BookOpen /> Book Management</NavItem>
      <NavItem><History /> History</NavItem>
    </nav>
    
    {/* User Profile and Logout */}
    <div className="mt-8 pt-6 border-t border-gray-100">
      <div className="flex items-center p-3 mb-4 rounded-xl bg-gray-50">
        <img
          className="w-10 h-10 rounded-full mr-3"
          src="https://placehold.co/40x40/d1d5db/4b5563?text=ML"
          alt="Mr. Librarian"
          onError={(e) => { (e.target as HTMLImageElement).onerror = null; (e.target as HTMLImageElement).src = 'https://placehold.co/40x40/d1d5db/4b5563?text=ML' }}
        />
        <div>
          <p className="text-sm font-semibold text-gray-800">Mr. Librarian</p>
          <p className="text-xs text-gray-500">librarian@bk.edu.vn</p>
        </div>
      </div>
      <button className="w-full py-3 bg-red-400 hover:bg-red-500 text-white font-semibold rounded-xl shadow-lg transition duration-200">
        Log out
      </button>
    </div>
  </div>
);

const NavItem: React.FC<{ children: React.ReactNode, active?: boolean }> = ({ children, active = false }) => (
  <a 
    href="#" 
    className={`flex items-center space-x-3 p-3 rounded-xl transition duration-200 ${
      active 
        ? 'bg-indigo-50 text-indigo-700 font-semibold shadow-inner' 
        : 'text-gray-600 hover:bg-gray-50'
    }`}
  >
    {children}
  </a>
);

const StatCard: React.FC<{ title: string, value: number, unit?: string, color?: string }> = ({ title, value, unit, color = 'text-gray-900' }) => (
  <div className="p-4 bg-white rounded-xl shadow-md border border-gray-100 flex-1 min-w-[140px]">
    <p className="text-sm text-gray-500 mb-1">{title}</p>
    <p className={`text-3xl font-bold ${color}`}>{value}{unit}</p>
  </div>
);

const ChartPlaceholder = () => {
  // Simple Bar Chart Placeholder based on the image
  const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
  const data = [100, 200, 150, 350, 180, 250, 400, 150, 300, 450, 220, 380]; // Mock heights

  return (
    <div className="bg-white p-6 rounded-xl shadow-lg border border-gray-100 col-span-1 lg:col-span-2 xl:col-span-3">
      <div className="flex justify-between items-center mb-4">
        <h3 className="text-lg font-semibold text-gray-800">Total book borrowed</h3>
        <select className="text-sm border border-gray-300 rounded-lg p-1.5 focus:ring-indigo-500 focus:border-indigo-500">
          <option>Year</option>
        </select>
      </div>
      <div className="h-64 flex items-end justify-between px-4">
        <div className="absolute left-0 top-0 h-full w-full opacity-5">
          {/* Y-axis lines */}
          <div className="absolute w-full border-t border-dashed top-0"></div>
          <div className="absolute w-full border-t border-dashed top-1/4"></div>
          <div className="absolute w-full border-t border-dashed top-1/2"></div>
          <div className="absolute w-full border-t border-dashed top-3/4"></div>
        </div>
        {/* Y-axis labels */}
        <div className="absolute left-0 text-xs text-gray-400 space-y-7">
          <p className="mt-[-10px]">400</p>
          <p>300</p>
          <p>200</p>
          <p>100</p>
          <p>0</p>
        </div>

        {data.map((h, index) => (
          <div key={index} className="flex flex-col items-center h-full justify-end w-1/12 mx-0.5">
            <div 
              style={{ height: `${(h / 450) * 100}%` }} 
              className={`w-3/4 rounded-t-lg transition-all duration-500 ${index === 9 ? 'bg-indigo-600' : 'bg-indigo-300 hover:bg-indigo-400'}`}
            ></div>
          </div>
        ))}
      </div>
      <div className="flex justify-between text-xs text-gray-500 mt-2 px-4">
        {months.map(m => <span key={m} className="w-1/12 text-center">{m}</span>)}
      </div>
    </div>
  );
};

const PendingRequestItem: React.FC<Request & { onAction: (id: number, action: 'accept' | 'reject') => void }> = ({ id, user, book, from, to, imageUrl, onAction }) => (
  <div className="flex items-center justify-between p-3 border-b last:border-b-0 border-gray-100">
    <div className="flex items-start">
      <img
        className="w-10 h-10 rounded-full mr-3 flex-shrink-0"
        src={imageUrl}
        alt={user}
        onError={(e) => { (e.target as HTMLImageElement).onerror = null; (e.target as HTMLImageElement).src = `https://placehold.co/40x40/f0f0f0/333333?text=${user.split(' ')[0][0]}${user.split(' ')[1]?.[0] || ''}` }}
      />
      <div className='text-sm flex-grow min-w-0'>
        <p className="font-semibold text-gray-800 truncate">{user}</p>
        <p className="text-xs text-gray-600 font-medium">{book}</p>
        <p className="text-[10px] text-gray-400 mt-1">From: {from} <span className="font-bold">To: {to}</span></p>
      </div>
    </div>
    <div className="flex space-x-2 flex-shrink-0 ml-4">
      <button 
        className="text-xs px-3 py-1 bg-indigo-500 hover:bg-indigo-600 text-white rounded-lg transition"
        onClick={() => onAction(id, 'accept')}
      >
        Accept
      </button>
      <button 
        className="text-xs px-3 py-1 bg-gray-200 hover:bg-gray-300 text-gray-800 rounded-lg transition"
        onClick={() => onAction(id, 'reject')}
      >
        Reject
      </button>
    </div>
  </div>
);

const NotificationItem: React.FC<Notification> = ({ time, message, type }) => {
  const colorMap = {
    overdue: 'text-red-500',
    returned: 'text-green-500',
    maintenance: 'text-yellow-600',
  };

  return (
    <div className="flex items-start space-x-3 p-3 border-b last:border-b-0 border-gray-100">
      <Bell />
      <div className="text-xs flex-1">
        <span className="font-semibold text-gray-600 mr-2">{time}</span>
        <span className={`${colorMap[type]}`}>{message}</span>
      </div>
    </div>
  );
};


const ContactCard: React.FC<{ icon: React.ReactNode, text: string, subText: string }> = ({ icon, text, subText }) => (
  <div className="flex items-center justify-center text-center p-4 bg-white rounded-xl shadow-md border border-gray-100">
    <div>
      <div className="flex justify-center mb-2">{icon}</div>
      <p className="text-sm font-medium text-gray-700">{text}</p>
      <p className="text-xs text-gray-500">{subText}</p>
    </div>
  </div>
)

// --- Main App Component ---

export default function App() {
  const [requests, setRequests] = useState<Request[]>(MOCK_REQUESTS);

  const handleRequestAction = (id: number, action: 'accept' | 'reject') => {
    console.log(`Request ID ${id} ${action}ed`);
    setRequests(prev => prev.filter(req => req.id !== id));
    // In a real app, you would send an API call here.
  };

  return (
    <div className="flex h-screen bg-gray-50 p-4 font-sans antialiased">
      {/* Container for the entire dashboard with rounded corners */}
      <div className="flex flex-1 max-w-7xl mx-auto bg-white rounded-2xl shadow-2xl overflow-hidden">
        
        {/* 1. Sidebar */}
        <Sidebar />

        {/* 2. Main Dashboard Content */}
        <div className="flex-1 p-6 space-y-6 overflow-y-auto">
          
          <h1 className="text-2xl font-bold text-gray-900 hidden lg:block">Dashboard (Librarian)</h1>

          {/* Header Bar (Date & Time) */}
          <div className="flex items-center space-x-4 text-gray-600 text-sm">
            <div className="flex items-center space-x-1.5 p-2 bg-gray-100 rounded-lg">
              <Clock />
              <span>30/9/2025</span>
              <span>•</span>
              <span>09:00</span>
            </div>
          </div>

          {/* Main Grid Layout */}
          <div className="grid grid-cols-1 lg:grid-cols-3 xl:grid-cols-4 gap-6">

            {/* A. Left/Main Content Column (lg:col-span-2 / xl:col-span-3) */}
            <div className="lg:col-span-2 xl:col-span-3 space-y-6">
              
              {/* A1. Today Statistics and Membership */}
              <div className="space-y-4">
                <h2 className="text-lg font-semibold text-gray-800">Today statistic</h2>
                <div className="flex flex-wrap gap-4">
                  <StatCard title="Overdue books" value={0} color="text-red-500" />
                  <StatCard title="Returned books" value={27} color="text-green-600" />
                </div>
                
                <h2 className="text-lg font-semibold text-gray-800 pt-4">Membership Registered Today</h2>
                <div className="flex flex-wrap gap-4">
                  <StatCard title="Monthly" value={11} />
                  <StatCard title="Annually" value={9} />
                </div>
              </div>

              {/* A2. Total Book Borrowed Chart */}
              <ChartPlaceholder />

              {/* A3. Pending Requests */}
              <div className="bg-white rounded-xl shadow-lg border border-gray-100">
                <div className="flex justify-between items-center p-4 border-b border-gray-100">
                  <h3 className="text-lg font-semibold text-gray-800">Pending extend borrowing period request</h3>
                  <a href="#" className="text-sm text-indigo-600 hover:text-indigo-800 font-medium">See All »</a>
                </div>
                <div className="divide-y divide-gray-100">
                  {requests.map(req => (
                    <PendingRequestItem key={req.id} {...req} onAction={handleRequestAction} />
                  ))}
                  {requests.length === 0 && (
                    <p className="p-4 text-center text-gray-500 text-sm">No pending requests at this time.</p>
                  )}
                </div>
              </div>

              {/* A4. Notification */}
              <div className="bg-white rounded-xl shadow-lg border border-gray-100">
                <div className="p-4 border-b border-gray-100">
                  <h3 className="text-lg font-semibold text-gray-800">Notification</h3>
                </div>
                <div className="divide-y divide-gray-100">
                  {MOCK_NOTIFICATIONS.map(note => (
                    <NotificationItem key={note.id} {...note} />
                  ))}
                </div>
              </div>

            </div>

            {/* B. Right Column (lg:col-span-1) */}
            <div className="space-y-6 lg:col-span-1">
              
              {/* B1. Total Register Member Card */}
              <div className="flex flex-col items-center justify-center p-6 h-36 bg-white rounded-xl shadow-lg border border-gray-100">
                <p className="text-sm text-gray-500">Total Register Member</p>
                <p className="text-4xl font-extrabold text-indigo-600 mt-2">69,420</p>
                <span className="text-xs text-indigo-500 font-medium mt-1">Members</span>
              </div>
              
              {/* B2. Contact Us */}
              <div className="space-y-4">
                <h3 className="text-lg font-semibold text-gray-800 mt-4">Contact Us</h3>
                <div className="space-y-3">
                  <ContactCard 
                    icon={<MapPin />} 
                    text="Address: A2 block, 268 Ly"
                    subText="Thuong Kiet, Phuong 14, Quan 10"
                  />
                  <ContactCard 
                    icon={<Phone />} 
                    text="Phone number: 028 3864 7256"
                    subText="(24/7 Support)"
                  />
                  <ContactCard 
                    icon={<Mail />} 
                    text="Email: thuvien@hcmut.edu.vn"
                    subText="(Librarian support email)"
                  />
                </div>
              </div>

            </div>
          </div>
        </div>
      </div>
    </div>
  );
}