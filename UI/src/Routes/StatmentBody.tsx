import React from "react";
import '../Static/css/body.css'
import '../Static/css/problemset.css';
import routing , {Link} from '../routes';
import { useOutletContext } from "react-router-dom";
interface back_data {
  _id:any,
  tags:string[],
  problemName:string,
  rating:Number,
  contest_id:any,
  problemStatement:string,
  timelimit:Number
}
const NavBar = ()=>{
  const [data]:any = useOutletContext();
    return <div className="main">
            <div className="Title">
                <div className="headers">
                    <h1>{data.problemName}</h1>
                    <h3>Time limit : {data.timelimit} second</h3>
                    <h3>Memory limit: 256 mb</h3>
                    <h3>Standard Input: stdin</h3>
                    <h3>Standard Output: stdout</h3>
                </div>
            </div>
        <div className="content">
            <div className="statment">
              <div className="item-50">
              {data.problemStatement}
              </div>
            </div>
        </div>
    <div className="middle-sample">
    <div className="sample">
         Sample 1
        <div className="example">
          <div className="bordering">Input</div>
          <div className="output">Output</div>
          <div className="divider"></div>
          <div className="input">
            <pre>{data.showtest}</pre>
            </div>
          <div className="output_left">
          <pre>{data.showoutput}</pre>
            </div>
        </div>
    </div>
    </div>
    </div>
}

export default NavBar