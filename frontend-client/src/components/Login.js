import React, {useState, useEffect, Component} from "react";
import axios from "axios";

const Login = ()  =>{
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [user, setUser] = useState();

    // User Login info
    const database = [
        {
            username: "user1",
            password: "pass1"
        },
        {
            username: "user2",
            password: "pass2"
        }
    ];

    useEffect(() => {
        const loggedInUser = localStorage.getItem("user");
        if (loggedInUser) {
            const foundUser = JSON.parse(loggedInUser);
            setUser(foundUser);
        }
    }, []);

    // logout the user
    const handleLogout = () => {
        setUser({});
        setUsername("");
        setPassword("");
        localStorage.clear();
    };

    // login the user
    const handleSubmit = async e => {
        e.preventDefault();
        const user = { username, password };
        // send the username and password to the server
        const response = await axios.post(
            "http://blogservice.herokuapp.com/api/login",
            user
        );
        // set the state of the user
        setUser(response.data);
        // store the user in localStorage
        localStorage.setItem("user", JSON.stringify(response.data));
    };

    // if there's a user show the message below
    if (user) {
        return (
            <div>
                {user.name} is loggged in
                <button onClick={handleLogout}>logout</button>
            </div>
        );
    }

    // if there's no user, show the login form
    return (
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Username: </label>
                <input
                    type="text"
                    value={username}
                    placeholder="enter a username"
                    onChange={({ target }) => setUsername(target.value)}
                />
                <div>
                    <label htmlFor="password">password: </label>
                    <input
                        type="password"
                        value={password}
                        placeholder="enter a password"
                        onChange={({ target }) => setPassword(target.value)}
                    />
                </div>
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Login;