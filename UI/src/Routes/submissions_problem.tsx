import { useEffect, useState } from 'react'
import axios from '../middlewares/axiosMiddleware'
import '../Static/css/newset.css'
import Loader from '../Components/loader'
import { Link, useNavigate } from 'react-router-dom'
const New_problem_set = ()=>{
const fetch_data = async ()=>{
    setLoading(true)
    try{
        const data = await axios.post('Poblems/submissions/get')
        setdata(data.data)
        console.log(data)
        setLoading(false)
    }catch(ex){}
   
}

interface Problem {
    langauge:string;
    problem_name: string;
    rating: number;
    status: string;
    submission_id: string;
    contest_id?: number;  
    time?: number;  
    problemStatement?: string; 
}

const [item1,setitem1] = useState(false)
const [item2,setitem2] = useState(false)
const [item3,setitem3] = useState(false)
const [item4,setitem4] = useState(false)
const [item5,setitem5] = useState(false)
const [item6,setitem6] = useState(false)
const [item7,setitem7] = useState(false)
const [item8,setitem8] = useState(false)
const [data,setdata] = useState<Problem[]>([])
const [Loading,setLoading] = useState(true)
const navigate = useNavigate();
useEffect(()=>{
    fetch_data()
},[item1,item2,item3,item4,item5,item6,item7,item8])
let ans = 1;
if(Loading)
    return <Loader/>
return <div className="main">
    <div className='full-color'>
    <div className='set-navbar'>
        <div className='current-page'>
            <div className='flex-columns'>
                <div className='row-flex-item'>Submissions</div>
                <div className='row-flex-item mr bold'>Submissions</div>
            </div>
        </div>
    </div>
    <div className='main-container-problem_set'>
        <div className='problem-set-problems'>
            {data.length > 0?data.map(x=>{
                var color = "gray";
                if(x?.status === "Accepted")
                    color = "green"
                else if(x?.status.includes("Runtime")){
                    color = "purple"
                }
                else if(x?.status.includes("Running")){
                    color = "gray"
                }
                else if(x?.status.includes("Wrong")){
                    color = "red"
                }
                else if(x?.status.includes("limit")){
                    color = "Yellow"
                }
                console.log(color)
                return <div className='problem-card-set'>
                <div className='left-card-side'>
                <span className='problem-title-set'>{x.problem_name}</span>
                <span className='lower-item-'>
                    <span className='problem-title-color'>Time: {x.time} ms, Language: {x.langauge}, Max Score:15</span>
                </span>
                </div>
                
                <Link to={`/Verdict/${x.submission_id}`}>
                <div className='right-section'>
                    <span style={{color:color}} className='verdict-text'>{x.status}</span>
                </div>
                </Link>
            </div>
            }) : <div>No Submissions yet </div>}
        </div>
    </div>
    </div>
</div>




}


export  default New_problem_set