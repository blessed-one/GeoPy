import "./HomePage.css"
import WellsTable from "../../../components/WellsTable.jsx";
import WellsImportForm from "../../../components/WellsImportForm.jsx";
import WellsExportButton from "../../../components/WellsExportButton.jsx";

export default function HomePage() {

    return (
        <>
            <h1>Скважины!</h1>
            <WellsTable/>
            <WellsImportForm/>
            <WellsExportButton/>
        </>
    )
}