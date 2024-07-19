import { PropsWithChildren, useState } from "react"
import { useSelector,useDispatch } from "react-redux";
import {RootState} from '../States/store';
import axios from '../middlewares/axiosMiddleware';
import {updateUserCredentials} from '../States/userslice';
import { useNavigate } from "react-router-dom";
import Loader from "../Components/loader";
export function AutoUserAck({children} : PropsWithChildren){
    const [Loading,setloading] = useState(true)
    const Dispatch = useDispatch()
    if(!Loading){
        return <>{children}</>
    }
    else{
        const fetch = async()=>{
            try{
                const response = await axios.get('/User/getUser',{withCredentials:true})
                if(response.status >= 200 && response.status <= 299){
                    Dispatch(updateUserCredentials({
                        email:response.data.email,username:response.data.username,Authenticataion_result:true
                    }))
                }
                else{
                    // here is an error about the email and password
                }
            }catch(ex){}
            setloading(false)
        }
        fetch()
        return <Loader/>
}
}