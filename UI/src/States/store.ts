import { configureStore } from '@reduxjs/toolkit';
import userReducer from './userslice';
import {updateUserCredentials} from './userslice'; 
const store = configureStore({
    reducer:{
        user:userReducer
    }
})
export type RootState = ReturnType<typeof store.getState>
export const AppDispatch = typeof store.dispatch
export default store