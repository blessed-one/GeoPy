import { Navigate } from 'react-router-dom';
import authStore from "../store/store.js";
import {toast} from "react-toastify";
import {useEffect, useState} from "react"; // Импортируйте ваш хранилище аутентификации

const ProtectedRoute = ({ element }) => {
    const [shouldRedirect, setShouldRedirect] = useState(false);

    useEffect(() => {
        if (!authStore.isAuthenticated) {
            toast.info("Please log in first", { toastId: "LogInRequired" });
            setShouldRedirect(true);
        }
    }, []);

    if (shouldRedirect) {
        return <Navigate to="/signin" />;
    }

    return authStore.isAuthenticated ? element : null;
};

export default ProtectedRoute;