const express = require('express');
const login = require('./routes/loginroutes');
const bodyparse = require('body-parser');
const PORT = process.env.PORT || 4000;
const app = express();
app.use(bodyparse.urlencoded({
  extended: true
}));
app.use(bodyparse.json());

app.use(function (datarequest, dataresults, next) {
  dataresults.header("Access-Control-Allow-Origin", "*");
  dataresults.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});
const router = express.Router();
router.get('/', (req,res)=>{
  res.json({message:'Welcome to our upload module apis'});
});

router.post('/signup', login.signup);
router.post('/login', login.login);
app.use('/api', router);
app.listen(PORT);