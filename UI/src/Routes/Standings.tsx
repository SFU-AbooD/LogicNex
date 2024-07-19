import React from "react";
import '../Static/css/Standings.css'
import routing , {Link} from '../routes';
const Standings_group = ()=>{
    return (
        <div className="content">
            <table className="table_standing">
                <thead>
                    <tr>
                        <th>Rank</th>
                        <th>TEAM</th>
                        <th>SCORE</th>
                        <th><span className="problem_tag">A</span></th>
                        <th><span className="problem_tag">A</span></th>
                        
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>1</td>
                        <td>Univercity of applied scinace private  </td>
                        <td><div className="score" ><h4>9</h4> <h4>500</h4></div></td>
                        <td><div className="submission_block"><h4>85</h4> <h4>2 tries</h4></div></td>
                        <td><div className="submission_block"><h4>85</h4> <h4>2 tries</h4></div></td>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>University of applied scinace private  </td>
                        <td><div className="score" ><h4>9</h4> <h4>500</h4></div></td>
                        <td><div className="submission_block"><h4>85</h4> <h4>2 tries</h4></div></td>
                        <td><div className="submission_block"><h4>85</h4> <h4>2 tries</h4></div></td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

export default Standings_group