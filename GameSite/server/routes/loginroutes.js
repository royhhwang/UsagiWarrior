import express from "express";
import controller from "../controllers/loginControllers"

const router = express.Router();

router.get('/', (req,res)=>{
    res.json('Welcome to our upload module apis');
  }); 
  router.post('/signup', controller.signup);
  router.post('/login', controller.login);
// Export routes for server.js to use.
export default router;
