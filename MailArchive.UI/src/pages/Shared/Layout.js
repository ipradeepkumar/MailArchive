import React from "react";
import { Outlet } from "react-router-dom";
import Navigation from "../../Components/Navigation";
import { Container } from "react-bootstrap";
import Header from "./Header"
import Footer from "./Footer";

function Layout() {
    return (
        <Container fluid no-gutters="true">
            <Header />
            <div className="row">
                <div className="col-md-2">
                    <Navigation />
                </div>
                <div className="col-md-10 body-content"><Outlet /></div>
            </div>
           <Footer />
        </Container>
    );
}

export default Layout;