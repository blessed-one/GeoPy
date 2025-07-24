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
                console.error(e);
                if (e.status === 409) {
                    return {error: e.response.data}
                } else {
                    return {error: e.message};
                }
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
                    console.error(e);
                    if (e.status === 401) {
                        return {error: e.response.data}
                    } else {
                        return {error: e.message};
                    }
                }).finally(() => setLoading(false));
    }

    return { loading, register, login };
};

export default useAuth;