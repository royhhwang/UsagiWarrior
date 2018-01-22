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
    const data = {
      username: this.state.username,
      password: this.state.password
    }
    fetch("/api/login", {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(data)
  }).then((response)=> {
      if (response.status >= 400) {
        throw new Error("Bad response from server");
      }
      return response.json();
  }).then((data)=> {  
  console.log(data)    
      if(data.code === 200){
        this.props.history.push('/home');
         console.log('Working');  
      }
  }).catch((err)=> {
      console.log(err)
  });
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



  //  <div className="row small-up-2 medium-up-3 large-up-4">
  //     <div className="column">
  //       <h2>Login Page</h2>
  //  <label>Username</label>
  //     <input type='text' name='username' placeholder='Username' onChange={this.onChange}/>
  //   <label>Password</label>
  //     <input type='password' name='password' placeholder='Password' onChange={this.onChange}/>
  //   <Link to='/home' type='submit' value='Login' className='button' onClick={this.login} method='POST'>Login</Link>
  // </div>
  // </div>

    );
  }
}

export default Login;
