# Instructions:

## To run locally:
* create a .env.local file in the root directory similar to below, but with your database info:
```
DB_HOST=127.0.0.1
DB_USERNAME=root
DB_PASSWORD=ChangeMe123
DB_SCHEMA=example_db
DB_DIALECT=mysql
```
* run schema.sql simply to create the schema in mysql workbench/command line.  Sequelize will take care of creating tables based on models created in server/models folder.
* run ```yarn watch``` or ```npm run watch``` for instant update of client/server on changes and use localhost:3000 for any frontend work and localhost:3001 for any backend work.
* run ```yarn dev``` or ```npm run dev``` to build and run the built files locally.  Use localhost:3001 for both frontend and backend.  This is more similar to what would be deployed on prod.
* run ```yarn prod``` or ```npm run prod``` to build and run the built files locally.  Use localhost:3001 for both frontend and backend.  This is what would be deployed on prod.
* run ```yarn prod-build``` or ```npm run prod-build``` to build files for prod.  On prod server only files that need to be deployed are the build folder files.  The server just needs to run ```node app.js``` from the build folder to run.
