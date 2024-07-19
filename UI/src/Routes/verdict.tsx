import  React,{useEffect, useRef, useState} from "react";
import '../Static/css/verdict.css'
import { useParams } from "react-router-dom";
import * as signalR from '@microsoft/signalr';
import  axios from "../middlewares/axiosMiddleware";
import Loader from "../Components/loader";
interface response {
    submissionID:string,
    verdict:string,
    Current_Case:number,
    time_usage:number,
    finish:boolean,
    ai_response?:string
    memory_usage:number,
    last_running_case:number
}
interface response_query {
    language:string,
    problemname:string,
    submissionDate:string,
    testCount:number,
    time:number,
    ai_response?:string,
    _id:string,
    verdict:string,
    last_running_case:number
}
const Verdict = ()=>{
    const fetch_infomation = async()=>{
        try{
            const data = await axios.get(`Poblems/problemData?submission_id=${VerdictID}`)
            const ans:response_query = data.data;
            if(!ans.verdict.includes("Running") && !ans.verdict.includes("queue")){
                setFinished(true)
                const temp:response = {
                    submissionID:ans._id,
                    verdict:ans.verdict,
                    Current_Case:ans.last_running_case,
                    time_usage:ans.time,
                    finish:true,
                    memory_usage:0,
                    last_running_case:ans.last_running_case
                }
                handle_res(temp)
            }
            else{
                const temp:response = {
                    submissionID:ans._id,
                    verdict:ans.verdict,
                    Current_Case:ans.last_running_case,
                    time_usage:ans.time,
                    finish:false,
                    memory_usage:0,
                    last_running_case:ans.last_running_case
                }
                handle_res(temp)
            }
            setTableData(data.data)
            const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:8080/Submission",{
                skipNegotiation:true,
                transport:signalR.HttpTransportType.WebSockets
            }).build()
            connection.start().then(()=>{
                console.log("correct")
                connection.invoke("AddGroup",VerdictID)
                connection.on("UpdateSubmission",(body:response)=>{
                    console.log(body)
                    if(body.ai_response != null && body.ai_response.length > 0){
                        setreply(body.ai_response)
                    }
                    if(body.finish){
                        setFinished(true)
                    }
                    if(responsingUpdate?.Current_Case == undefined ||body.Current_Case > responsingUpdate?.Current_Case!)
                     {
                            handle_res(body)
                     }
                })
                setconnection(connection)
            }).catch(err=>{})
        }catch(ex){}
        setLoading(false)
    }
    const [Loading,setLoading] = useState(true)
    const [Finished,setFinished] = useState(false)
    const [responsingUpdate,handle_res] = useState<response>()
    const [TableData,setTableData] = useState<response_query>();
    const {VerdictID} = useParams();
    const [Aireply,setreply] = useState<string | null>(null);
    useEffect(()=>{
        fetch_infomation();
    },[])
    const [connection,setconnection] = useState<any>(null);
    const {problemName} = useParams()
    var flag:Boolean =false;
    if(Loading){
        return <Loader/>
    }
    else{
    let color = "gray";
    if(responsingUpdate?.verdict === "Accepted")
        color = "green"
    else if(responsingUpdate?.verdict != "in-queue"){
        color = "red"
    }
    return <div className="main">
            <div className="main-verdict-verdict">
                <div className="Submission-verdict-verdict">
                    <h1>Submission</h1>
                    <table className="verdict-table-verdict">
                        <thead>
                        <tr className="table-row">
                                <th className="bold cell-verdict" rowSpan={2}>ID</th>
                                <th className="bold cell-verdict">DATE</th>
                                <th className="bold cell-verdict">PROBLEM</th>
                                <th className="bold cell-verdict">STATUS</th>
                                <th className="bold cell-verdict">Time</th>
                                <th className="bold cell-verdict">Language</th>
                            </tr>
                            <tr className="table-row-verdict">
                                <th colSpan={5} className="bold cell-verdict">TEST CASES</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td rowSpan={2} className="bold cell-verdict">{TableData?._id.toString()}</td>
                                <td className="bold cell-verdict">{TableData?.submissionDate}</td>
                                <td className="bold cell-verdict">{TableData?.problemname}</td>
                                {
                                    <td className={`${color} bold cell-verdict`}>{responsingUpdate?.verdict}</td>
                                }
                                <td className="bold cell-verdict">{Finished && <>{responsingUpdate?.time_usage}ms</> }</td>
                                <td className="bold cell-verdict cc">{TableData?.language}</td>
                            </tr>
                            <tr>
                               <td className="break-newline" colSpan={6}>
                                <div className="test_cases-verdict">
                                    {[...Array(TableData?.testCount)].map((_,x)=>{
                                        const v = x + 1
                                        if(v! <= responsingUpdate?.last_running_case!){
                                            return <span className="test_case-verdict">
                                            <svg fill="#37ff00" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" width="15px" height="15px" viewBox="0 0 335.765 335.765"  stroke="#37ff00" stroke-width="17.124015"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <g> <g> <polygon points="311.757,41.803 107.573,245.96 23.986,162.364 0,186.393 107.573,293.962 335.765,65.795 "></polygon> </g> </g> </g></svg>
                                            </span>
                                        }
                                        else{
                                            if(responsingUpdate?.finish && !flag){
                                                flag = true;
                                               return <span className="test_case-verdict">
                                                <svg fill="#ff0000" width="15px" height="15px" viewBox="0 0 200.00 200.00" data-name="Layer 1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" stroke="#ff0000" stroke-width="6.6"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"><title></title><path d="M114,100l49-49a9.9,9.9,0,0,0-14-14L100,86,51,37A9.9,9.9,0,0,0,37,51l49,49L37,149a9.9,9.9,0,0,0,14,14l49-49,49,49a9.9,9.9,0,0,0,14-14Z"></path></g></svg>
                                                </span>
                                            }
                                            else
                                            {
                                                return <span className="test_case-verdict"/>
                                            }
                                        }
                                    })}
                                </div>
                                </td> 
                            </tr>
                        </tbody>
                    </table>
                </div>
               {(Aireply != null || TableData?.ai_response != null)  && <div className="Ai-response">
                 <h2>Ai FeedBack</h2>
                 <pre className="ft-16">{Aireply != null ? Aireply : TableData?.ai_response}</pre>
                </div>}
            </div>
    </div>
}
}

export default Verdict