import React, { Component } from 'react';
import {Redirect, Link, withRouter} from 'react-router-dom';
import './Login.css'

class Login extends Component {
  constructor(){
    super();
    this.state = {
      username: '',
      password: ''
      
    }
    this.login = this.login.bind(this);
    this.onChange = this.onChange.bind(this);
  }
  login(event){
    event.preventDefault();
    const data = {
      username: this.state.username,
      password: this.state.password
    }
    fetch("/api/login", {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(data)
  }).then(function(response) {
      if (response.status >= 400) {
        throw new Error("Bad response from server");
      }
      return response.json();
  }).then(function(data) {  
  console.log(data)    
      if(data === "success"){
         this.setState({msg: 'Welcome'});
         console.log('Working');  
      }
  }).catch(function(err) {
      console.log(err)
  });
  }

  onChange(e){
    this.setState({[e.target.name]: e.target.value });
  }
  render() {
    return (
   <div className="row small-up-2 medium-up-3 large-up-4">
      <div className="column">
        <h2>Login Page</h2>
   <label>Username</label>
      <input type='text' name='username' placeholder='Username' onChange={this.onChange}/>
    <label>Password</label>
      <input type='password' name='password' placeholder='Password' onChange={this.onChange}/>
    <Link to='/home' type='submit' value='Login' className='button' onClick={this.login} method='POST'>Login</Link>
  </div>
  </div>

    );
  }
}

export default Login;
