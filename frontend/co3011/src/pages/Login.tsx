import { useState } from "react";
import { useNavigate } from "react-router-dom";

const ACCOUNT = {
  email: "admin@gmail.com",
  password: "123456",
};

export default function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();

    if (email === ACCOUNT.email && password === ACCOUNT.password) {
      localStorage.setItem("loggedIn", "true");
      navigate("/");
    } else {
      setError("Incorrect email or password");
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

        <button type="submit" className="login-btn">
          Login
        </button>

        <div className="divider">
          <span>OR</span>
        </div>

        <div className="signup-text">
          Don’t have an account? <a href="#">Sign Up</a>
        </div>
      </form>
    </div>
  );
}
