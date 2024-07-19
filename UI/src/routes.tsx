import App from './Routes/App';
import ProblemStatementLayout from './Routes/ProblemDetails';
import ProblemsetLayout from './Routes/Problemset';
import {
  createRoutesFromElements,
    createBrowserRouter,
    RouterProvider,
    Link,
    Route,
    NavLink
  } from 'react-router-dom';
import StatmentBody from './Routes/StatmentBody';
import Standings from './Routes/Standings';
import ProblemDetails from './Routes/ProblemDetails';
import Submission from './Routes/Submission';
import Layout from './layout';
import Standings_group from './Routes/Standings';
import Login from './Components/login';
import Signup from './Components/signup';
import UserLayout from './Routes/userLayout';
import Confirm_email from './Routes/Confirm_email';
import Verdict from './Routes/verdict';
import Profile from './Routes/profile';
import Education from './Routes/education';
import GeneratePlans from './Routes/GeneratePlan';
import Plans from './Routes/plans';
import New_problem_set from './Routes/new_problemset';
import Submission_page from './Routes/submissions_problem';
import NewLogin from './Components/newlogin';
import Login_new from './Components/logingood';
import Login_new2 from './Components/register';
const routers = createBrowserRouter(createRoutesFromElements(
<Route>
  <Route path='/' element={<Layout/>}>
  <Route path='logout' element={<NewLogin/>}></Route>
  <Route path='login-new' element={<Login_new/>}></Route>
  <Route path='plans/:planName' element={<Plans/>}></Route>
  <Route path='generate' element={<GeneratePlans/>}></Route>
  <Route path='education' element={<Education></Education>}></Route>
  <Route path='/Submissions' element={<Submission_page/>}></Route>
  <Route path='Verdict/:VerdictID' element={<Verdict/>}></Route>
  <Route path='/problems/:problemName' element={<ProblemStatementLayout/>}>
    <Route path='Submission' element={<Submission/>}></Route>
    <Route index element={<StatmentBody/>}></Route>
  </Route>
    <Route index element={<App/>}></Route>
    <Route path='/problemset' element={<New_problem_set/>}></Route>
    <Route path='/User' element={<UserLayout/>}>
    <Route path='/User/login' element={<Login_new/>}/>
    <Route path='/User/signup' element={<Login_new2/>}/>
    </Route>
    <Route path='/User/confirm' element={<Confirm_email/>}/>
    </Route>
</Route>
))
export default routers
export {Link,RouterProvider,NavLink}