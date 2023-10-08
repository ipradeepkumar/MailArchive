import React, { Component } from 'react';
import '../App.css'
import { Container, Row, Col } from 'react-bootstrap';

class MapData extends Component {
  constructor(props) {
    super(props);
  }
  state = {
    tasks: [
        {id: "1", taskName:"Read book", type:"inProgress", backgroundColor: "red", top: '100px', left: '226px', isActive: false},
        {id: "2", taskName:"Pay bills", type:"inProgress", backgroundColor:"green", top: '115px', left: '226px', isActive: false},
        {id: "3", taskName:"Go to the gym", type:"Done", backgroundColor:"blue", top: '130px', left: '226px', isActive: false},
        {id: "4", taskName:"Play baseball", type:"Done", backgroundColor:"green", top: '145px', left: '226px', isActive: false}
    ]
  }
  componentDidMount(){
    document.addEventListener("keydown", this.onKeyDown);
  }
  onDragStart = (event, taskName) => {
    console.log('dragstart on div: ', taskName);
    event.dataTransfer.setData("taskName", taskName);
    let newTasks = this.state.tasks.map(task=>{
      if(taskName === task.taskName)
      {   
          let newTask={
              ...task,
              isActive:true
          }
          return newTask   
      }
      else
      {
          return task
      }
});

this.setState({
   tasks:newTasks
});
  }
onDragOver = (event) => {
    event.preventDefault();
}
onDragEnd = (event, taskName) => {
  let tasks = this.state.tasks.filter((task) => {
    if (task.taskName == taskName) {
        task.left = event.pageX - 10 + 'px';
        task.top = event.pageY - 10 + 'px';
    }
    return task;
});

this.setState({
    ...this.state,
    tasks
});
}

onKeyDown = (event) => {
let task = this.state.tasks.filter((task) => {
    if (task.isActive == true) return task;
});

console.log(task);

  switch (event.keyCode) {
    case 37://left
    task.left = task.left - 10;
      break;
    case 38://up
    task.top = task.top - 10;
      break;
    case 39://right
      task.left = task.left + 10;
      break;
    case 40://down
      task.top = task.top + 10;
      break;
    default:
      break;
  }
  this.setState({
    ...this.state,
    task
});
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
      <span key={task.id} 
        onDragStart = {(event) => this.onDragStart(event, task.taskName)}
        draggable
        onDragEnd= {(e) => this.onDragEnd(e, task.taskName)}
        style = {{backgroundColor: task.bgcolor, position:'absolute', left: task.left, top: task.top}}>
        {task.taskName}
      </span>
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
          <Col md={10} className="droppable" style={{ height: '200px'}}
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