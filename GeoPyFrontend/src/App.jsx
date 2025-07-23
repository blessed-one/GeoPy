import { useState } from 'react'
import './App.css'
import WellsImportForm from "./components/WellsImportForm.jsx";
import WellsExportButton from "./components/WellsExportButton.jsx";
import WellsTable from "./components/WellsTable.jsx";

function App() {
  const [count, setCount] = useState(0)

  return (
      <>
          <div>
              <h1>Скважины</h1>
              <WellsTable/>
              <WellsImportForm/>
              <WellsExportButton/>
          </div>
      </>
  )
}

export default App
