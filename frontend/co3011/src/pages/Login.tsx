import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  //const [totpCode, setTotpCode] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await fetch(
        "https://lms-authentication-microservice.onrender.com/login",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            email,
            password,
            //totp_code: totpCode,
          }),
        }
      );

      const data = await response.json();

      if (response.ok) {
        // Store authentication token if returned
        if (data.token) {
          localStorage.setItem("token", data.token);
        }
        localStorage.setItem("loggedIn", "true");
        navigate("/");
      } else {
        setError(data.message || "Incorrect email or password");
      }
    } catch {
      setError("Network error. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-card" onSubmit={handleLogin}>
        <h2 className="title">Login</h2>
        <p className="subtitle">Welcome back! Please log in to access your account.</p>

        {error && <p className="error">{error}</p>}

        <label>Email</label>
        <input
          className="input"
          type="email"
          placeholder="Enter your Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        <label>Password</label>
        <div className="password-wrapper">
          <input
            className="input"
            type={showPwd ? "text" : "password"}
            placeholder="Enter your Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <span className="eye" onClick={() => setShowPwd(!showPwd)}>
            {showPwd ? "👁️" : "👁️‍🗨️"}
          </span>
        </div>

                <div className="forgot">Forgot Password?</div>

        <button type="submit" className="login-btn" disabled={loading}>
          {loading ? "Logging in..." : "Login"}
        </button>

        <div className="divider">
          <span>OR</span>
        </div>

        <div className="signup-text">
          Don't have an account? <a href="/register">Sign Up</a>
        </div>
      </form>
    </div>
  );
}