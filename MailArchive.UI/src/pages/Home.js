    import React from "react";
    import { Tab, Tabs } from "react-bootstrap";
    
import UploadData from "./uploaddata";
import MapData from "./mapdata";
    function Home(){
        return (
            <Tabs
            defaultActiveKey="uploaddata"
            id="justify-tab-example"
            className="col-md-10"
            justify
          >
            <Tab eventKey="uploaddata" title="Upload Data">
                <UploadData />
            </Tab>
            <Tab eventKey="uploadtemplate" title="Upload Template">
             
            </Tab>
            <Tab eventKey="mapdata" title="Map Data">
                <MapData />
            </Tab>
            <Tab eventKey="view" title="View">
             
            </Tab>
            <Tab eventKey="save" title="Save">
             
             </Tab>
          </Tabs>
               
            );
    }

    export default Home;