require('./env');
module.exports = {
  //depending on where deployed, these could change.
  "username": process.env.DB_USERNAME,
  "password": process.env.DB_PASSWORD,
  "database": process.env.DB_SCHEMA,
  "host": process.env.DB_HOST,
  "dialect": process.env.DB_DIALECT
}
