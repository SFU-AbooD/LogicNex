import React from "react";
import '../Static/css/App.css'
import routing , {Link} from '../routes';
import { useSelector } from "react-redux";
import { RootState } from "../States/store";
const NavBar = ()=>{
    const store = useSelector((x:RootState)=>x.user)
    return <div className="Navbar">
            <div className="Left">
            <Link to='/'>LogicNex</Link>
            </div>
                <ul className="Right">
                <li><Link to='/'>Home</Link></li>
               { 
               // <li><Link to='/contest' >contest</Link></li>
               }
                <li><Link to='/problemset'>ProblemSet</Link></li>
                <li><Link to='/submissions' >Submissions</Link></li>
                {store.isAuthenticated == false ? <>                <li><Link to='/User/login' >SignIn</Link></li>
                <li><Link id="sign" to='User/signup' >SignUp</Link></li></> :
               <>
                <Link to='problemset'>hello,{store.username}</Link>
                <Link to='logout'>Logout</Link>
                </>
                }
                </ul>
    </div>
}

export default NavBar