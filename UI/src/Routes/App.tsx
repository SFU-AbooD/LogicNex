import React from 'react';
import '../Static/css/App.css';
import NavBar from '../Components/NavBar'
import { useNavigation ,useFetcher, useNavigate} from 'react-router-dom';
import ProblemsetLayout from './Problemset';
function App() {
  const navigate = useNavigate()
  return (
    <div className="App">
      <div className='body-main'>
      <video className='video-fitting' autoPlay muted>
        <source src='https://cdn-cf-east.streamable.com/video/mp4/t8x3af.mp4?Expires=1717705076096&Key-Pair-Id=APKAIEYUVEN4EVB2OKEQ&Signature=QWwkhVNpn-Dp14wJxwiDbICmUmW7dzJ3ntJrbQxXTjr5TPC2Ju1WFidmZTQNu5CroxB5FuL5lMOGmJKeitLwdzlyxUybj24pwthVf3ogURfFx8001RCIDU8H3c6h8zDM3XN-DboWjEQosbkgHKs3xVJ382z8w5tT0yGkyfaF5Rkd0X4~JOOHTI5c1ZGO1f3WtoFFdhGMrqU6lay0aqDPqiaW0S3AQwaxFsSH6jq8iOPI007qLRkoej5J3narq1g8HmvQ8U~fDnaUemNBgJu5CLtc6NC~6aamifOz5a2~34GV0NKgaucgxN3tHl1Uf~-3lvCxIR1ECCiSevZ2DuVXSA__'></source>
      </video>
      <div className='bodyUpper'>
        <div>LogicNex</div>
       <div>
       <button className='Button-format'>Start Now</button>
       </div>
      </div>
      </div>
</div>
  );
}

export default App;
