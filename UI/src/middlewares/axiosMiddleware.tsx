import axios from "axios";

const api = axios.create({
    withCredentials:true,
    baseURL:'https://localhost:7179/api/',
});
api.interceptors.response.use((response)=>response,async (error)=>{
    const request = error.config
    if(error.response.status == 401){
        try{
            const res_ref = await axios.post('/api/refreshToken');
            return axios(request);
        }catch(error){
            // handle redirect to login
        }
    }
    return Promise.reject(error);
})
export default api;