import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "./Managers/UserManager";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faEnvelope,
  faLock,
  faDumbbell,
} from "@fortawesome/free-solid-svg-icons";
import "./Register.css";

function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [maxSquat, setMaxSquat] = useState(0);
  const [maxBench, setMaxBench] = useState(0);
  const [maxDeadlift, setMaxDeadlift] = useState(0);
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register({
        username,
        email,
        passwordHash: password,
        maxSquat,
        maxBench,
        maxDeadlift,
      });
      navigate("/login");
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="register-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faUser} className="icon" />
          <input
            type="text"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faEnvelope} className="icon" />
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faLock} className="icon" />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faDumbbell} className="icon" />
          <input
            type="number"
            placeholder="Max Bench"
            value={maxBench}
            onChange={(e) => setMaxBench(e.target.value)}
            required
          />
        </div>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faDumbbell} className="icon" />
          <input
            type="number"
            placeholder="Max Squat"
            value={maxSquat}
            onChange={(e) => setMaxSquat(e.target.value)}
            required
          />
        </div>
        <div className="register-input-group">
          <FontAwesomeIcon icon={faDumbbell} className="icon" />
          <input
            type="number"
            placeholder="Max Deadlift"
            value={maxDeadlift}
            onChange={(e) => setMaxDeadlift(e.target.value)}
            required
          />
        </div>
        <div>
          <button type="submit">Register</button>
        </div>
      </form>
    </div>
  );
}

export default Register;
