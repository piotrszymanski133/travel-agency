import React, { Component } from 'react'
import {createAPIEndpoint, ENDPOINTS} from "../api";

export class Destinations extends Component {
    constructor(props) {
        super(props)

        this.state = {
            destinations: { 'trips': []
            }
        }
        
    }

    componentDidMount(){
        createAPIEndpoint(ENDPOINTS.trip).fetch().then((res) => {
            this.setState({ destinations: res.data});
        });
    }

    handleClick = (data) => {
        const searchParams = new URLSearchParams();
        searchParams.append("when", "05/20/2022 - 05/27/2022");
        searchParams.append("departure", "");
        searchParams.append("destination", "");
        searchParams.append("adults", '1');
        searchParams.append("children_under_3", '0');
        searchParams.append("children_under_10", '0');
        searchParams.append("children_under_18", '0');
        window.location.href = "/offer?" + searchParams;
    }

    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Nasze kierunki</h3>
                <ul className="list-group">
                    {
                        this.state.destinations.trips.map(
                            offer =>
                                <li onClick={this.handleClick.bind(this, offer.hotel.destinationCountry)} key={offer.hotel.id} value={offer.hotel.destinationCountry} className="border list-group-item mt-5 offer">
                                    { offer.hotel.destinationCountry}
                                    <button className="check_offer">Wyszukaj ofert</button>
                                </li>
                        )
                    }

                </ul>
            </div>
        )
    }
}