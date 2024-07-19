import '../Static/css/newlogin.css'
import { useNavigate } from "react-router-dom";
import { useSelector,useDispatch } from "react-redux";
import {RootState} from '../States/store';
import {updateUserCredentials} from '../States/userslice';
import { FormEvent } from 'react';
import axios from '../middlewares/axiosMiddleware';
interface Login {
    Email:string,
    password:string
}
const Login_new =  ()=>{
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
                    <span className='header--login-top'>Login your account</span>
                </div>
                <form className='auth-body' onSubmit={submit_handle}>
                <input name='username' placeholder='Your Email' className='input--auth' type='text'/>
                <input name='password' placeholder='Your Password' className='input--auth' type='password'/>
                <button className='btn_submit-login' type='submit'>Log In</button>
                </form>
            </div>
        </div>
}



export default Login_new