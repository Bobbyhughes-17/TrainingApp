import React from "react";
import { Link } from "react-router-dom";
import "./Welcome.css";

function Welcome() {
  return (
    <div className="welcome-container">
      <h1>Welcome to GitFit!</h1>
      <p>
        GitFit is your ultimate companion in your weight lifting journey.
        Whether you're a beginner starting your journey or an expert looking to
        refine your techniques, we've got you covered.
      </p>
      <p>Here's what you can expect:</p>
      <ul>
        <li>
          <strong>Personalized Programs:</strong> Tailored weight lifting
          programs based on your goals and fitness level.
        </li>
        <li>
          <strong>Step-by-Step Guidance:</strong> Detailed guidance on each
          exercise, including descriptions, dos & don'ts and videos coming soon!
        </li>
        <li>
          <strong>Progress Tracking:</strong> Keep track of your sets, reps, and
          weights to watch your progress unfold.
        </li>
        <li>
          <strong>Community Support:</strong> Join a community of like-minded
          fitness enthusiasts, share your journey, and get inspired.
        </li>
      </ul>
      <p>
        Get started by registering or logging in, and let's embark on this
        journey together!
      </p>

      <div className="auth-buttons">
        <p>
          Already a member? <Link to="/login">Login!</Link>
        </p>
        <p>
          Want to join? <Link to="/register">Click here to register!</Link>
        </p>
      </div>
    </div>
  );
}

export default Welcome;
