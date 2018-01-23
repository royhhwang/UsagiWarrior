import React, { Component } from 'react';
import './Header.css'

class Header extends Component {
  render() {
    return (
    <div>
  <div className="callout primary" id='newHeader'>
    <div className="row column">
      <h1>{this.props.name} Project</h1>
      <p className="lead">This will be frikkin' rad!</p>
    </div>
  </div>
  </div>
    );
  }
}

export default Header;
