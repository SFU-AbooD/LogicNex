import React, { FormEvent, FormEventHandler, FormHTMLAttributes, useState } from "react";
import '../Static/css/UserLayout.css'
import { NavLink, Outlet } from "react-router-dom";
const UserLayout = ()=>{
    return (
                <Outlet/>
    )
}

export default UserLayout