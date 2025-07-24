import ICONS from "../../../assets/icons.jsx";
import COLORS from '../../../assets/colors.jsx';
import {Button} from "../../../components/UI/Button/Button.jsx";
import {Input} from "../../../components/UI/Input/Input.jsx";
import {Banner} from "../../../components/UI/Banner/Banner.jsx";

import { Link, useNavigate } from 'react-router-dom';
import useAuth from '../../../hooks/useAuth.js';
import { useState } from 'react';
import './SignInPage.css';

const SignInPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [validationError, setError] = useState('');
    const { loading, login } = useAuth();
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        if (!email || !password) {
            setError('Please fill in all fields');
            return;
        } else { setError('') }

        const formData = new FormData();
        formData.append('email', email);
        formData.append('password', password)

        await login(formData).then(async (result) => {
            if (result.error) {
                setError(result.error);
            } else {
                navigate("/")
            }
        });
    };

    return (
        <>
            <div className={'container-sign-in'}>
                <div className={'title-sign-in'}>
                    <h1>SIGN IN</h1>
                </div>
                <Banner>
                    <form className={'sign-in-form'}>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'name'}>Enter your email</label>
                            <Input
                                className="input-sign-in"
                                id="email"
                                placeholder="email"
                                type="text"
                                onChange={(e) => setEmail(e.target.value)}/>
                        </div>
                        <div className={'label-input-sign-in'}>
                            <label htmlFor={'password'}>Enter your password</label>
                            <Input
                                className="input-sign-in"
                                id="password"
                                placeholder="********"
                                type="password"
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>
                        <div className={'button-sign-in'}>
                            <Button
                                text={loading ? "Wait..." : "Sign in"}
                                onClick={handleLogin}
                                round={"true"}
                                color={"blue"}
                                iconPath={ICONS.sign_in}
                            />
                        </div>
                        {validationError &&
                            <p className="sign-up-error">{validationError}</p>
                        }
                    </form>
                </Banner>
                <div className={'dont-sign-in'}>
                    <p>Don’t registered yet?</p>
                    <p><Link to="/signup" style={{color: COLORS.blue}}>Create an account</Link> in a few steps</p>
                </div>
            </div>
        </>
    )
}

export default SignInPage