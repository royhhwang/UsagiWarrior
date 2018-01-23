import hash from 'bcrypt';

const db = require("../models");
const bcrypt = require('bcrypt');
// Defining methods for the loginController
const controller = {
  signup: (req, res) => {
    let newUser = {};
    newUser.password = bcrypt.hashSync(req.body.password, 5);
    db.Users.create({
      username: req.body.username,
      password: newUser.password
    }).then(()=>{
      res.json({worked: true});
    }).catch((err => {res.send(err);
    }));
  },
  login:(req, res) =>{
    const password = req.body.password;
    console.log('1');
    db.Users.findOne({
      where: {username: req.body.username}
    })
  .then(dbModel => {bcrypt.compare(password, req.body.password);
  if(dbModel){
    res.json(dbModel);
  }
  })
  .catch((err => {res.send(err);
  }))
 


    // // User.create(bcrypt.hash(req.body.password, 5, (err, bcryptedPassword) => {
    // //   const users = {
    // //     'username': req.body.username,
    // //     'password': bcryptedPassword
    // //   }})).then(()=> User.findOrCreate({where: {username:req.body.username, password: users.bcryptedPassword}}));
      
    //   bcrypt.hash(req.body.password, 5, (err, bcryptedPassword) => {
    //     const users = {
    //       'username': req.body.username,
    //       'password': bcryptedPassword
    //     }
    //    sequelize.query('INSERT INTO `users` SET ?',{ type: sequelize.QueryTypes.INSERT}).then(users =>{
    //     if (error) {
    //       console.log('There was an error.', error);
    //       res.send({
    //         'code': 400,
    //         'failed': 'There was an error.'
    //       });
    //     } else {
    //       console.log('User registered, info: ', response);
    //       res.send({
    //         'code': 200,
    //         'success': 'User registered successfully.'
    //       });
    //     }
    //   }));
    //   });
    // }
  //   bcrypt.hash(req.body.password, 5, (err, bcryptedPassword) => {
  //     const users = {
  //       'username': req.body.username,
  //       'password': bcryptedPassword
  //     }
  //    sequelize.query('INSERT INTO users SET ?', users, (error, response, fields) => {
  //       if (error) {
  //         console.log('There was an error.', error);
  //         res.send({
  //           'code': 400,
  //           'failed': 'There was an error.'
  //         });
  //       } else {
  //         console.log('User registered, info: ', response);
  //         res.send({
  //           'code': 200,
  //           'success': 'User registered successfully.'
  //         });
  //       }
  //     });
  //   });
  // },
  
  // login: (req, res) => {
  //   const username = req.body.username;
  //   const password = req.body.password;
  //   sequelize.query('SELECT * FROM users WHERE username = ?', [username], (error, results, fields) => {
  //     if (error) {
  //       res.send({
  //         'code': 400,
  //         'failed': 'Something went wrong'
  //       });
  //     } else {
  //       if (results.length > 0) {
  //         bcrypt.compare(password, results[0].password, (err, doesMatch) => {
  //           if (doesMatch) {
  //             res.send({
  //               'code': 200,
  //               'success': 'Login Successfull'
  //             });
  //           } else {
  //             res.send({
  //               'code': 204,
  //               'success': 'U & P doesn\'t match'
  //             });
  //           }
  //         });
  //       } else {
  //         res.send({
  //           'code': 205,
  //           'success': 'Something went wrong'
  //         })
  //       }
  //     }
  //   });
  }
}
export { controller as default };
