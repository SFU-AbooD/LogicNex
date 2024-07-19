import React from 'react';
import './Static/css/problemset.css';
import './Static/css/App.css';
import { Outlet } from 'react-router-dom';
import NavBar from './Components/NavBar';
const Layout = ()=>{
    
    return (
     <div className='main'>
        
        <NavBar/>
        <Outlet/>
    </div>
    )
}

export default Layout;
