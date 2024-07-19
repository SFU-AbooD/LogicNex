import '../Static/css/newlogin.css'
import { useNavigate } from "react-router-dom";
import { useSelector,useDispatch } from "react-redux";
import {RootState} from '../States/store';
import {updateUserCredentials} from '../States/userslice';
import { FormEvent, useState } from 'react';
import axios from '../middlewares/axiosMiddleware';
interface signup {
    username:string,
    password:string,
    email:string,
}
const Login_new2 =  ()=>{
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
    return <div className="splitter-data-type">
            <div className='left-side-panel'>
                <video muted loop autoPlay playsInline className='constrain'>
                    <source  src='https://hrcdn.net/fcore/assets/onboarding/globe-5fdfa9a0f4.mp4' type='video/mp4'></source>
                </video>
                <div className='content-left-side-panel'>
                Welcome Back
                <br/>
                LogicNex is the next coding generation
                </div>
            </div>
            <div className='right-side-panel'>
                <div className='header-welcome-login'>
                    <span className='header--login-top'>Welcome Back!</span>
                    <span className='header--login-top'>Signup your account</span>
                </div>
                <form className='auth-body' onSubmit={submit_handle}>
                <input name='username' placeholder='Your Username' className='input--auth' type='text'/>
                <input name='email' placeholder='Your email' className='input--auth' type='text'/>
                <input name='password' placeholder='Your Password' className='input--auth' type='password'/>
                <input name='confirmPassword' placeholder='Confirm password' className='input--auth' type='password'/>
                <button className='btn_submit-login' type='submit'>Log In</button>
                </form>
            </div>
        </div>
}



export default Login_new2