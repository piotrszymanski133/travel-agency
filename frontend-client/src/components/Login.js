import React, {useState, useEffect, Component} from "react";
import axios from "axios";

const Login = ()  =>{
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [user, setUser] = useState();

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
        const user = { name: "user1", pass: "pass1" };
        // send the username and password to the server
        /**const response = await axios.post(
            "http://blogservice.herokuapp.com/api/login",
            user
        );**/
            
        // set the state of the user
        setUser(user);
        //setUser(response.data);
        // store the user in localStorage
        localStorage.setItem("user", JSON.stringify(user));
        //localStorage.setItem("user", JSON.stringify(response.data));
        window.location.href = "/";
    };

    // if there's a user show the message below
    if (user) {
        return (
            <div className="mt-5">
                <h2 className="alreadyLogged">Jesteś już zalogowany jako {user.name}</h2>
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