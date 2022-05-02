import React, { Component } from 'react'
import { createAPIEndpoint, ENDPOINTS, BASE_URL } from '../api'
import {OfferSearchForm} from "./OfferSearchForm";

const queryParams = new URLSearchParams(window.location.search);
var when = queryParams.get('when');
var departure = queryParams.get('departure');
var destination = queryParams.get('destination');
var adults = queryParams.get('adults');
var children_under_3 = queryParams.get('children_under_3')
var children_under_10 = queryParams.get('children_under_10')
var children_under_18 = queryParams.get('children_under_18')

export class Offer extends Component {
    constructor(props) {
        super(props)

        this.state = {
            offers: { 'trips': []
            } 
        }
    }

    convertDate(inputFormat) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        var d = new Date(inputFormat)
        return [pad(d.getFullYear()), pad(d.getMonth()+1), d.getDate()].join('-')
    }

    componentDidMount(){
        if(when !== null) {
            var date = when.split("-");
            var startDate = date[0].replace(/\s/g, "");
            var endDate = date[1].replace(/\s/g, "")
            startDate = this.convertDate(startDate)
            endDate = this.convertDate(endDate)
        }
        const searchParams = new URLSearchParams();
        if(departure === ""){
            departure = "any"
        }
        if(destination === ""){
            destination = "any"
        }
        searchParams.append("startDate", startDate);
        searchParams.append("endDate", endDate);
        searchParams.append("departure", departure);
        searchParams.append("destination", destination);
        searchParams.append("adults", adults);
        searchParams.append("children_under_3", children_under_3);
        searchParams.append("children_under_10", children_under_10);
        searchParams.append("children_under_18", children_under_18);
        
        createAPIEndpoint(ENDPOINTS.trip + '?' + searchParams).fetch().then((res) => {
            this.setState({ offers: res.data});
        });
    }

    handleClick = () => {
        const searchParams = new URLSearchParams();
        searchParams.append("when", when);
        searchParams.append("departure", departure);
        searchParams.append("destination", destination);
        searchParams.append("adults", adults);
        searchParams.append("children_under_3", children_under_3);
        searchParams.append("children_under_10", children_under_10);
        searchParams.append("children_under_18", children_under_18);
        window.location.href = "/offer_details?" + searchParams;
    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <div className="mt-5">
                    <OfferSearchForm className="row"></OfferSearchForm>
                </div>
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