import React, {Component} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../api";

export class UserTrips extends Component{

    constructor(props) {
        super(props)
        this.state = {
            offers: {'userTrips': []}
        }
    }

    componentDidMount() {
        const key = Object.keys(localStorage)
        const user = localStorage.getItem(key)
        const isLogged = false
        var parsedUser = "";
        if(user === null){
            this.isLogged = false
        }
        else{
            this.isLogged = true
            parsedUser = JSON.parse(user);
            this.setState({username: parsedUser.username})
        }
        const searchParams = new URLSearchParams();
        searchParams.append("Username", parsedUser.username);
        createAPIEndpoint(ENDPOINTS.getUserTrips + '?' + searchParams).fetch().then((res) => {
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
                <table className = "table table-striped table-bordered mt-4">
                    <thead>
                    <tr>
                        <th> ID rezerwacji </th>
                        <th> Data początkowa </th>
                        <th> Data końcowa </th>
                        <th> Liczba osób </th>
                        <th> Kraj </th>
                        <th> Miasto </th>
                        <th> Hotel </th>
                        <th> Posiłki </th>
                        <th> Typ pokoju </th>
                        <th> Typ transportu </th>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        !this.state.offers.userTrips.isEmpty && this.state.offers.userTrips.map(
                            offer =>
                                <tr key={offer.id}>
                                    <td> {offer.id} </td>
                                    <td> {new Date (offer.startDate).toLocaleDateString()} </td>
                                    <td> {new Date (offer.endDate).toLocaleDateString()} </td>
                                    <td> {offer.persons} </td>
                                    <td> {offer.country} </td>
                                    <td> {offer.city} </td>
                                    <td> {offer.hotelName} </td>
                                    <td> {offer.foodType} </td>
                                    <td> {offer.hotelRoomName} </td>
                                    <td> {offer.transportTypeName} </td>
                                </tr>
                        )
                    }
                    </tbody>
                </table>
            </div>
        )
    }
}