import {createBrowserRouter} from "react-router-dom";

import MainLayout from "../layouts/MainLayout/MainLayout.jsx";
import SignInPage from "../pages/AuthPages/SignInPage/SignInPage.jsx";
import SignUpPage from "../pages/AuthPages/SignUpPage/SignUpPage.jsx";
import HomePage from "../pages/MainPages/HomePage/HomePage.jsx";
import ProtectedRoute from "./ProtectedRoute.jsx";

const routes = [
    {
        path: '/',
        element: <MainLayout />,
        children: [
            {
                path:'',
                element: <ProtectedRoute element={<HomePage />} />
            },
            {
                path:'/home',
                element: <ProtectedRoute element={<HomePage />} />
            },
            {
                path: '/signin',
                element: <SignInPage />
            },
            {
                path: '/signup',
                element: <SignUpPage />
            }
        ]
    },
];

const router = createBrowserRouter(routes);

export default router;
