import React, { useState, useEffect, useContext } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Form,
  Button,
  Dropdown,
} from "react-bootstrap";
import UserContext from "../contexts/UserContext";
import { getUserProgramByUserId } from "../Managers/UserProgramManager";
import { addSetLog } from "../Managers/SetLogManager";
import { getTrainingProgramById } from "../Managers/TrainingManager";
import { fetchExercisesForUserDay } from "../Managers/DayExerciseManager";
import { getDaysForTrainingProgram } from "../Managers/DayManager";
import { getAllMuscleGroups } from "../Managers/MuscleGroupManager";
import "./Log.css";

const ExerciseLogger = () => {
  const { user } = useContext(UserContext);
  const [currentDay, setCurrentDay] = useState(null);
  const [trainingProgram, setTrainingProgram] = useState(null);
  const [exercises, setExercises] = useState([]);
  const [setLogs, setSetLogs] = useState({});
  const [days, setDays] = useState([]);
  const [muscleGroups, setMuscleGroups] = useState([]);

  console.log(muscleGroups);

  // fetch user program and training details when comp mounts
  useEffect(() => {
    const fetchUserProgramAndTrainingDetails = async () => {
      const userProgram = await getUserProgramByUserId(user.id);
      console.log(userProgram);
      const allMuscleGroups = await getAllMuscleGroups();
      setMuscleGroups(allMuscleGroups);

      // if the user has a program get the details and exercises
      if (userProgram) {
        const trainingDetails = await getTrainingProgramById(
          userProgram.trainingProgramId
        );
        setCurrentDay(userProgram.currentDay);
        setTrainingProgram(trainingDetails);

        if (trainingDetails) {
          const programDays = await getDaysForTrainingProgram(
            trainingDetails.id
          );
          setDays(programDays);
        }

        const dayExercises = await fetchExercisesForUserDay(
          user.id,
          userProgram.currentDay
        );
        setExercises(dayExercises);
      }
    };

    fetchUserProgramAndTrainingDetails();
  }, [user]);

  // get muscle group name by id
  const getMuscleGroupNameById = (id) => {
    const muscleGroup = muscleGroups.find((mg) => mg.id === id);
    return muscleGroup ? muscleGroup.name : "";
  };

  // handling day change
  const handleChangeDay = async (selectedDayNumber) => {
    const newDay = parseInt(selectedDayNumber);
    setCurrentDay(newDay);
    const dayExercises = await fetchExercisesForUserDay(user.id, newDay);
    setExercises(dayExercises);
  };

  //adding logged sets
  const handleAddSet = (exerciseID) => {
    const logs = { ...setLogs };
    if (!logs[exerciseID]) logs[exerciseID] = [];
    logs[exerciseID].push({ weight: "", reps: "" });
    setSetLogs(logs);
  };

  // handle submitting logs
  const handleLogSubmit = () => {
    Object.keys(setLogs).forEach((exerciseId) => {
      setLogs[exerciseId].forEach((log) => {
        addSetLog({
          UserId: user.id,
          ExerciseId: exerciseId,
          Weight: log.weight,
          Repetitions: log.reps,
          Date: new Date().toISOString(),
        });
      });
    });
    alert("Logs added!");
  };

  return (
    <Container>
      <Row className="my-4">
        <Col></Col>
        <Col md="auto">
          {days.length > 0 && (
            <Dropdown onSelect={handleChangeDay}>
              <Dropdown.Toggle variant="secondary" id="dropdown-basic">
                {(currentDay &&
                  days.find((day) => day.dayNumber === currentDay)?.title) ||
                  "Select Day"}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {days.map((day) => (
                  <Dropdown.Item key={day.Id} eventKey={day.dayNumber}>
                    {day.title}
                  </Dropdown.Item>
                ))}
              </Dropdown.Menu>
            </Dropdown>
          )}
        </Col>
      </Row>

      <h2 className="my-3">
        {(currentDay &&
          days.find((day) => day.dayNumber === currentDay)?.title) ||
          "Select a Day"}
      </h2>

      {exercises.map((exercise) => (
        <Card className="mb-4" key={exercise.id}>
          <Card.Body>
            <Card.Title>{exercise.name}</Card.Title>
            <Card.Subtitle className="mb-2 text-muted">
              ({getMuscleGroupNameById(exercise.muscleGroupId)})
            </Card.Subtitle>
            {setLogs[exercise.id] &&
              setLogs[exercise.id].map((log, index) => (
                <Form.Group key={index} as={Row}>
                  <Col xs={6}>
                    <Form.Label>Reps:</Form.Label>
                    <Form.Control
                      type="number"
                      value={log.reps}
                      onChange={(e) => {
                        const logs = { ...setLogs };
                        logs[exercise.id][index].reps = e.target.value;
                        setSetLogs(logs);
                      }}
                    />
                  </Col>
                  <Col xs={6}>
                    <Form.Label>Weight:</Form.Label>
                    <Form.Control
                      type="number"
                      value={log.weight}
                      onChange={(e) => {
                        const logs = { ...setLogs };
                        logs[exercise.id][index].weight = e.target.value;
                        setSetLogs(logs);
                      }}
                    />
                  </Col>
                </Form.Group>
              ))}
            <Button
              className="mt-2"
              variant="outline-primary"
              onClick={() => handleAddSet(exercise.id)}
            >
              Add Set
            </Button>
          </Card.Body>
        </Card>
      ))}

      <Button variant="dark" size="lg" onClick={handleLogSubmit}>
        Submit Logs
      </Button>
    </Container>
  );
};

export default ExerciseLogger;
