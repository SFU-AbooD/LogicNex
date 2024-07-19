import React, { RefObject, useEffect, useRef, useState } from 'react';
import '../Static/css/problemset.css';
import '../Static/css/App.css';
import NavBar from '../Components/NavBar'
import { Link } from 'react-router-dom';
import axios from '../middlewares/axiosMiddleware';
import Loader from '../Components/loader';
interface back_data {
  _id:any,
  tags:string[],
  problemName:string,
  rating:Number,
  contest_id:any,
  problemStatement:string,
}
const ProblemsetLayout = ()=>{
  var [problemlist,setproblemset] = useState<back_data[]>([]);
  const reset = ()=>{
    min_ref.current.value = undefined
    max_ref.current.value = undefined
    setRatingChoice(null)
    setTags([])
    setmax_refv(null)
    setmax_refv(null)
  }
  const fetch_data = async ()=>{
    setLoading(true)
    const body:any = {}
    if(Tags.length > 0){
      body["Tags"] = Tags
    }
    if(RatingChoice !== null){
      body["RatingChoice"] = RatingChoice
    }
    if(min_ref.current.value){
      body["minrange"] = min_ref.current.value
      setmin_refv( min_ref.current.value)
    }
    else{
      setmin_refv(null)
    }
    if(max_ref.current.value){
      body["maxrange"] = max_ref.current.value
      setmax_refv( max_ref.current.value)
    }
    else{
      setmax_refv(null)
    }
      try{
        const data = await axios.post(`/Poblems`,body)
        setproblemset(x=>[...data.data])
      }catch(ex){ 
      }
      setLoading(false)
  }
  const init_data = async ()=>{
    try{
      const data = await axios.get("/Poblems")
      setproblemset(x=>[...data.data])
      setLoading(false)
    }catch(ex){}
  }
  useEffect(()=>{
    init_data()
  },[]) // on mount only
  const min_ref = useRef<any>(null)
  const max_ref = useRef<any>(null)
  const [max_refv,setmax_refv] = useState<Number|any>(null);
  const [min_refv,setmin_refv] = useState<Number|any>(null);
  const [Loading,setLoading] = useState<boolean>(true);
  const [Dropdown,setDropDown] = useState<boolean>(false);
  const [RatingChoice,setRatingChoice] = useState<Number | null>(null);
  const [Tags,setTags] = useState<string[]>([])
  const OpenDropDown = ()=>{
      setDropDown(x=>!x)
  }
  const drop_item_tag = (tag:string)=>{
    setTags(Tags.filter((x)=>x != tag))
  }
  const tags_callback = (tag:string) =>{
      setTags([...Tags,tag])
      setDropDown(false)
  }
  if(Loading){
    return <Loader/>
  }
  return (
    <div className='App'>
      <div className='container_set'>
        <div className='table'>
          <tr>
            <th>#ID</th>
            <th style={{width:"60%"}}>Problem</th>
            <th>Rating</th>
          </tr>
          {problemlist.map((item)=>{
          return <Link className='cell' to={`/problems/${item.problemName}`}>
                  <td className='bold'>{item._id.timestamp}</td>
                <td>
                  <div className='problem_content'>
                {item.problemName}
                        <br/>
                        <div className='tags'>{item.tags.map((x)=>x)}</div>
                        </div>
                      </td>
                      <td><div className='bold'>{item.rating.toString()}</div></td>
                    </Link>
          })}
        </div>
        <div className='problem-set-sidebar'>
          <div className='problem-set-sidebar-center'>
            <div style={{marginBottom:"10px"}}>Filters</div> 
            <div className='Filter-group'>
              Rating
              <div className='w-100 Filter-group-items-selection-center'>
                {
                RatingChoice == 1 ? <div  className='Filter-group-items-selection pointer bb' >High to Low</div > :
                <div onClick={()=>setRatingChoice(1)} className='Filter-group-items-selection pointer' >High to Low</div > 
                }
                {
                RatingChoice == 0 ? <div  className='Filter-group-items-selection pointer bb' >Low to high</div > :
                <div onClick={()=>setRatingChoice(0)} className='Filter-group-items-selection pointer' >Low to high</div > 
                }
              </div>
            </div>
            <div className='Filter-group'>
              Solved by
              <div className='w-100 Filter-group-items-selection-center'>
              {
                RatingChoice == 2 ? <div  className='Filter-group-items-selection pointer bb' >High to Low</div > :
                <div onClick={()=>setRatingChoice(2)} className='Filter-group-items-selection pointer' >High to Low</div > 
                }
                {
                RatingChoice == 3 ? <div  className='Filter-group-items-selection pointer bb' >Low to high</div > :
                <div onClick={()=>setRatingChoice(3)} className='Filter-group-items-selection pointer' >Low to high</div > 
                }
              </div>
            </div>
            <div className='Filter-group'>
              Tags
              <div className='w-100 Filter-group-items-selection-center'>
                <div className='Filter-group-items-selection flex-customize' >
                  <div className='left-part-tags'>
                    {Tags.length == 0 ? 
                    <>Select Tags</> : 
                    <>
                    {Tags.map(x=>{
                      return <div className='added_tag'>
                    {x}
                    <svg className='svg_hover' onClick={()=>drop_item_tag(x)} fill="#ff0000" width="12px" height="12px" viewBox="-3.5 0 19 19" xmlns="http://www.w3.org/2000/svg"  stroke="#ff0000"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"><path d="M11.383 13.644A1.03 1.03 0 0 1 9.928 15.1L6 11.172 2.072 15.1a1.03 1.03 0 1 1-1.455-1.456l3.928-3.928L.617 5.79a1.03 1.03 0 1 1 1.455-1.456L6 8.261l3.928-3.928a1.03 1.03 0 0 1 1.455 1.456L7.455 9.716z"></path></g></svg>
                       </div>
                    })}
                    </>
                    }
                  </div>
                  <div className='Filter-group-items-selection-rightmost pointer'>
                  <svg onClick={OpenDropDown} width="20px" height="20px" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="#000000" transform="matrix(1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <path fill="none" d="M0 0h24v24H0z"></path> <path d="M12 15l-4.243-4.243 1.415-1.414L12 12.172l2.828-2.829 1.415 1.414z"></path> </g> </g></svg>
                  </div>
                </div >
                <div className='somehiddenthing'>
                <div className={Dropdown == false ? 'Filter-group-items' : 'block'}>
                    {Tags.length == 6 ? <div className='no-options'>No Options</div> : <>
                    {Tags.some(x => x === "Dp") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Dp")}}>Dp</h5>}
                  {Tags.some(x => x === "Greedy") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Greedy")}}>Greedy</h5>}
                  {Tags.some(x => x === "Implementaion") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Implementaion")}}>Implementaion</h5>}
                  {Tags.some(x => x === "Searching") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Searching")}}>Searching</h5>}
                  {Tags.some(x => x === "Trees") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Trees")}}>Trees</h5>}
                {Tags.some(x => x === "Math") === false 
                  &&  <h5 className='drop-item' onClick={()=>{tags_callback("Math")}}>Math</h5>}
                    </>}
                  </div>
                </div>
              </div>
            </div>
            <div className='Filter-group'>
              Rate Range
              <div className='w-100  Filter-group-items-selection-center'>
                <div className='pd Filter-group-items-selection' >
                  <div className='div-rate'>
                    <div className='min-max-by'>MinBy</div>
                    <input defaultValue={min_refv} ref={min_ref} min="1000" max="2000" className='input_rem' type='number'/>
                  </div>
                </div >
                <div className='pd Filter-group-items-selection' >
                  <div className='div-rate'>
                    <div className='min-max-by'>MaxBy</div>
                    <input defaultValue={max_refv} ref={max_ref} min="1000" max="2000" className='input_rem' type='number'/>
                  </div>
                </div >
              </div>
              </div>
              <div className='actions-bottom'>
                <button className='btn-Size filter' onClick={fetch_data}>Filter</button>
                <button className='btn-Size reset' onClick={reset}>reset</button>
              </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProblemsetLayout;
