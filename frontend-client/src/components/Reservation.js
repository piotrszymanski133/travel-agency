import React, {Component} from "react";
import {Nav, Navbar} from "react-bootstrap";
import {ReactComponent as Logo} from "../icons/logo.svg";
import {NavLink} from "react-router-dom";


export class Reservation extends Component{

    constructor(props) {
        super(props)
    }

    componentWillMount() {
        const key = Object.keys(localStorage)
        const user = localStorage.getItem(key)
        const isLogged = false
        if(user === null){
            this.isLogged = false
        }
        else{
            this.isLogged = true
        }
    }
    
    render(){
        // if there's a user show the message below
        if (!this.isLogged) {
            return (
                <div className="mt-5">
                    <h2 className="alreadyLogged">Zaloguj się, aby zarezerować ofertę!</h2>
                </div>
            );
        }
        return(
            <div className="mt-5">
                <h2 className="alreadyLogged">Jesteś już zalogowany, zatem dokonaj rezerwacji poniżej</h2>
            </div>
        )
    }
}