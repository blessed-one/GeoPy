import axios from 'axios';
import configData from "./../../config.json";

export const axiosToBackend = axios.create({
    baseURL: `${configData.BASE_URL}`,
    withCredentials: true,
    timeout: 30000
});

axiosToBackend.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`;
    return config;
})