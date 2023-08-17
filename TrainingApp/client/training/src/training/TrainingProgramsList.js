import React, { useEffect, useState, useContext } from "react";
import UserContext from "../contexts/UserContext";
import {
  getAllTrainingPrograms,
  addTrainingProgram,
} from "../Managers/TrainingManager";
import { getDaysForTrainingProgram } from "../Managers/DayManager";
import { useNavigate } from "react-router-dom";
import { addUserProgram } from "../Managers/UserProgramManager";
import { ListGroup, Form, Button, Modal, Card } from "react-bootstrap";
import "./Exercise.css";

export const TrainingProgramsList = () => {
  const { user, setIsAuthenticated, setUser } = useContext(UserContext);
  const [programs, setPrograms] = useState([]);
  const [selectedProgramDays, setSelectedProgramDays] = useState([]);
  const [newProgramName, setNewProgramName] = useState("");
  const [newProgramDays, setNewProgramDays] = useState("");
  const [showModal, setShowModal] = useState(false);
  const handleClose = () => setShowModal(false);
  const handleShow = () => setShowModal(true);
  const navigate = useNavigate();

  useEffect(() => {
    refreshPrograms();
  }, []);

  const refreshPrograms = () => {
    getAllTrainingPrograms()
      .then((data) => setPrograms(data))
      .catch((err) => console.error(err));
  };

  // handles adding a program to a user
  const handleAddProgramToUser = (programId) => {
    if (!user) {
      console.error("no user");
      return;
    }

    const userProgram = {
      userId: user.id,
      trainingProgramId: programId,
      startDate: new Date(),
      currentDay: 0,
    };

    addUserProgram(userProgram)
      .then(() => {
        alert("Program added!");
      })
      .catch((err) => console.error(err));
  };

  const navigateToDaysPage = (programId) => {
    navigate(`/days/${programId}`);
  };

  const handleAddProgram = () => {
    const newProgram = {
      name: newProgramName,
      daysPerWeek: parseInt(newProgramDays, 10),
    };

    addTrainingProgram(newProgram)
      .then(() => {
        setNewProgramName("");
        setNewProgramDays("");
        refreshPrograms();
      })
      .catch((err) => console.error(err));
  };

  return (
    <div className="container my-4">
      <h1 className="header">Training Programs</h1>
      <Button variant="outline-primary" onClick={handleShow}>
        Add Program
      </Button>

      <Modal show={showModal} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Add Training Program</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form inline>
            <Form.Control
              type="text"
              placeholder="Program Name"
              value={newProgramName}
              onChange={(e) => setNewProgramName(e.target.value)}
              className="mr-2"
            />
            <Form.Control
              type="text"
              placeholder="Days per week"
              value={newProgramDays}
              onChange={(e) => setNewProgramDays(e.target.value)}
              className="mr-2"
            />
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button
            variant="outline-primary"
            onClick={() => {
              handleAddProgram();
              handleClose();
            }}
          >
            Add New Program
          </Button>
        </Modal.Footer>
      </Modal>

      <div className="card-container mt-4">
        {programs.map((program) => (
          <Card className="program-card" key={program.id}>
            <Card.Body>
              <Card.Title>{program.name}</Card.Title>
              <Button
                variant="outline-primary"
                className="ml-2"
                onClick={() => navigateToDaysPage(program.id)}
              >
                Details
              </Button>
              <Button
                className="secondary-button ml-2"
                onClick={() => handleAddProgramToUser(program.id)}
              >
                Add to My Programs
              </Button>
            </Card.Body>
          </Card>
        ))}
      </div>

      {selectedProgramDays.length > 0 && (
        <div className="mt-4">
          <ListGroup>
            {selectedProgramDays.map((day) => (
              <ListGroup.Item key={day.id}>{day.title}</ListGroup.Item>
            ))}
          </ListGroup>
        </div>
      )}
    </div>
  );
};
