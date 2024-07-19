import { PayloadAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit"
interface User{
    isAuthenticated:Boolean,
    username:string | null,
    email:string | null
}
const userState:User = {
    isAuthenticated:false,
    username:null,
    email : null
}
type PayloadAuthState = {
    Authenticataion_result:boolean,
    username:null | string,
    email:string |  null
}
export const userSlice = createSlice({
    name:'user',
    initialState:userState,
    reducers:{
        updateUserCredentials:(state:User,action:PayloadAction<PayloadAuthState>)=>{
            state.isAuthenticated = action.payload.Authenticataion_result;
            state.username = action.payload.username
            state.email = action.payload.email
        }
    }
});
export const {updateUserCredentials} = userSlice.actions 
export default userSlice.reducer;