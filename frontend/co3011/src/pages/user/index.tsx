import { useEffect, useState } from "react";
import { useAuth } from "../.../../../../context/AuthContext";
import { useNavigate } from "react-router-dom";

// ...rest of your code stays the same

export default function UserMainPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user) {
      navigate("/login");
      return;
    }
    setLoading(false);
  }, [user, navigate]);

  if (loading || !user) {
    return (
      <div style={{ padding: 40, textAlign: "center" }}>
        <p>Loading your dashboard...</p>
      </div>
    );
  }

  const memberDays = user.joinedAt 
    ? Math.floor((new Date().getTime() - new Date(user.joinedAt).getTime()) / (1000 * 60 * 60 * 24))
    : 0;

  return (
    <div style={{ padding: "24px", maxWidth: "1400px", margin: "0 auto" }}>
      {/* Welcome Header */}
      <div style={{ marginBottom: 32 }}>
        <h1 style={{ fontWeight: 600, fontSize: 32, marginBottom: 8, color: "#111827" }}>
          Welcome back, {user.name}! 👋
        </h1>
        <p style={{ color: "#6B7280", fontSize: 16 }}>
          Here's what's happening with your library account today.
        </p>
      </div>

      {/* Quick Stats */}
      <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(240px, 1fr))", gap: 20, marginBottom: 40 }}>
        <StatCard
          title="Total Points"
          value={user.totalPoints?.toLocaleString() || "0"}
          icon="⭐"
          color="#F59E0B"
          subtitle="Keep reading to earn more!"
        />
        <StatCard
          title="Books Borrowed"
          value={user.borrowedBooks || 0}
          icon="📚"
          color="#3B82F6"
          subtitle="Currently in your collection"
        />
        <StatCard
          title="Days as Member"
          value={memberDays}
          icon="📅"
          color="#10B981"
          subtitle="Since you joined"
        />
        <StatCard
          title="Current Rank"
          value="#--"
          icon="🏆"
          color="#8B5CF6"
          subtitle="Check leaderboard"
        />
      </div>

      {/* Quick Actions */}
      <div style={{ marginBottom: 40 }}>
        <h2 style={{ fontWeight: 600, fontSize: 24, marginBottom: 20, color: "#111827" }}>
          Quick Actions
        </h2>
        <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(250px, 1fr))", gap: 20 }}>
          <QuickActionCard
            title="Browse Catalog"
            description="Discover your next great read"
            icon="📚"
            onClick={() => navigate("/user/library-catalog")}
            color="#3B82F6"
          />
          <QuickActionCard
            title="View History"
            description="Check your borrowed books"
            icon="📖"
            onClick={() => navigate("/user/history")}
            color="#8B5CF6"
          />
          <QuickActionCard
            title="Leaderboard"
            description="See where you rank"
            icon="🏆"
            onClick={() => navigate("/user/leaderboard")}
            color="#F59E0B"
          />
          <QuickActionCard
            title="My Profile"
            description="Update your information"
            icon="👤"
            onClick={() => navigate("/user/profile")}
            color="#10B981"
          />
        </div>
      </div>

      {/* Currently Borrowed Books */}
      <div style={{ marginBottom: 40 }}>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 20 }}>
          <h2 style={{ fontWeight: 600, fontSize: 24, color: "#111827" }}>
            Currently Borrowed
          </h2>
          <button
            onClick={() => navigate("/user/history")}
            style={{
              padding: "8px 16px",
              background: "transparent",
              border: "1px solid #D1D5DB",
              borderRadius: 8,
              cursor: "pointer",
              fontSize: 14,
              color: "#6B7280",
            }}
          >
            View All →
          </button>
        </div>
        
        {user.borrowedBooks && user.borrowedBooks > 0 ? (
          <BorrowedBooksList userId={user.id} />
        ) : (
          <EmptyState
            icon="📚"
            title="No books borrowed yet"
            description="Visit the library catalog to start borrowing books"
            actionLabel="Browse Catalog"
            onAction={() => navigate("/user/library-catalog")}
          />
        )}
      </div>

      {/* Reading Progress */}
      <div>
        <h2 style={{ fontWeight: 600, fontSize: 24, marginBottom: 20, color: "#111827" }}>
          Your Reading Journey
        </h2>
        <div style={{ 
          padding: 32, 
          background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
          borderRadius: 16,
          color: "white",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center"
        }}>
          <div>
            <h3 style={{ fontSize: 20, fontWeight: 600, marginBottom: 8 }}>
              Keep up the great work! 🎉
            </h3>
            <p style={{ opacity: 0.9, marginBottom: 16 }}>
              You've earned {user.totalPoints || 0} points so far
            </p>
            <button
              onClick={() => navigate("/user/rankmap")}
              style={{
                padding: "10px 24px",
                background: "white",
                color: "#667eea",
                border: "none",
                borderRadius: 8,
                cursor: "pointer",
                fontWeight: 600,
                fontSize: 14,
              }}
            >
              View Progress Map
            </button>
          </div>
          <div style={{ fontSize: 64 }}>📊</div>
        </div>
      </div>
    </div>
  );
}

// Stat Card Component
function StatCard({ 
  title, 
  value, 
  icon, 
  color,
  subtitle 
}: { 
  title: string; 
  value: string | number; 
  icon: string; 
  color: string;
  subtitle: string;
}) {
  return (
    <div
      style={{
        padding: 24,
        background: "white",
        border: "1px solid #E5E7EB",
        borderRadius: 16,
        boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
        transition: "all 0.2s",
      }}
    >
      <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", marginBottom: 16 }}>
        <div
          style={{
            width: 56,
            height: 56,
            borderRadius: 12,
            background: `${color}20`,
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            fontSize: 28,
          }}
        >
          {icon}
        </div>
      </div>
      <p style={{ fontSize: 14, color: "#6B7280", marginBottom: 8 }}>{title}</p>
      <p style={{ fontSize: 32, fontWeight: 700, color: "#111827", marginBottom: 4 }}>{value}</p>
      <p style={{ fontSize: 12, color: "#9CA3AF" }}>{subtitle}</p>
    </div>
  );
}

// Quick Action Card Component
function QuickActionCard({ 
  title, 
  description, 
  icon, 
  onClick,
  color 
}: { 
  title: string; 
  description: string; 
  icon: string; 
  onClick: () => void;
  color: string;
}) {
  const [isHovered, setIsHovered] = useState(false);

  return (
    <div
      onClick={onClick}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
      style={{
        padding: 24,
        background: "white",
        border: `2px solid ${isHovered ? color : "#E5E7EB"}`,
        borderRadius: 16,
        cursor: "pointer",
        transition: "all 0.2s",
        transform: isHovered ? "translateY(-4px)" : "translateY(0)",
        boxShadow: isHovered ? "0 8px 20px rgba(0,0,0,0.1)" : "0 1px 3px rgba(0,0,0,0.05)",
      }}
    >
      <div style={{ 
        fontSize: 40, 
        marginBottom: 16,
        filter: isHovered ? "grayscale(0%)" : "grayscale(20%)",
        transition: "filter 0.2s"
      }}>
        {icon}
      </div>
      <h3 style={{ fontWeight: 600, fontSize: 18, marginBottom: 8, color: "#111827" }}>
        {title}
      </h3>
      <p style={{ fontSize: 14, color: "#6B7280", lineHeight: 1.5 }}>
        {description}
      </p>
    </div>
  );
}

// Borrowed Books List Component (Placeholder)
function BorrowedBooksList({ userId }: { userId: string }) {
  // TODO: Fetch actual borrowed books from API
  return (
    <div style={{ 
      padding: 24, 
      background: "white", 
      border: "1px solid #E5E7EB", 
      borderRadius: 12,
      textAlign: "center"
    }}>
      <p style={{ color: "#6B7280" }}>Loading your borrowed books...</p>
      <p style={{ fontSize: 12, color: "#9CA3AF", marginTop: 8 }}>User ID: {userId}</p>
    </div>
  );
}

// Empty State Component
function EmptyState({ 
  icon, 
  title, 
  description, 
  actionLabel, 
  onAction 
}: { 
  icon: string; 
  title: string; 
  description: string; 
  actionLabel: string; 
  onAction: () => void;
}) {
  return (
    <div style={{ 
      padding: 48, 
      background: "white", 
      border: "2px dashed #D1D5DB", 
      borderRadius: 16,
      textAlign: "center"
    }}>
      <div style={{ fontSize: 64, marginBottom: 16 }}>{icon}</div>
      <h3 style={{ fontSize: 18, fontWeight: 600, marginBottom: 8, color: "#111827" }}>
        {title}
      </h3>
      <p style={{ fontSize: 14, color: "#6B7280", marginBottom: 24 }}>
        {description}
      </p>
      <button
        onClick={onAction}
        style={{
          padding: "12px 24px",
          background: "#3B82F6",
          color: "white",
          border: "none",
          borderRadius: 8,
          cursor: "pointer",
          fontWeight: 600,
          fontSize: 14,
        }}
      >
        {actionLabel}
      </button>
    </div>
  );
}