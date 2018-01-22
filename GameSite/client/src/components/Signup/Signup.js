import React, { Component } from 'react';
import {Link} from 'react-router-dom';

import './Signup.css'
class Signup extends Component {
  constructor(props){
    super(props)
      this.state = {
        username: '',
        password: '',
        passwordConfirm: ''
      }
      this.handleSubmit = this.handleSubmit.bind(this);
      this.onChange = this.onChange.bind(this);
    }
    showFormErrors(){
      const inputs = document.querySelectorAll('input');
      let isFormValid = true;
      inputs.forEach(input => {
        input.classList.add('active');
        console.log('input', input)
        const isInputValid = this.showInputError(input.name);
          if(!isInputValid){
            isFormValid = false;
          }
      });
      return isFormValid;
    }
    showInputError(refName){
      const validity = this.refs[refName].validity;
      console.log('valid',validity);
      const label = document.getElementById(`${refName}Label`).textContent;
      const error = document.getElementById(`${refName}Error`);
      const isPassword = refName.indexOf('password')!== -1;
      const isPasswordConfirm = refName === 'passwordConfirm';
        if (isPasswordConfirm){
          if(this.refs.password.value !== this.refs.passwordConfirm.value){
            this.refs.passwordConfirm.setCustomValidity('Passwords dod not match');
          }else{
            this.refs.passwordConfirm.setCustomValidity('');
          }
        }
        if(!validity.valid){
          if(validity.valueMissing){
            error.textContent = `${label} is a required field`;
          }else if(isPassword && validity.patternMismatch){
            error.textContent = `${label} should be longer than 4 chars`;
          }else if(isPasswordConfirm && validity.customError){
            error.textContent = 'Passwords do not match';
          }
          return false;
        }
        error.textContent = '';
        return true;
    }

    handleSubmit(event){
      event.preventDefault();
      
      if(this.showFormErrors()){
        const data = {
          username: this.state.username,
          password: this.state.password
        }
        console.log('form is valid: submit data');
        fetch("/api/signup", {
          method: 'POST',
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(data)
      }).then((response) =>{
          if (response.status >= 400) {
            throw new Error("Bad response from server");
          }
          return response.json();
      }).then((data) =>{
          console.log(data)    
          if(data.code === 200){
            this.props.history.push('/home');
             console.log('Working');  
          }
      }).catch((err)=>{
          console.log(err)
      });
      }else{
        console.log('form is invalid: do not submit');
      }
      
      
      
    }
    onChange(e){
       e.target.classList.add('active');
      this.setState({
        [e.target.name]: e.target.value 
      });
      this.showInputError(e.target.name);
      }

  render(){
    return (
      <div>
      <form className='columns medium-4'>
  <div className="form-icons">
    <h4>Signup!</h4>

    <div className="input-group">
        <label id="usernameLabel"></label>
      <input className="input-group-field" type="text" placeholder="Username"
            name="username"
            ref="username"
            value={ this.state.username } 
            onChange={ this.onChange }
            required />
            <div className="error" id="usernameError" />
    </div>

    <div className="input-group">
      <label id="passwordLabel"></label>
      <input className="input-group-field" type="password" placeholder="Password"
            name="password"
            ref="password"
            value={ this.state.password } 
            onChange={ this.onChange }
            pattern=".{5,}"
            required />
            <div className="error" id="passwordError" />
    </div>

    <div className="input-group">
      <label id="passwordConfirmLabel"></label>
      <input className="input-group-field" type="password" placeholder="Confirm Password"
            name="passwordConfirm"
            ref="passwordConfirm"
            value={ this.state.passwordConfirm } 
            onChange={ this.onChange }
            required />
            <div className="error" id="passwordConfirmError" />
    </div>
  </div>

  <button className="button expanded success" type='submit' value='Signup' onClick={this.handleSubmit} method='POST'>Sign Up</button>
</form>
<div>
  <p>Already a member? Click below!
  </p>
  <Link to='/login' className='button'> Login</Link>
</div>
</div>
  // <form className='form-group row small-up-2 medium-up-3 large-up-4 medium-4 cell columns '>
  // <h2>Signup Page</h2> 
        
  //         <label id="usernameLabel">Username</label>
  //         <input className="form-control"
  //           type="text"
  //           name="username"
  //           ref="username"
  //           value={ this.state.username } 
  //           onChange={ this.onChange }
  //           required />
  //         <div className="error" id="usernameError" />
   
       
  //         <label id="passwordLabel">Password</label>
  //         <input className="form-control"
  //           type="password" 
  //           name="password"
  //           ref="password"
  //           value={ this.state.password } 
  //           onChange={ this.onChange }
  //           pattern=".{5,}"
  //           required />
  //         <div className="error" id="passwordError" />
        
        
  //         <label id="passwordConfirmLabel">Confirm Password</label>
  //         <input className="form-control"
  //           type="password" 
  //           name="passwordConfirm"
  //           ref="passwordConfirm"
  //           value={ this.state.passwordConfirm } 
  //           onChange={ this.onChange }
  //           required />
  //         <div className="error" id="passwordConfirmError" />
  //     <button type='submit' value='Signup' className='button' onClick={this.handleSubmit} method='POST'>Sign up!</button>
  //   </form>
    );
  }
}

export default Signup;