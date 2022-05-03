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

    // logout the user
    const handleLogout = () => {
        setUser({});
        setUsername("");
        setPassword("");
        localStorage.clear();
        window.window.location.href = "/";
    };
    
    // if there's a user show the message below
    if (user) {
        return (
            <div className="logout">
                <div className="p-5 mb-4 align-items-center">
                    {user.name} is loggged in <br/>
                    <button className="button" onClick={handleLogout}>logout</button>
                </div>
            </div>
        );
    }
};

export default Login;