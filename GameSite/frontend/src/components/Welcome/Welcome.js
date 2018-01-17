import React, { Component } from 'react';
import './Welcome.css'
class Welcome extends Component {
  render() {
    return (
     <div>
   <div className="row small-up-2 medium-up-3 large-up-4">
   <div className="column">
     <h2>Welcome Page</h2>
     <p> We are making a game! Signup to check it out!</p>
     <p> If you are already a member click Login!</p>
     <a href='/login' className='button'> Login</a>
     <a href='/signup' className='button success'> Signup</a>
   </div>
   
   </div>
  </div>
    );
  }
}

export default Welcome;
