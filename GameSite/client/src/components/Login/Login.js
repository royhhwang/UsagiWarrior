import React, { Component } from 'react';
import {Link} from 'react-router-dom';
import './Login.css'

class Login extends Component {
  constructor(props){
    super(props);
    this.state = {
      username: '',
      password: ''
      
    }
    this.login = this.login.bind(this);
    this.onChange = this.onChange.bind(this);
  }
  login(event){
    event.preventDefault();
    const requestBody = {
      username: this.state.username,
      password: this.state.password
    }
    fetch("/api/login", {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(requestBody)
  })
  .then((data) =>{
    console.log(data);
      if(data.status === 200){
        this.props.history.push('/home');
      }
  })
  }

  onChange(e){
    this.setState({[e.target.name]: e.target.value });
  }
  render() {
    return (
      <div>
<form className='columns medium-4'>
  <div className="form-icons">
    <h4>Login!</h4>

    <div className="input-group">
        <label id="usernameLabel"></label>
      <input type='text' name='username' placeholder='Username' onChange={this.onChange}/>      
    </div>

    <div className="input-group"> 
      <label id="passwordLabel"></label>
      <input type='password' name='password' placeholder='Password' onChange={this.onChange}/>  
    </div>
  </div>
  <button className="button expanded" type='submit' value='Login' onClick={this.login} method='POST'>Login</button>
</form>

<div>
<p>Not a Member? Click below!</p>
     <Link to='/signup' className='button success'> Signup</Link>
</div>
</div>

    );
  }
}

export default Login;
