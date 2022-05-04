import React, {useState, useEffect, Component} from "react";
import axios from "axios";
import {BASE_URL, createAPIEndpoint, ENDPOINTS} from "../api";

const Login = ()  =>{
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [user, setUser] = useState();
    const [loginResponse, setLoginResponse] = useState();

    useEffect(() => {
        const loggedInUser = localStorage.getItem("user");
        if (loggedInUser) {
            const foundUser = JSON.parse(loggedInUser);
            setUser(foundUser);
        }
    }, []);
    
    // login the user
    const handleSubmit = async e => {
        e.preventDefault();
        const user = { username: username, password: password };

        fetch(BASE_URL + ENDPOINTS.login, {
            method: 'POST',
            body: JSON.stringify(user),
            headers: {
                'Content-Type': 'application/json',
            }
        }).then(response => {
            console.log(response.status)
            console.log("Data was sent")
            setLoginResponse(response.status)
            if(response.status === 200){
                setUser(user);
                localStorage.setItem("user", JSON.stringify(user));
                window.setTimeout(function() {
                    window.location.href = '/';
                }, 1000);
            }
            else {
                window.setTimeout(function() {
                    window.location.href = '/loginError';
                }, 1000);
            }
        })
    };

    // if there's a user show the message below
    if (user) {
        return (
            <div className="mt-5">
                <h2 className="alreadyLogged">Zalogowano jako {user.username}</h2>
            </div>
        );
    }
    
    // if there's no user, show the login form
    return (
        <div className="loginForm">
            <h3 className="text-center mb-5">Zaloguj się</h3>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Nazwa użytkownika: </label>
                <input
                    type="text"
                    value={username}
                    onChange={({ target }) => setUsername(target.value)}
                />
                <div>
                    <label htmlFor="password">Hasło: </label>
                    <input
                        type="password"
                        value={password}
                        onChange={({ target }) => setPassword(target.value)}
                    />
                </div>
                <button className="button mt-3" type="submit">Zaloguj</button>
            </form>
        </div>
    );
};

export default Login;