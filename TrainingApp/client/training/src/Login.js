import React, { useState, useContext, useEffect } from "react";
import UserContext from "./contexts/UserContext";
import { useNavigate } from "react-router-dom";
import { login, getUserById } from "./Managers/UserManager";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser, faLock } from "@fortawesome/free-solid-svg-icons";
import "./Login.css";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const [isUserLoggedIn, setIsUserLoggedIn] = useState(false);
  const { setUser, setIsAuthenticated } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (isUserLoggedIn) {
      navigate("/musclegroups");
    }
  }, [isUserLoggedIn, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      //attempt login with email/password
      const { userId, token } = await login(email, password);
      // store token and user id in local storage
      localStorage.setItem("token", token);
      localStorage.setItem("userId", userId);
      const userDetails = await getUserById(userId);
      // set user details in state
      setUser(userDetails);
      // set auth state and user login state to true
      setIsAuthenticated(true);
      setIsUserLoggedIn(true);
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      {error && <p className="error-message">{error}</p>}
      <form onSubmit={handleSubmit} className="login-form">
        <div className="input-group">
          <FontAwesomeIcon icon={faUser} className="icon" />
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="input-group">
          <FontAwesomeIcon icon={faLock} className="icon" />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="submit-group">
          <button type="submit">Login</button>
        </div>
      </form>
    </div>
  );
}

export default Login;
