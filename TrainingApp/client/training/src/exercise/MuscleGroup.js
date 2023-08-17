import React, { useState, useEffect } from "react";
import { getAllMuscleGroups } from "../Managers/MuscleGroupManager";
import { useNavigate } from "react-router-dom";
import { ListGroup, Container, Row, Col, Image } from "react-bootstrap";
import Image1 from "../images/Image1.jpeg";
import Image3 from "../images/Image3.jpeg";
import "./List.css";


const MuscleGroup = ({ onMuscleGroupClick }) => {
  const [muscleGroups, setMuscleGroups] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    getAllMuscleGroups().then((groups) => {
      setMuscleGroups(groups);
    });
  }, []);

  const handleMuscleGroupClick = (id) => {
    if (onMuscleGroupClick) {
      onMuscleGroupClick(id);
    } else {
      navigate(`/exercises/${id}`);
    }
  };

  return (
    <Container className="mt-5">
      <Row className="mb-4 text-center">
        <Col md={12} className="custom-row">
          <h3>Training Guide Central Hub</h3>
          <p>
            Below are the guides, linked here for easy access, and designed for
            your success. Click on any of the muscle groups to see a list of
            exercises specific to that muscle group! Video tutorials for each
            movement coming soon!
          </p>
        </Col>
      </Row>

      <Row className="mb-5">
        <Col md={{ span: 4, offset: 2 }}>
          <Image
            src={Image1}
            alt="Muscle Group Image"
            fluid
            style={{ maxWidth: "300px" }}
            className="smooth-image"
          />
        </Col>
        <Col md={4}>
          <Image
            src={Image3}
            alt="Muscle Group Image"
            fluid
            style={{ maxWidth: "300px" }}
            className="smooth-image"
          />
        </Col>
      </Row>

      <Row className="mt-5 custom-row">
        <Col md={{ span: 8, offset: 2 }}>
          <h3>Understanding Muscle Groups</h3>
          <p>
            The human body comprises multiple muscle groups, each responsible
            for specific functions and movements. By categorizing exercises
            based on these muscle groups, it allows for a more systematic and
            targeted approach to strength training and body conditioning.
          </p>
          <p>
            When you select a specific muscle group on our platform, you're
            presented with a curated list of exercises uniquely tailored for
            that group. This segmentation ensures that:
          </p>
          <ul>
            <ul>
              <strong>Efficient Training:</strong> By focusing on one muscle
              group at a time, you can work on improving strength and endurance
              in that particular area, leading to faster results.
            </ul>
            <ul>
              <strong>Reduced Risk of Injury:</strong> Targeted exercises help
              in ensuring that you’re using the right form and technique,
              significantly decreasing the chances of strains or injuries.
            </ul>
            <ul>
              <strong>Better Recovery:</strong> Isolating muscle groups allows
              other parts of your body to rest, ensuring optimal recovery
              periods and preventing overtraining.
            </ul>
            <ul>
              <strong>Versatile Workouts:</strong> With a variety of exercises
              available for each muscle group, you can constantly switch up your
              routine, keeping your workouts fresh and challenging.
            </ul>
          </ul>
          <p>
            So, as you embark on your fitness journey, understanding and
            utilizing these muscle group categorizations will be crucial in
            helping you achieve your goals efficiently and safely.
          </p>
        </Col>
      </Row>

      <Row className="my-5 text-center">
        <Col md={{ span: 8, offset: 2 }}>
          <ListGroup className="d-inline-block" style={{ width: "100%" }}>
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
        </Col>
      </Row>
    </Container>
  );
};

export default MuscleGroup;
