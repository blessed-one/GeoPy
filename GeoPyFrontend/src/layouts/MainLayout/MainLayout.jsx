import ICONS from '../../assets/icons';
import { Icon } from '../../components/UI/Icon/Icon';
import './MainLayout.css';
import { Link, Outlet } from "react-router-dom";
import authStore from "../../store/store.js";
import {observer} from "mobx-react-lite";

const MainLayout = observer(() => {
    const handleLogout = () => {
        authStore.logout();
    };

    return (
        <>
            <div className="page-container">
                <header className='page-header'>
                    <Link to="/" style={{textDecoration: 'none'}}>
                        <div className="page-logo">
                            <Icon path={ICONS.wells} width={'35'}/>
                        </div>
                    </Link>
                    <div className="page-options">
                        {   authStore.isAuthenticated ? (
                            <>
                                <Link to="#" onClick={handleLogout} style={{textDecoration: 'none'}}>
                                    <Icon path={ICONS.sign_out}/>
                                </Link>
                            </> ) : (
                            <Link to="/signin" style={{textDecoration: 'none'}}>
                                <Icon path={ICONS.sign_in}/>
                            </Link>
                        ) }
                    </div>
                </header>
                <div className="page-content">
                    <Outlet/>
                </div>
            </div>
        </>
    )
})

export default MainLayout