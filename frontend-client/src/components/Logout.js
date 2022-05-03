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
            <div className="logout text-center">
                <h3>Zalogowano jako {user.name}</h3>
                <button className="button mt-4" onClick={handleLogout}>Wyloguj</button>
            </div>
        );
    }
};

export default Login;