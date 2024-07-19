import { useEffect, useState } from 'react'
import axios from '../middlewares/axiosMiddleware'
import '../Static/css/newset.css'
import Loader from '../Components/loader'
import { useNavigate } from 'react-router-dom'
const New_problem_set = ()=>{
const fetch_data = async ()=>{
    setLoading(true)
    var Solved = new Array();
    var Ratings = new Array();
    var Types = new Array();
    if(item1 == true)
        Solved.push("s");
    if(item2 == true)
        Solved.push("u");
    if(item3 == true)
        Ratings.push("e");
    if(item4 == true)
        Ratings.push("m");
    if(item5 == true)
        Ratings.push("h");
    if(item6 == true)
        Types.push("ranges");
    if(item7 == true)
        Types.push("search");
    if(item8 == true)
        Types.push("sortings");
    
    try{
        const data = await axios.post('Poblems/GetProblems/',{
            Status:Solved,
            Ratings:Ratings,
            ProblemTypes:Types,
        })
        setdata(data.data)
        console.log(data)
        setLoading(false)
    }catch(ex){}
   
}

interface Problem {
    tags: string[];
    problemName: string;
    rating: number;
    showtest: string;
    showoutput: string;
    contest_id?: number;  
    timelimit?: number;  
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
                <div className='row-flex-item'>Prepare {'>'} problemset</div>
                <div className='row-flex-item mr bold'>Problemset</div>
            </div>
        </div>
    </div>
    <div className='main-container-problem_set'>
        <div className='problem-set-problems'>
            {data.map(x=>{
                var rate = "Easy";
                if(x.rating >= 1000 && x.rating <= 1199)
                    rate = "Easy";
                else if(x.rating >= 1200 && x.rating <= 1400 )
                    rate = "Medium";
                else
                  rate = "Hard";
                return <div className='problem-card-set'>
                <div className='left-card-side'>
                <span className='problem-title-set'>{x.problemName}</span>
                <span className='lower-item-'>
                    <span className='problem-title-color'>{rate}, {x.tags[0]}, Max Score:15</span>
                </span>
                </div>
                
                <div className='right-section'>
                    <button onClick={()=>{
                        navigate(`/problems/${x.problemName}`)
                    }} className='solve-go-to'>Solve Challenge</button>
                </div>
            </div>
            })}
        </div>
        <div className='sorting-site-section'>
            <div className='filter-group-new'>
                <div className='group-label'>
                    <span className='group-label-text'>Status</span>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem1(x=>!x)
                    }} style={{backgroundColor:(item1 ? "green" : "")}} className='checkbox-item'>
                        {item1 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Solved</div>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem2(x=>!x)
                    }} style={{backgroundColor:(item2 ? "green" : "")}} className='checkbox-item'>
                        {item2 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Unsolved</div>
                </div>
            </div>



            <div className='filter-group-new'>
                <div className='group-label'>
                    <span className='group-label-text'>Rating</span>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem3(x=>!x)
                    }} style={{backgroundColor:(item3 ? "green" : "")}} className='checkbox-item'>
                        {item3 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Easy</div>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem4(x=>!x)
                    }} style={{backgroundColor:(item4 ? "green" : "")}} className='checkbox-item'>
                        {item4 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Medium</div>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem5(x=>!x)
                    }} style={{backgroundColor:(item5 ? "green" : "")}} className='checkbox-item'>
                        {item5 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Hard</div>
                </div>
            </div>

            <div className='filter-group-new'>
                <div className='group-label'>
                    <span className='group-label-text'>Problem Types</span>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem6(x=>!x)
                    }} style={{backgroundColor:(item6 ? "green" : "")}} className='checkbox-item'>
                        {item6 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Ranges</div>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem7(x=>!x)
                    }} style={{backgroundColor:(item7 ? "green" : "")}} className='checkbox-item'>
                        {item7 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Search</div>
                </div>
                <div className='item-sort'>
                    <div onClick={()=>{
                        setitem8(x=>!x)
                    }} style={{backgroundColor:(item8 ? "green" : "")}} className='checkbox-item'>
                        {item8 && <svg width="14px" height="14px" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#e8e8e8"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M5.5 12.5L10.167 17L19.5 8" stroke="#ffffff" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>}
                    </div>
                    <div className='label-checkbox'>Sortings</div>
                </div>
            </div>

     
        </div>
    </div>
    </div>
</div>




}


export  default New_problem_set