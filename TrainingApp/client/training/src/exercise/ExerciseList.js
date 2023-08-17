import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import {
  getExercisesByMuscleGroupId,
  addExercise,
  updateExercise,
  deleteExercise,
} from "../Managers/ExerciseManager";
import {
  Container,
  Row,
  Col,
  Card,
  Modal,
  Button,
  Form,
} from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faEdit, faTrash } from "@fortawesome/free-solid-svg-icons";

const ExerciseList = ({ muscleGroupId }) => {
  const [exercises, setExercises] = useState([]);
  const { id: routeId } = useParams();
  // getting the muscle group id to use from the id prop 
  const muscleGroupIdToUse = muscleGroupId || routeId;

  const [showAddModal, setShowAddModal] = useState(false);
  const [showUpdateModal, setShowUpdateModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [exerciseToUpdate, setExerciseToUpdate] = useState({});
  const [exerciseFormData, setExerciseFormData] = useState({
    name: "",
    description: "",
    muscleGroupId: muscleGroupIdToUse,
  });

  //getting exercises based on muscle group id
  useEffect(() => {
    getExercisesByMuscleGroupId(muscleGroupIdToUse)
      .then(setExercises)
      .catch((err) => console.error(err));
  }, [muscleGroupIdToUse]);

  const handleAdd = () => {
    addExercise(exerciseFormData).then((newExercise) => {
      setExercises([...exercises, newExercise]);
      setShowAddModal(false);
    });
  };

  const handleUpdate = () => {
    const fullExerciseData = {
      ...exerciseToUpdate,
      ...exerciseFormData,
    };

    updateExercise(exerciseToUpdate.id, fullExerciseData).then(() => {
      const updatedExercises = exercises.map((ex) =>
        ex.id === exerciseToUpdate.id ? fullExerciseData : ex
      );
      setExercises(updatedExercises);
      setShowUpdateModal(false);
    });
  };

  const handleDelete = (id) => {
    deleteExercise(id).then(() => {
      const filteredExercises = exercises.filter((ex) => ex.id !== id);
      setExercises(filteredExercises);
      setShowDeleteModal(false);
    });
  };

  return (
    <Container className="mt-4">
      <Row className="bh-light p-5">
        <Col md={{ span: 12 }}>
          <Button
            variant="outline-primary"
            onClick={() => setShowAddModal(true)}
          >
            <FontAwesomeIcon icon={faPlus} /> Add Exercise
          </Button>
          <h2 className="text-center mb-4">Exercises</h2>
          <Row>
            {exercises.map((exercise) => (
              <Col md={4} key={exercise.id}>
                <Card className="mb-3">
                  <Card.Body>
                    <Card.Title>{exercise.name}</Card.Title>
                    <Card.Text>{exercise.description}</Card.Text>
                    <Button
                      variant="outline-primary"
                      onClick={() => {
                        setExerciseToUpdate(exercise);
                        setExerciseFormData({
                          name: exercise.name,
                          description: exercise.description,
                        });
                        setShowUpdateModal(true);
                      }}
                    >
                      <FontAwesomeIcon icon={faEdit} />
                    </Button>

                    <Button
                      variant="outline-danger"
                      onClick={() => {
                        setExerciseToUpdate(exercise);
                        setShowDeleteModal(true);
                      }}
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </Button>
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>

          <Modal show={showAddModal} onHide={() => setShowAddModal(false)}>
            <Modal.Header closeButton>
              <Modal.Title>Add Exercise</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form>
                <Form.Group>
                  <Form.Label>Name</Form.Label>
                  <Form.Control
                    type="text"
                    placeholder="Enter exercise name"
                    value={exerciseFormData.name}
                    onChange={(e) =>
                      setExerciseFormData({
                        ...exerciseFormData,
                        name: e.target.value,
                      })
                    }
                  />
                </Form.Group>
                <Form.Group>
                  <Form.Label>Description</Form.Label>
                  <Form.Control
                    as="textarea"
                    rows={3}
                    placeholder="Enter exercise description"
                    value={exerciseFormData.description}
                    onChange={(e) =>
                      setExerciseFormData({
                        ...exerciseFormData,
                        description: e.target.value,
                      })
                    }
                  />
                </Form.Group>
              </Form>
            </Modal.Body>
            <Modal.Footer>
              <Button
                variant="secondary"
                onClick={() => setShowAddModal(false)}
              >
                Close
              </Button>
              <Button variant="primary" onClick={handleAdd}>
                Save Changes
              </Button>
            </Modal.Footer>
          </Modal>

          <Modal
            show={showUpdateModal}
            onHide={() => setShowUpdateModal(false)}
          >
            <Modal.Header closeButton>
              <Modal.Title>Update Exercise</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form>
                <Form.Group>
                  <Form.Label>Name</Form.Label>
                  <Form.Control
                    type="text"
                    placeholder="Enter exercise name"
                    value={exerciseFormData.name}
                    onChange={(e) =>
                      setExerciseFormData({
                        ...exerciseFormData,
                        name: e.target.value,
                      })
                    }
                  />
                </Form.Group>
                <Form.Group>
                  <Form.Label>Description</Form.Label>
                  <Form.Control
                    as="textarea"
                    rows={3}
                    placeholder="Enter exercise description"
                    value={exerciseFormData.description}
                    onChange={(e) =>
                      setExerciseFormData({
                        ...exerciseFormData,
                        description: e.target.value,
                      })
                    }
                  />
                </Form.Group>
              </Form>
            </Modal.Body>
            <Modal.Footer>
              <Button
                variant="secondary"
                onClick={() => setShowUpdateModal(false)}
              >
                Close
              </Button>
              <Button variant="primary" onClick={handleUpdate}>
                Update
              </Button>
            </Modal.Footer>
          </Modal>

          <Modal
            show={showDeleteModal}
            onHide={() => setShowDeleteModal(false)}
          >
            <Modal.Header closeButton>
              <Modal.Title>Confirm Delete</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              Are you sure you want to delete {exerciseToUpdate.name}?
            </Modal.Body>
            <Modal.Footer>
              <Button
                variant="secondary"
                onClick={() => setShowDeleteModal(false)}
              >
                Close
              </Button>
              <Button
                variant="danger"
                onClick={() => handleDelete(exerciseToUpdate.id)}
              >
                Delete
              </Button>
            </Modal.Footer>
          </Modal>
        </Col>
      </Row>
    </Container>
  );
};

export default ExerciseList;
