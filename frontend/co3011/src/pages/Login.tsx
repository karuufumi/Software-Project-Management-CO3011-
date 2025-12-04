import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";

export default function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await fetch("https://lms-authentication-microservice.onrender.com/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();

      if (!response.ok) {
        setError(data.detail || "Incorrect email or password");
        return;
      }

      const user = data.user;
      localStorage.setItem("token", data.token);
      localStorage.setItem("userId", user.id);
      localStorage.setItem("username", user.name);
      localStorage.setItem("role", user.role);
      localStorage.setItem("email", user.email);


      if (user.role === "admin") navigate("/admin");
      else if (user.role === "librarian") navigate("/librarian");
      else navigate("/user");
    } catch {
      setError("Network error. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-card" onSubmit={handleLogin}>
        <h2 className="title">Login</h2>
        <p className="subtitle">Welcome back!</p>

        {error && <p className="error">{error}</p>}

        <label>Email</label>
        <input
          className="input"
          type="email"
          placeholder="Enter email"
          value={email}
          required
          onChange={(e) => setEmail(e.target.value)}
        />

        <label>Password</label>
        <div className="password-wrapper">
          <input
            className="input"
            type={showPwd ? "text" : "password"}
            placeholder="Enter password"
            value={password}
            required
            onChange={(e) => setPassword(e.target.value)}
          />
          <span className="eye" onClick={() => setShowPwd(!showPwd)}>
            {showPwd ? "👁️" : "👁️‍🗨️"}
          </span>
        </div>

        <button type="submit" className="login-btn" disabled={loading}>
          {loading ? "Logging in..." : "Login"}
        </button>

        {/* ➕ Add Register Link */}
        <div className="signup-text" style={{ marginTop: "10px", textAlign: "center" }}>
          Don't have an account? <Link to="/register">Register</Link>
        </div>
      </form>
    </div>
  );
}