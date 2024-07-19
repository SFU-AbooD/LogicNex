import { Link } from "react-router-dom";
import "../Static/css/profile.css";
const Profile = () => {
    const fetch_submission = ()=>{
        
    }
  return (
    <div className="main">
        <div className="profile-center">
            <div className="w-90 table">
        <tr>
          <th>#ID</th>
          <th style={{ width: "60%" }}>Problem</th>
          <th>Verdict</th>
        </tr>
        <Link className="cell" to={`/problems/${1}`}>
          <td>3310</td>
          <td>
            <div className="problem_content">
              All capital
              <br />
              <div className="tags">dp</div>
            </div>
          </td>
          <td>{1}</td>
        </Link>
      </div>
        </div>
    </div>
  );
};

export default Profile;
