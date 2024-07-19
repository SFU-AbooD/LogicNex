import { Link, useNavigate } from "react-router-dom";
import "../Static/css/plans.css";
import axios from "../middlewares/axiosMiddleware";
import { useEffect, useRef, useState } from "react";
import Loader from "../Components/loader";
const GeneratePlans = () => {
    const ref_name = useRef<any>(null)
    const ref_area = useRef<any>(null)
    const navigate = useNavigate();
    const [Loading,setLoading] = useState<boolean>(false);
    const PostPlanRequest = async ()=>{
        try{
             setLoading(true)
                const data = await axios.post("/Plan/create",{
                    name:ref_name.current!.value,
                    explain:ref_area.current!.value,
                })
                navigate('/Education')
        }catch(ex){}
        setLoading(false)
    }
if(Loading){
        return <Loader/>
    }
    else{
  return (
    <div className="main">
        <div className="plan-generation">
                    <h5 className="w-50 starting">Provide a plan name</h5>
                    <input placeholder="Insert a plan name " ref={ref_name}  className="div-50" type="text"></input>
                    <br/>
                    <br/>
                    <br/>
                    <h5 className="w-50 starting">Tell us about yourself</h5>
                    <textarea ref={ref_area} maxLength={4000} placeholder="tell us about yourself , what you know and all of your experince" className="div-50-100" ></textarea>
                    <br/>
                    <br/>
                    <button onClick={()=>{
                        PostPlanRequest()
            }} className="button-5">Generate Your plan $2</button>
            </div>
    </div>
  );
};
}

export default GeneratePlans;
