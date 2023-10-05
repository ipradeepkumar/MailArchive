import React from 'react'
import "bootstrap/dist/css/bootstrap.min.css";
import { Row, Col } from 'react-bootstrap';
function Header() {
  return (
    <div className="sticky-header">
        <Row>
          <Col md={2}>Project Name</Col>
          <Col md={8}></Col>
          <Col md={2}>Logout</Col>
        </Row>
    </div>
  )
}

export default Header;
