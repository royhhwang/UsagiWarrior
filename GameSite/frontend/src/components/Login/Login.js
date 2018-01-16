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
  login(){
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
   <input type='submit' value='Login' className='button' onClick={this.login}/>
   </div>
   
   </div>
  </div>
    );
  }
}

export default Login;
