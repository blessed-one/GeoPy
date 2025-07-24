import './App.css'
import {RouterProvider} from "react-router-dom";
import router from './router/router';
import {ToastContainer} from "react-toastify";

function App() {
  return (
      <>
          <div>
              <RouterProvider router={router}/>
              <ToastContainer />
          </div>
      </>
  )
}

export default App
