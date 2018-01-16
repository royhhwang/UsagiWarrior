const mysql = require('mysql');
const bcrypt = require('bcrypt');
const connection = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: '3Nkerman',
  database: 'users_DB'
});
connection.connect((err) => {
  if (!err) {
    console.log('DB is connected...');
  } else {
    console.log('DB went wrong');
  }
});

exports.signup = (req, res) => {
  bcrypt.hash(req.body.password, 5, (err, bcryptedPassword) => {
    const users = {
      'username': req.body.username,
      'password': bcryptedPassword
    }
    connection.query('INSERT INTO users SET ?', users, (error, response, fields) => {
      if (error) {
        console.log('There was an error.', error);
        res.send({
          'code': 400,
          'failed': 'There was an error.'
        });
      } else {
        console.log('User registered, info: ', response);
        res.send({
          'code': 200,
          'success': 'User registered successfully.'
        });
      }
    });
  });
}
exports.login = (req, res) => {
  const username = req.body.username;
  const password = req.body.password;
  connection.query('SELECT * FROM users WHERE username = ?', [username], (error, response, fields) => {
    if (error) {
      res.send({
        'code': 400,
        'failed': 'Something went wrong'
      });
    } else {
      if (res.length > 0) {
        bcrypt.compare(password, response[0].password, (err, doesMatch) => {
          if (doesMatch) {
            res.send({
              'code': 200,
              'success': 'Login Successfull'
            });
          } else {
            res.send({
              'code': 204,
              'success': 'U & P doesn\'t match'
            });
          }
        });
      } else {
        res.send({
          'code': 204,
          'success': 'Something went wrong'
        })
      }
    }
  });
}