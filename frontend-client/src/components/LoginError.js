import React, {useState, useEffect, Component} from "react";
import axios from "axios";

const Logout = ()  =>{
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
    const handleRetry = () => {
        window.window.location.href = "/login";
    };

    // if there's a user show the message below
    if (!user) {
        return (
            <div className="logout text-center">
                <h3>Nie udało się poprawnie zalogować. Spróbuj ponownie</h3>
                <button className="button mt-4" onClick={handleRetry}>Spróbuj ponownie</button>
            </div>
        );
    }
};

export default Logout;