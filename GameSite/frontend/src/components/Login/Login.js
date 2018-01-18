import React, { Component } from 'react';
import {Redirect} from 'react-router-dom';
import './Login.css'
class Login extends Component {
  constructor(props){
    super(props);
    this.state = {
      username: '',
      password: '',
      redirect: false
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
         this.setState({msg: "Welcome: "+ this.data.username});  
      }
  }).catch(function(err) {
      console.log(err)
  });
  }

  onChange(e){
    this.setState({[e.target.name]: e.target.value });
  }
  render() {
    if(this.state.redirect){
return(<Redirect to={'/home'}/>);

    }

    return (
      <div>
   <div className="row small-up-2 medium-up-3 large-up-4">
   <div className="column">
     <h2>Login Page</h2>
   <label>Username</label>
   <input type='text' name='username' placeholder='Username' onChange={this.onChange}/>
   <label>Password</label>
   <input type='password' name='password' placeholder='Password' onChange={this.onChange}/>
   <button type='submit' value='Login' className='button' onClick={this.login} method='POST'/>
   </div>
   
   </div>
  </div>
    );
  }
}

export default Login;
