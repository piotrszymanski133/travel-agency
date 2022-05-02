import React, { Component } from 'react'
import {createAPIEndpoint, ENDPOINTS} from "../api";

const queryParams = new URLSearchParams(window.location.search);
var when = queryParams.get('when');
var departure = queryParams.get('departure');
var destination = queryParams.get('destination');
var adults = queryParams.get('adults');
var children_under_3 = queryParams.get('children_under_3')
var children_under_10 = queryParams.get('children_under_10')
var children_under_18 = queryParams.get('children_under_18')

const convertDate = (inputFormat) => {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getFullYear()), pad(d.getMonth()+1), d.getDate()].join('-')
}

export class OfferDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            offer: {}
        }
    }

    componentWillMount(){
        if(when !== null) {
            var date = when.split("-");
            var startDate = date[0].replace(/\s/g, "");
            var endDate = date[1].replace(/\s/g, "")
            startDate = convertDate(startDate)
            endDate = convertDate(endDate)
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

        createAPIEndpoint(ENDPOINTS.getTrip + '?' + searchParams).fetch().then((res) => {
            this.setState({ offer: res.data});
        });
    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Szczegóły oferty</h3>
                <div className="row mt-4">
                    <div className="col-sm offerDetails">
                        <img src="https://images.pexels.com/photos/20787/pexels-photo.jpg?auto=compress&cs=tinysrgb&h=350" alt="new"/>

                    </div>

                    <div className="col">
                        <h5 className="row mx-auto center-column">Tu będą jakieś dodatkowe filtry jak zdążymy zrobić</h5>
                        <button className="mt-4 mx-auto row text-center">Filtr</button>
                        <button className="mt-4 mx-auto row center-column">Filtr</button>
                        <button className="mt-4 mx-auto row center-column">Filtr</button>
                        <button className="mt-4 mx-auto row center-column">Rezerwuj</button>
                    </div>
                </div>
                <div className="col">
                    <div>
                        <h5 className="mt-4">{this.state.offer.destinationCountry} - {this.state.offer.destinationCity}</h5>
                        <p> Opis</p>
                    </div>
                </div>
            </div>
            
        )
    }
}