import React from "react";
import routing , {NavLink} from '../routes';
import '../Static/css/problemset.css'
import { useParams } from "react-router-dom";
const ProblemBar = ()=>{
    const {problemName} = useParams();
    return (
    <div className='problemBar'>
        <ul>
            <NavLink to={`/problems/${problemName}`}><li className="bold">Statement</li></NavLink>
            <NavLink to='./Submission'><li className="bold">Submit code</li></NavLink>
        </ul>
    </div>
    )
}

export default ProblemBar