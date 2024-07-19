import React, { FormEvent, FormEventHandler, FormHTMLAttributes, useEffect } from "react";
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
const NewLogin = ()=>{
    const Dispatch = useDispatch()
    //const store = useSelector((x:RootState)=>x.user)
    useEffect(()=>{
        submit_handle()
    },[])
    const navigate = useNavigate(); 
    const submit_handle = async ()=>{
            try{
                const response = await axios.get('User/logout')
                Dispatch(updateUserCredentials({
                    email:null,username:null,Authenticataion_result:false
                }))
                navigate("/problemset")
                }catch(ex){}

        }
        return (
            <></>
        )
    }



export default NewLogin