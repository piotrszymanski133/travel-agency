import React, { Component } from 'react'
import { createAPIEndpoint, ENDPOINTS, BASE_URL } from '../api'

export class Offer extends Component {
    constructor(props) {
        super(props)

        this.state = {
            offers: { 'trips': []
            } 
        }
    }

    componentDidMount(){
        createAPIEndpoint(ENDPOINTS.trip).fetch().then((res) => {
            this.setState({ offers: res.data});
        });
    }

    handleClick = () => {
        window.location.href = "/offer_details";
    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Znalezione oferty</h3>
                    <ul className="list-group">
                        {
                            this.state.offers.trips.map(
                                offer =>
                                    <li key={offer.hotel.id} className="border list-group-item mt-5 offer">
                                        <img src="https://images.pexels.com/photos/20787/pexels-photo.jpg?auto=compress&cs=tinysrgb&h=350" alt="new"/>
                                        <h5> { offer.hotel.name} </h5>
                                        <p> { offer.hotel.destinationCountry} / {offer.hotel.destinationCity} </p>
                                        <p> Średnia z ocen: { offer.hotel.rating}</p>
                                        <p> Liczba gwiazdek: { offer.hotel.stars}</p>
                                        <p className="price_par"> Cena: </p>
                                        <button onClick={this.handleClick} className="check_offer">Sprawdź ofertę</button>
                                    </li>
                            )
                        }

                    </ul>
            </div>
        )
    }
}