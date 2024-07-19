import  React,{useRef, useState} from "react";
import '../Static/css/submission.css'
import Editor from "@monaco-editor/react";
import routing , {Link} from '../routes';
import { useLocation, useNavigate, useOutletContext, useParams } from "react-router-dom";
import  axios from "../middlewares/axiosMiddleware";
import Loader from "../Components/loader";

const NavBar = ()=>{
    const [ans,statting]:any = useOutletContext();
    const navigate = useNavigate()
    const {problemName} = useParams<string>();
    const inputRef = useRef<any>(null)
    var EditorRef = useRef<any>(null)
    var code = "";
    const [fileDetails,setFileDetails] = useState<any| null>(null)
    const [Loading,setLoading] = useState<any| null>(false)
    const Switching = ()=>{
        swap(x=>x == 1 ? 0 : 1)
    }
    const onEditorMount = (ref:any)=>{
        EditorRef.current = ref;
    }
    const file_upload = (event:any)=>{
            setFileDetails(event.target.files[0])
    }
    const submit = async()=>{
            const form = new FormData();
            if(textOrUplaod == 1){
                if(inputRef.current.files[0] !== undefined)
                 form.append("file",inputRef.current.files[0])
                else
                return
            }
            else{
                form.append("code",EditorRef.current.getValue())
            }
            form.append("Problem_ID", `${problemName}`)
            if(statting!= null )
                {
                    form.append("AI","1")
                    form.append("Tag",statting.tag)
                }
            else
             {
             }
            try{
                setLoading(true)
                const data = await axios.post("Poblems/Submission",form,{
                    headers:{
                        'Content-Type': 'multipart/form-data'
                    }
                    
                })
                setLoading(false)
                navigate(`/verdict/${data.data.id}`)
            }catch(Exception){}
        
    }
    const [textOrUplaod,swap] = useState(0) // 1 stands for upload a file first
    if(Loading)
        return <Loader/>
    return <div className="main-second"><div className="container_2">
          <h1>Submit a solution for {problemName}</h1>
          <br></br>
          {textOrUplaod === 1 ? (
          <div className="align">
                <div className="text-body">
                <input 
                accept=".cpp,.py,.java"
                onChange={file_upload} ref={inputRef} className="files-uploading"  type="file" name="solution"/>
                <div className="middle-text-div">
                    {
                    fileDetails === null ? <>Drag and drop files here to upload!</> :
                    <>Currently the file to upload is {fileDetails.name}</>
                    }
                    </div>
                </div>
            </div>): 
            textOrUplaod === 0 && (
                <div className="text-body">
                <Editor 
                onMount={onEditorMount}
                defaultValue={code}
                theme="vs-light" 
                language="cpp"/>
            </div>)
            } 
            {textOrUplaod  === 1 &&  <button onClick={Switching} className="switcher"><h1>Switch to Editor</h1></button>}
            {textOrUplaod  === 0 &&  <button onClick={Switching} className="switcher" ><h1>Switch to Upload</h1> </button>}
        <div className="choose a language">
            <select className="selector">
                <option>C++17</option>
            </select>
        </div>
        <div className="right__"><button className="submit-button-design" onClick={submit}>Submit</button></div>
    </div>
    </div>
}

export default NavBar