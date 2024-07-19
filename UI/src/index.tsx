import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './Routes/App';
import reportWebVitals from './reportWebVitals';
import routers,{RouterProvider} from './routes';
import { Provider } from 'react-redux';
import { AutoUserAck } from './customWrappers/autoUserAck';
import store from './States/store'

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
    <Provider store={store}>
      <AutoUserAck>
       <RouterProvider router={routers}/>
      </AutoUserAck>
    </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
