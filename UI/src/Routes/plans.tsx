import { Link, useParams } from "react-router-dom";
import "../Static/css/profile.css";
import axios from "../middlewares/axiosMiddleware";
import { JSXElementConstructor, ReactElement, ReactNode, ReactPortal, useEffect, useState } from "react";
import Loader from "../Components/loader";
const Plans = () => {
    const fetch_submission = async()=>{
        try{
                const data = await axios.get(`Plan/problems?name=${planName}`)
                setData(data.data)
                setLoading(false)
        }catch(ex){}
    }
  const {planName} = useParams();
  const [data,setData] = useState<any[]>()
  const [Loading,setLoading] = useState<boolean>(true)
  useEffect(()=>{
        fetch_submission()
  },[])
  if(Loading)
    return <Loader/>
  return (
    <div className="main">
        <div className="profile-center">
            {data?.map(x=>{
                if(x.length > 0){
                    return <div className="w-90 table">
                         <tr>
          <th>#ID</th>
          <th style={{ width: "60%" }}>Problem</th>
        </tr>
        {console.log(x)}
                {x.map((t: { tags:string[] , problemName: string | number | boolean | ReactElement<any, string | JSXElementConstructor<any>> | Iterable<ReactNode> | ReactPortal | null | undefined; })=>{
                            const Ai = {Ai:true,tag:t.tags[0]} 
                            return <Link state={Ai}  className="cell" to={`/problems/${t.problemName}`}>
                            <td>3310</td>
                            <td>
                              <div className="problem_content">
                                {t.problemName}
                                <br />
                                <div className="tags">{t.tags[0]}</div>
                              </div>
                            </td>
                          </Link>
                })}
               
                    </div>
                }
            })}
        </div>
    </div>
  );
};

export default Plans;
