import React, { useContext, useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Dropdown, Modal, Button } from "react-bootstrap";
import { updateUser } from "./Managers/UserManager";
import UserContext from "./contexts/UserContext";
import { getUserProgramByUserId } from "./Managers/UserProgramManager";
import { getTrainingProgramById } from "./Managers/TrainingManager";
import "bootstrap/dist/css/bootstrap.min.css";

import "./Header.css";

function Header() {
  const [userProgram, setUserProgram] = useState(null);
  const [trainingProgram, setTrainingProgram] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const { isAuthenticated, setIsAuthenticated, user, setUser } =
    useContext(UserContext);
  const [tempUser, setTempUser] = useState({ ...user });
  const navigate = useNavigate();

  const handleLogout = () => {
    // clear token, auth state and user details and nav to login
    localStorage.removeItem("token");
    setIsAuthenticated(false);
    setUser(null);
    navigate("/login");
  };

  // get user program and training details when the user changes
  useEffect(() => {
    if (user && user.id) {
      getUserProgramByUserId(user.id).then((data) => {
        setUserProgram(data);
        if (data && data.trainingProgramId) {
          getTrainingProgramById(data.trainingProgramId).then((programData) => {
            setTrainingProgram(programData);
          });
        }
      });
    }
  }, [user]);

  // set temp user data when the user data changes
  useEffect(() => {
    if (user) {
      setTempUser({
        id: user.id,
        email: user.email,
        username: user.username,
        maxBench: user.maxBench,
        maxSquat: user.maxSquat,
        maxDeadlift: user.maxDeadlift,
      });
    }
  }, [user]);

  const handleUpdate = async () => {
    try {
      const updatedUser = await updateUser(tempUser);
      setUser(updatedUser);
      setShowModal(false);
      // couldn't get state to refresh when a user updates info so I took the easy way out. sry
      window.location.reload();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <nav className="navbar">
      <Link to="/" className="navbar-brand">
        GitFit
      </Link>
      <div className="nav-links-container">
        <Link to="/musclegroups" className="nav-item">
          Hub
        </Link>
        <Link to="/programs" className="nav-item">
          Programs
        </Link>
      </div>
      <div>
        {!isAuthenticated ? (
          <>
            <Link to="/login" className="nav-item">
              Login
            </Link>
            <Link to="/register" className="nav-item">
              Register
            </Link>
          </>
        ) : (
          <>
            <Dropdown>
              <Dropdown.Toggle variant="outline-info" id="dropdown-basic">
                {user && user.username}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                <Dropdown.Item href="#/action-1">
                  Email: {user && user.email}
                </Dropdown.Item>
                <Dropdown.Item href="#/action-2">
                  Max Bench: {user && user.maxBench}
                </Dropdown.Item>
                <Dropdown.Item href="#/action-3">
                  Max Squat: {user && user.maxSquat}
                </Dropdown.Item>
                <Dropdown.Item href="#/action-4">
                  Max Deadlift: {user && user.maxDeadlift}
                </Dropdown.Item>
                <Dropdown.Item onClick={() => setShowModal(true)}>
                  Edit Information
                </Dropdown.Item>

                <Dropdown.Item onClick={() => navigate(`/exercise-log-viewer`)}>
                  View Logged Workouts
                </Dropdown.Item>

                {trainingProgram && (
                  <>
                    <Dropdown.Divider />
                    <Dropdown.Item onClick={() => navigate(`/exercise-logger`)}>
                      Current Program: {trainingProgram.name}
                    </Dropdown.Item>
                  </>
                )}
                <Dropdown.Divider />
                <Dropdown.Item onClick={handleLogout}>Logout</Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
            <Modal
              show={showModal}
              onHide={() => setShowModal(false)}
              className="custom-modal"
            >
              <Modal.Header closeButton className="custom-modal-header">
                <Modal.Title>Update Info</Modal.Title>
              </Modal.Header>
              <Modal.Body className="custom-modal-body">
                <div className="input-group-header">
                  <label>Email</label>
                  <input
                    value={tempUser.email}
                    onChange={(e) =>
                      setTempUser({ ...tempUser, email: e.target.value })
                    }
                  />
                  <label>Username</label>
                  <input
                    value={tempUser.username}
                    onChange={(e) =>
                      setTempUser({ ...tempUser, username: e.target.value })
                    }
                  />
                  <label>Max Bench</label>
                  <input
                    value={tempUser.maxBench}
                    onChange={(e) =>
                      setTempUser({ ...tempUser, maxBench: e.target.value })
                    }
                  />

                  <label>Max Squat</label>
                  <input
                    value={tempUser.maxSquat}
                    onChange={(e) =>
                      setTempUser({ ...tempUser, maxSquat: e.target.value })
                    }
                  />

                  <label>Max Deadlift</label>
                  <input
                    value={tempUser.maxDeadlift}
                    onChange={(e) =>
                      setTempUser({ ...tempUser, maxDeadlift: e.target.value })
                    }
                  />
                </div>
              </Modal.Body>
              <Modal.Footer className="custom-modal-footer">
                <Button
                  variant="outline-secondary"
                  onClick={() => setShowModal(false)}
                >
                  Close
                </Button>
                <Button variant="outline-primary" onClick={handleUpdate}>
                  Save Changes
                </Button>
              </Modal.Footer>
            </Modal>
          </>
        )}
      </div>
    </nav>
  );
}

export default Header;
