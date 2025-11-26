import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

/*
const ACCOUNT = {
  email: "admin@gmail.com",
  password: "123456",
};
*/

export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (password && confirmPassword && password !== confirmPassword) {
      setError("Passwords do not match");
    } else {
        console.log("Passwords match");
        setError("");
    }
  }, [confirmPassword, password]);

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();

    alert("Registration successful! Please login with your new credentials.");

    navigate("/login");
  };

  return (
    <div className="login-container">
      <form className="login-card" onSubmit={handleRegister}>
        <h2 className="title">Register</h2>
        <p className="subtitle">Feel nice to create a new account.</p>

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

        <button type="submit" className="login-btn">
          Register
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
