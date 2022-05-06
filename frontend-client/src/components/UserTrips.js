import React, {Component} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../api";

export class UserTrips extends Component{

    constructor(props) {
        super(props)
        this.state = {
            offers:  []
        }
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
            const parsedUser = JSON.parse(user);
            this.setState({username: parsedUser.username})
        }
        createAPIEndpoint(ENDPOINTS.getDestinations).fetch().then((res) => {
            this.setState({ offers: res.data});
        });
    }

    render(){
        // if there's a user show the message below
        if (!this.isLogged) {
            return (
                <div className="mt-5">
                    <h2 className="alreadyLogged">Zaloguj się, aby zobaczyć zakupione oferty.</h2>
                </div>
            );
        }
        return(
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Zakupione oferty dla użytkownika {this.state.username}</h3>
                <ul className="list-group">
                    {
                        this.state.offers.map(
                            offer =>
                                <li value={offer} className="border list-group-item mt-3 offer h5">
                                    { offer}

                                </li>
                        )
                    }

                </ul>
            </div>
        )
    }
}