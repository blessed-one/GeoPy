import { useState } from "react";
import { axiosToBackend } from "./useAxios.js";
import authStore from "../store/store.js";

const useAuth = () => {
    const [loading, setLoading] = useState(false);

    async function register(userData) {
        setLoading(true);
        return await axiosToBackend.post(`/auth/register`, userData)
            .then((response) => {
                if (response.status === 201) {
                    return {error: null };
                } else {
                    return {error: response.data.error };
                }}).catch((e) => {
                return errorProcessing(e)
                }).finally(() => setLoading(false));
    }

    async function login(userData) {
        setLoading(true);
        return await axiosToBackend.post(`/auth/login`, userData)
            .then(async (response) => {
                if (response.status === 200) {
                    await authStore.login(response.data);
                    return {error: null };
                }}).catch((e) => {
                    return errorProcessing(e)
                }).finally(() => setLoading(false));
    }

    function errorProcessing(error) {
        switch (error.status) {
            case 401:
            case 409:
                return {error: error.response.data}
            case 400:
                return {error: `${error.response.data.title} ${error.response.data.errors?.Email ?? ''} ${error.response.data.errors?.Password ?? ''}`}
            default:
                return {error: error.message}
        }
    }

    return { loading, register, login };
};

export default useAuth;