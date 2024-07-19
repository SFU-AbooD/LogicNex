import { Link, useNavigate } from "react-router-dom";
import "../Static/css/plans.css";
import { useEffect, useState } from "react";
import axios from "../middlewares/axiosMiddleware";
import Loader from "../Components/loader";
const Education = () => {
    const GetUserPlans = async ()=>{
        try{
            const data = await axios.get("Plan/get")
            setData(data.data)
        }catch(ex){}

        setLoading(false)
    }
    useEffect(()=>{
        GetUserPlans()
    },[])
    const navigate = useNavigate();
    const [data,setData] = useState<any[]>()
    const [Loading,setLoading] = useState<boolean>(true)
  const [Plans,setPlans] = useState<any[]>();
  if(Loading)
    return <Loader/>

  return (
    <div className="main">
        {
        data && data.length > 0 ? 
        <div className="plans-body">
        <div className="uppers"> 
        <h1 className="mr-10">Generated Plans</h1>
        <button onClick={()=>{
            navigate('/generate')
        }} className="button-5">Create new Plan</button>
        </div>
        {data.map(x=>{
            return <div className="plan-inside">
            <h4>Plan: {x.planName} </h4>
            <button onClick={()=>{
                navigate(`/plans/${x.planName}`)
            }} className="button-5">Go to</button>
        </div>
        })}

       </div>
        : 
        <div className="no-plans-generate">
            <h1>You Don`t Have Any Plans</h1>
            <button onClick={()=>{
                navigate('/generate')
            }} className="button-5">Generate One</button>
            </div>}
    </div>
  );
};

export default Education;
