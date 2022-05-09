import React, { Component } from 'react'
import {createAPIEndpoint, ENDPOINTS} from "../api";

export class Destinations extends Component {
    constructor(props) {
        super(props)

        this.state = {
            destinations: []
        }
    }

    componentDidMount(){
        createAPIEndpoint(ENDPOINTS.getDestinations).fetch().then((res) => {
            this.setState({ destinations: res.data});
        });
    }

    handleClick = (data) => {
        const searchParams = new URLSearchParams();
        searchParams.append("when", "06/01/2022 - 06/08/2022");
        searchParams.append("departure", "");
        searchParams.append("destination", data);
        searchParams.append("adults", '1');
        searchParams.append("childrenUnder3", '0');
        searchParams.append("childrenUnder10", '0');
        searchParams.append("childrenUnder18", '0');
        window.location.href = "/offer?" + searchParams;
    }

    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Nasze kierunki</h3>
                <ul className="list-group">
                    {
                        this.state.destinations.map(
                            destination =>
                                destination !== "dowolnie"  && <li key={destination} onClick={this.handleClick.bind(this, destination)}  value={destination} className="border list-group-item mt-3 offer h5">
                                    {destination}
                                    <button className="check_offer">Wyszukaj oferty</button>
                                </li>
                        )
                    }
                </ul>
            </div>
        )
    }
}