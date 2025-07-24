import {observable, action, makeObservable, runInAction} from 'mobx';
import {toast} from "react-toastify";
import {axiosToBackend} from "../hooks/useAxios.js";

class AuthStore {
    isAuthenticated = !!localStorage.getItem('token');

    constructor() {
        makeObservable(this, {
            isAuthenticated: observable,
            login: action.bound,
            logout: action.bound
        });
    }
    login = async (token) => {
        toast.info("You have successfully logged in", {toastId: "LogInInfo"})
        localStorage.setItem('token', token);
        await axiosToBackend.get(`/wells`).then( () => runInAction(() => {
            this.isAuthenticated = true;
        })).catch((e) => {
            if (e.status === 401) {
                toast.error("Error: Unauthorized")
            } else {
                console.log(e)
            }
        });
    }
    logout = () => {
        toast.info("You have successfully logged out", {toastId: "LogOutInfo"})
        localStorage.removeItem('token');
        runInAction(() =>  {
            this.isAuthenticated = false });
        location.reload();
    }

}

const authStore = new AuthStore();

export default authStore;