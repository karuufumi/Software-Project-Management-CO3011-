import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type AllowedRole = "admin" | "user" | "guest" | "member";

interface FastApiErrorDetail {
  msg: string;
}

export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [name, setName] = useState("");
  const [role, setRole] = useState<AllowedRole>("user");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string>("");
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

    const validRoles: AllowedRole[] = ["admin", "user", "guest", "member"];
    if (!validRoles.includes(role)) {
      setError("Invalid role. Choose admin, user, guest or member.");
      return;
    }

    setLoading(true);
    setError("");

    try {
      const authRes = await fetch(
        "https://lms-authentication-microservice.onrender.com/register",
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            email,
            name,
            password,
            role,
          }),
        }
      );

      const authData = await authRes.json();

      if (!authRes.ok) {
        let message = "Registration failed";

        if (Array.isArray(authData.detail)) {
          message = authData.detail.map((e: FastApiErrorDetail) => e.msg).join(" | ");
        } else if (typeof authData.detail === "string") {
          message = authData.detail;
        }

        setError(message);
        return;
      }

      alert("Registration successful! Please login.");
      navigate("/login");
    } catch {
      setError("Unable to connect to authentication server.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form className="login-card" onSubmit={handleRegister}>
        <h2 className="title">Register</h2>
        <p className="subtitle">Create your account</p>

        {error && <p className="error">{error}</p>}

        <label>Name</label>
        <input
          className="input"
          type="text"
          value={name}
          required
          onChange={(e) => setName(e.target.value)}
        />

        <label>Email</label>
        <input
          className="input"
          type="email"
          value={email}
          required
          onChange={(e) => setEmail(e.target.value)}
        />

        <label>Password</label>
        <input
          className="input"
          type="password"
          value={password}
          required
          onChange={(e) => setPassword(e.target.value)}
        />

        <label>Confirm Password</label>
        <input
          className="input"
          type="password"
          value={confirmPassword}
          required
          onChange={(e) => setConfirmPassword(e.target.value)}
        />

        <label>Role</label>
        <select
          className="input"
          value={role}
          onChange={(e) => setRole(e.target.value as AllowedRole)}
        >
          <option value="user">User / Member</option>
          <option value="admin">Admin</option>
          <option value="guest">Guest</option>
          <option value="member">Member</option>
        </select>

        <button className="login-btn" type="submit" disabled={loading}>
          {loading ? "Registering..." : "Register"}
        </button>

        <div className="signup-text">
          Already have an account? <a href="/login">Login</a>
        </div>
      </form>
    </div>
  );
}
