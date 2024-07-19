import React, { useEffect, useState } from 'react';
import '../Static/css/problemset.css';
import '../Static/css/App.css';
import NavBar from '../Components/NavBar'
import ProblemBar from '../Components/ProblemBar'
import { Outlet, useLocation, useParams } from 'react-router-dom';
import axios from '../middlewares/axiosMiddleware';
import Loader from '../Components/loader';
interface back_data {
    _id:any,
    tags:string[],
    problemName:string,
    rating:Number,
    contest_id:any,
    problemStatement:string,
    timelimit:Number
  }
const ProblemStatementLayout = ()=>{
    const [data,setData] = useState<back_data>();
    const [Loading,setLoading] = useState<boolean>(true);
    const {problemName} = useParams();
    const location_ai = useLocation()
    const [statting,_] = useState<any>(location_ai.state)
    console.log(statting)
    const fect_data = async ()=>{
        setLoading(true)
            try{
                const data = await axios.get(`/Poblems/${problemName}`)
                setData(x=>data.data)
                setLoading(false)
            }catch(ex){}
    }
    useEffect(()=>{
            fect_data();
    },[])
    if(Loading){
        return <Loader/>
    }
    return (
     <div className='App'>
         <ProblemBar/>
         <Outlet context={[data,statting]}/>
    </div>
    )
}

export default ProblemStatementLayout;
