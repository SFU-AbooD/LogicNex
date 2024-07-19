import React, { FormEvent, FormEventHandler, FormHTMLAttributes } from "react";
import '../Static/css/login.css'
import axios from '../middlewares/axiosMiddleware';
import { useNavigate } from "react-router-dom";
import { useSelector,useDispatch } from "react-redux";
import {RootState} from '../States/store';
import {updateUserCredentials} from '../States/userslice';
interface Login {
    Email:string,
    password:string
}
const Login = ()=>{
    const Dispatch = useDispatch()
    //const store = useSelector((x:RootState)=>x.user)

    const navigate = useNavigate(); 
    const submit_handle = async (event:FormEvent<HTMLFormElement>)=>{
            event.preventDefault();
            const form_values : Login = {
                Email:event.currentTarget.username.value,
                password:event.currentTarget.password.value,
            }
            try{
                const response = await axios.post('/User/Login',form_values)
                if(response.status >= 200 && response.status <= 299){
                    const getUserName =  await axios.get(`/User/username?email=${form_values.Email}`)
                    const username:string = getUserName.data.username
                    Dispatch(updateUserCredentials({
                        email:form_values.Email,username:username,Authenticataion_result:true
                    }))
                    navigate("/problemset")
                }
                else{
                    // here is an error about the email and password
                }
            }catch(ex){}
    }
    return (
        <form className="login" onSubmit={submit_handle}>
            <div>
            <span><h3>Email</h3></span>
            <input name="username" type="text"/>
            </div>
            <div>
            <span><h3>Password</h3></span>
            <input  name="password" type="password"/>
            </div>
            <div>
                <button className="loginBtn">Login</button>
            </div>

        </form>
    )
}

export default Login