import React from "react";
import routing , {NavLink} from '../routes';
import '../Static/css/problemset.css'
const ProfileBar = ()=>{
    return (
    <div className='problemBar'>
        <ul>
            <NavLink to='./Profile'><li>Profile</li></NavLink>
            <NavLink to='./Settings'><li>Settings</li></NavLink>
        </ul>
    </div>
    )
}

export default ProfileBar