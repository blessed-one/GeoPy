import "./HomePage.css"
import WellsTable from "../../../components/WellsTable.jsx";
import WellsImportForm from "../../../components/WellsImportForm.jsx";
import WellsExportButton from "../../../components/WellsExportButton.jsx";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";

export default function HomePage() {

    return (
        <>
            <h1>Скважины</h1>
            <Banner>
                <WellsTable/>
                <WellsImportForm/>
                <WellsExportButton/>
            </Banner>

        </>
    )
}