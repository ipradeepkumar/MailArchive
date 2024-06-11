    import React from "react";
    import { Tab, Tabs } from "react-bootstrap";
    import { Nav } from "react-bootstrap";
    import { Link } from "react-router-dom";
    import UploadData from "./uploaddata";
    import MapData from "./mapdata";
    import UploadTemplate from "./uploadtemplate";
    
    
    function Home(){
        return (
            <React.Fragment>
                <Tabs defaultActiveKey="uploaddata" id="justify-tab-example" className="col-md-10" justify>
                    <Tab eventKey="uploaddata" title="Upload Data">
                        <UploadData />
                    </Tab>
                    <Tab eventKey="uploadtemplate" title="Upload Template">
                        <UploadTemplate />
                    </Tab>
                    <Tab eventKey="mapdata" title="Map Data">
                        <MapData />
                    </Tab>
                    <Tab eventKey="view" title="View">             
                    </Tab>
                    <Tab eventKey="save" title="Save">             
                    </Tab>
            </  Tabs>
          </React.Fragment>
    );
}

export default Home;