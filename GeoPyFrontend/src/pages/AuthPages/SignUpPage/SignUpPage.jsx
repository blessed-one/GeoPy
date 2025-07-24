import {Input} from "../../../components/UI/Input/Input.jsx";
import {Button} from "../../../components/UI/Button/Button.jsx";
import "./SignUpPage.css";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";
import useAuth from "../../../hooks/useAuth.js";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import COLORS from "../../../assets/colors.jsx";

const SignUpPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [validationError, setError] = useState('');
    const { loading, register } = useAuth();
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        if (!email || !password || !confirmPassword) {
            setError('Please fill in all fields');
            return;
        } else if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        } else { setError('') }

        const formData = new FormData();
        formData.append('email', email);
        formData.append('password', password)

        const response = await register(formData);
        if (response.error) {
            setError(response.error);
        } else {
            navigate("/")
        }
    };

    return (
        <>
            <div className={'container-sign-up'}>
                <div className={'title-sign-up'}>
                    <h1>SIGN UP</h1>
                </div>
                <Banner className={'banner-sign-up'}>
                    <form className={'sign-up-form'}>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'name'}>Enter your email</label>
                            <Input
                                className="input-sign-up"
                                id="email"
                                placeholder="email"
                                type="text"
                                onChange={(e) => setEmail(e.target.value)}/>
                        </div>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'password'}>Type your password</label>
                            <Input
                                className="input-sign-up"
                                id="password"
                                placeholder="********"
                                type="password"
                                onChange={(e) => setPassword(e.target.value)}/>
                        </div>
                        <div className={'label-input-sign-up'}>
                            <label htmlFor={'retype-password'}>Retype your password</label>
                            <Input
                                className="input-sign-up"
                                id="retype-password"
                                placeholder="********"
                                type="password"
                                onChange={(e) => setConfirmPassword(e.target.value)}/>
                        </div>
                        <div className={'button-sign-up'}>
                            <Button
                                text={loading ? "Wait..." : "Continue"}
                                onClick={handleRegister}
                                round={"true"}
                                color={"blue"}/>
                        </div>
                        {validationError &&
                            <p className="sign-up-error">{validationError}</p>
                        }
                    </form>
                </Banner>
                <div className={'have-sign-up'}>
                    <p>Already have an account?</p>
                    <p><Link to="/signin" style={{color: COLORS.blue}}>Sign in</Link></p>
                </div>
            </div>
        </>
    )
}

export default SignUpPage