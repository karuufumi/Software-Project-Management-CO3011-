import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [role] = useState("user");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (password && confirmPassword && password !== confirmPassword) {
      setError("Passwords do not match");
    } else {
      setError("");
    }
  }, [confirmPassword, password]);

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    setError("");
    setLoading(true);

    try {
      const response = await fetch(
        "https://lms-authentication-microservice.onrender.com/register",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            email,
            name,
            password,
            role,
          }),
        }
      );

      const data = await response.json();

      if (response.ok) {
        alert("Registration successful! Please login with your new credentials.");
        navigate("/login");
      } else {
        setError(data.message || "Registration failed. Please try again.");
      }
    } catch {
      setError("Network error. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-card" onSubmit={handleRegister}>
        <h2 className="title">Register</h2>
        <p className="subtitle">Feel nice to create a new account.</p>

        {error && <p className="error">{error}</p>}

        <label>Name</label>
        <input
          className="input"
          type="text"
          placeholder="Enter your Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />

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

        <label>Confirm Password</label>
        <div className="password-wrapper">
          <input
            className="input"
            type={showPwd ? "text" : "password"}
            placeholder="Confirm your Password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
          <span className="eye" onClick={() => setShowPwd(!showPwd)}>
            {showPwd ? "👁️" : "👁️‍🗨️"}
          </span>
        </div>

        

        <button type="submit" className="login-btn" disabled={loading}>
          {loading ? "Registering..." : "Register"}
        </button>

        <div className="divider">
          <span>OR</span>
        </div>

        <div className="signup-text">
          Already have an account? <a href="/login">Login</a>
        </div>
      </form>
    </div>
  );
}