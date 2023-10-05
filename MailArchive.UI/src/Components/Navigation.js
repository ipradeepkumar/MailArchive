import React from "react";
import { Nav } from "react-bootstrap";
import { Link } from "react-router-dom";
import styleClasses from "./Navigation.module.css";

function Navigation(){
    return (
        <Nav className="flex-column">
            <Nav.Link as={Link} to="/home">Home</Nav.Link>
            <Nav.Link as={Link} to="/about">About</Nav.Link>
        </Nav>
    );
}

export default Navigation;