import React, { Component } from 'react';
import '../App.css'
import { Container, Row, Col } from 'react-bootstrap';

class MapData extends Component {
  constructor(props) {
    super(props);
  }
  state = {
    tasks: [
        {id: "1", taskName:"Read book",type:"inProgress", backgroundColor: "red"},
        {id: "2", taskName:"Pay bills", type:"inProgress", backgroundColor:"green"},
        {id: "3", taskName:"Go to the gym", type:"Done", backgroundColor:"blue"},
        {id: "4", taskName:"Play baseball", type:"Done", backgroundColor:"green"}
    ]
  }
  onDragStart = (event, taskName) => {
    console.log('dragstart on div: ', taskName);
    event.dataTransfer.setData("taskName", taskName);
}
onDragOver = (event) => {
    event.preventDefault();
}

onDrop = (event, cat) => {
    let taskName = event.dataTransfer.getData("taskName");

    let tasks = this.state.tasks.filter((task) => {
        if (task.taskName == taskName) {
            task.type = cat;
        }
        return task;
    });

    this.setState({
        ...this.state,
        tasks
    });
}
  render() {
    let tasks = {
      inProgress: [],
      Done: []
    }

  this.state.tasks.forEach ((task) => {
    tasks[task.type].push(
      <div key={task.id} 
        onDragStart = {(event) => this.onDragStart(event, task.taskName)}
        draggable
        className="draggable"
        style = {{backgroundColor: task.bgcolor}}>
        {task.taskName}
      </div>
    );
  });

    return (
      <Container fluid="md">
        <Row>
          <Col md={2} className="inProgress"
            onDragOver={(event)=>this.onDragOver(event)}
            onDrop={(event)=>{this.onDrop(event, "inProgress")}}>
            <span className="group-header">In Progress</span>
            {tasks.inProgress}
          </Col>
          <Col md={10} className="droppable" 
              onDragOver={(event)=>this.onDragOver(event)}
              onDrop={(event)=>this.onDrop(event, "Done")}>
            <span className="group-header">Done</span>
            {tasks.Done}
          </Col>
        </Row>        
      </Container>
    );
}

}

export default MapData;