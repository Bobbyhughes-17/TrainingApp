import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./Login";
import Register from "./Register";
import UserContext from "./contexts/UserContext";
import Header from "./Header";
import MuscleGroup from "./exercise/MuscleGroup";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Welcome from "./Welcome";
import ExerciseList from "./exercise/ExerciseList";
import Footer from "./Footer";
import { TrainingProgramsList } from "./training/TrainingProgramsList";
import DayList from "./training/DayList";
import ExerciseLogger from "./logger/ExerciseLogger";
import { ExerciseLogViewer } from "./logger/ExerciseLogViewer";
import { getUserById, getToken } from "./Managers/UserManager";

function App() {
  const [user, setUser] = useState(null);
  const token = localStorage.getItem("token");
  const [isAuthenticated, setIsAuthenticated] = useState(
    !!localStorage.getItem("token")
  );

  // hook to handle authentication and fetch details based off token
  useEffect(() => {
    if (token) {
      // get userId from local storage
      const userId = localStorage.getItem("userId");
      if (userId) {
        // get user details
        getUserById(userId)
          .then((userDetails) => {
            setUser(userDetails);
          })
          .catch((error) => {
            console.error(error);
            setIsAuthenticated(false);
            localStorage.removeItem("token");
            localStorage.removeItem("userId");
          });
      }
    }
  }, [token]);

  return (
    <UserContext.Provider
      value={{ user, setUser, isAuthenticated, setIsAuthenticated }}
    >
      <Router>
        <div className="App">
          <Header
            isAuthenticated={isAuthenticated}
            setIsAuthenticated={setIsAuthenticated}
          />
          <div className="main-content">
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route path="/" element={<Welcome />} />
              <Route path="/register" element={<Register />} />
              <Route path="/musclegroups" element={<MuscleGroup />} />
              <Route path="/exercises/:id" element={<ExerciseList />} />
              <Route path="/programs" element={<TrainingProgramsList />} />
              <Route path="/days/:programId" element={<DayList />} />
              <Route path="/exercise-logger" element={<ExerciseLogger />} />
              <Route
                path="/exercise-log-viewer"
                element={<ExerciseLogViewer />}
              />
            </Routes>
          </div>
          <Footer />
        </div>
      </Router>
    </UserContext.Provider>
  );
}

export default App;
