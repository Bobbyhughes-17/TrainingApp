import React from "react";
import { Container, Row, Col } from "react-bootstrap";

const Footer = () => {
  return (
    <footer style={footerStyle}>
      <Container>
        <Row>
          <Col className="text-center py-3">
            © {new Date().getFullYear()} GitFit App. All Rights Reserved.
          </Col>
        </Row>
      </Container>
    </footer>
  );
};

const footerStyle = {
  backgroundColor: "#333",
  color: "#fff",
  position: "fixed",
  width: "100%",
  bottom: 0,
};

export default Footer;
