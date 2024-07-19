import React, { FormEvent, FormEventHandler, FormHTMLAttributes, useState } from "react";
import '../Static/css/login.css'
import axios from '../middlewares/axiosMiddleware';
import { useNavigate } from "react-router-dom";
import { resolve } from "path";
interface signup {
    username:string,
    password:string,
    email:string,
}
const Signup = ()=>{
    const navigate = useNavigate();
    const [loading,setloading] = useState(false)
    const submit_handle = async (event:FormEvent<HTMLFormElement>)=>{
        event.preventDefault();
        const form_values : signup = {
            username:event.currentTarget.username.value,
            email:event.currentTarget.email.value,
            password:event.currentTarget.password.value,
        }
        if(form_values.password === event.currentTarget.confirmPassword.value){
            setloading(loading=>true);
            const response = await axios.post('/User/SignUp',form_values)
            if(response.status >= 200 && response.status <= 299){
                navigate('/User/confirm');
            }
            else{
                console.log(response.data)
            }
           setloading(loading=>false);
        }
        else{
    }
    }
    return (
        <form className="login" onSubmit={submit_handle}>
           <div>
            <span><h3>Handle</h3></span>
            <input name="username" type="text"/>
            </div>
            <div>
            <span><h3>Email</h3></span>
            <input   name="email" type="text"/>
            </div>
            <div>
            <span><h3>Password</h3></span>
            <input  name="password" type="text"/>
            </div>
            <div>
            <span><h3>Confirm Password</h3></span>
            <input  name="confirmPassword" type="text"/>
            </div>
            <div>
            <button className="loginBtn">Login</button>
            {loading && <span>Loader!</span>}
            </div>

        </form>
    )
}

export default Signup