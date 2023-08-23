import React, { useState, useEffect } from "react";
import {
  getExercisesByMuscleGroupId,
  getExerciseById,
} from "../Managers/ExerciseManager";
import {
  getExercisesForDay,
  addExerciseToDay,
} from "../Managers/DayExerciseManager";
import { useParams } from "react-router-dom";
import { addDay } from "../Managers/DayManager";
import { getAllMuscleGroups } from "../Managers/MuscleGroupManager";
import { getDaysForTrainingProgram } from "../Managers/DayManager";
import { ListGroup, Modal, Button } from "react-bootstrap";
import "./Exercise.css";

const DayList = () => {
  const { programId } = useParams();
  const [days, setDays] = useState([]);
  const [dayExercises, setDayExercises] = useState([]);
  const [muscleGroups, setMuscleGroups] = useState([]);
  const [exercises, setExercises] = useState([]);
  const [selectedMuscleGroupId, setSelectedMuscleGroupId] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [allExercises, setAllExercises] = useState([]);
  const [selectedDayId, setSelectedDayId] = useState(null);
  const [showAddDayModal, setShowAddDayModal] = useState(false);
  const [newDayTitle, setNewDayTitle] = useState("");

  const fetchAllExercises = () => {
    getDaysForTrainingProgram(programId)
      .then((fetchedDays) => {
        setDays(fetchedDays);
        return Promise.all(
          fetchedDays.map((day) => getExercisesForDay(day.id))
        );
      })
      .then((dayExercisesArrays) => {
        // combine all evercise arrays inot a single array
        const allExercises = [].concat(...dayExercisesArrays);
        setDayExercises(allExercises);
        //grab details for each exercise
        return Promise.all(
          allExercises.map((exercise) => getExerciseById(exercise.exerciseId))
        );
      })
      .then((fetchedExercises) => {
        setAllExercises(fetchedExercises);
      })
      .catch((err) => console.error(err));
  };
  // get exercises when programId changes
  useEffect(() => {
    fetchAllExercises();
  }, [programId]);

  // get all muclse groups when comp mounts
  useEffect(() => {
    getAllMuscleGroups()
      .then(setMuscleGroups)
      .catch((error) => {
        console.error(error);
      });
  }, []);

  const handleNewDayTitleChange = (event) => {
    setNewDayTitle(event.target.value);
};

const handleAddDay = () => {
    const newDay = {
        trainingProgramId: programId,
        dayNumber: days.length + 1,
        title: newDayTitle
    };

    addDay(newDay)
        .then(() => {
            handleCloseAddDayModal();
            fetchAllExercises(); 
        })
        .catch((err) => console.error(err));
};

const handleCloseAddDayModal = () => setShowAddDayModal(false);


  const handleDayClick = (dayId) => {
    setSelectedDayId(dayId);
    setShowModal(true);
  };

  const handleMuscleGroupClick = (id) => {
    setSelectedMuscleGroupId(id);
    getExercisesByMuscleGroupId(id)
      .then(setExercises)
      .catch((error) => {
        console.error(`Error ${id}:`, error);
      });
  };

  const handleAddExerciseToDay = (exerciseId) => {
    const dayExercise = {
      dayId: selectedDayId,
      exerciseId: exerciseId,
    };

    addExerciseToDay(dayExercise)
      .then(fetchAllExercises)
      .catch((err) => console.error(err));

    setShowModal(false);
  };

  const handleClose = () => setShowModal(false);

  return (
    <div className="center-content">
      <h1></h1>
      <Button variant="success" onClick={() => setShowAddDayModal(true)}>
    Add New Day
</Button>

      <ListGroup variant="flush" className="day-custom">
        {days.map((day) => (
          <>
          <br></br>
            <ListGroup.Item
              action
              onClick={() => {
                console.log("Day clicked:", day.id);
                handleDayClick(day.id);
              }}
              className="day-title"
            >
              <strong>{day.title}</strong>
            </ListGroup.Item>
            {dayExercises
              .filter((de) => de.dayId === day.id)
              .map((de) => {
                const exercise = allExercises.find(
                  (e) => e.id === de.exerciseId
                );
                return (
                  <ListGroup.Item key={de.id} className="exercise-item">
                    <strong className="exercise-name">
                      {exercise ? exercise.name : "Loading..."}
                    </strong>
                    <br></br>
                    <span className="exercise-description">
                      {exercise ? `  ${exercise.description}` : "Loading..."}
                    </span>
                  </ListGroup.Item>
                );
              })}
          </>
        ))}
      </ListGroup>

      <Modal show={showModal} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Add Exercises</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <h2>Choose a Muscle Group</h2>
          <ListGroup>
            {muscleGroups.map((group) => (
              <ListGroup.Item
                key={group.id}
                action
                onClick={() => handleMuscleGroupClick(group.id)}
              >
                {group.name}
              </ListGroup.Item>
            ))}
          </ListGroup>
          {selectedMuscleGroupId && (
            <div className="mt-4">
              <h2>Exercises</h2>
              <ListGroup>
                {exercises.map((exercise) => (
                  <ListGroup.Item key={exercise.id}>
                    {exercise.name}
                    <Button
                      variant="outline-primary"
                      size="sm"
                      style={{ float: 'right', marginRight: '10px' }}  
                      onClick={() => handleAddExerciseToDay(exercise.id)}
                    >
                      Add
                    </Button>
                  </ListGroup.Item>
                ))}
              </ListGroup>
            </div>
          )}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="outline-secondary" onClick={handleClose}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
      <Modal show={showAddDayModal} onHide={handleCloseAddDayModal}>
    <Modal.Header closeButton>
        <Modal.Title>Add New Day</Modal.Title>
    </Modal.Header>
    <Modal.Body>
        <label>Title:</label>
        <input 
            type="text" 
            className="form-control" 
            value={newDayTitle} 
            onChange={handleNewDayTitleChange}
        />
    </Modal.Body>
    <Modal.Footer>
        <Button variant="outline-secondary" onClick={handleCloseAddDayModal}>
            Close
        </Button>
        <Button variant="primary" onClick={handleAddDay}>
            Add Day
        </Button>
    </Modal.Footer>
</Modal>

    </div>
  );
};

export default DayList;
