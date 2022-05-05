import React, {Component, useState} from 'react'
import {BASE_URL, createAPIEndpoint, ENDPOINTS} from "../api";

const queryParams = new URLSearchParams(window.location.search);
var hotelID = queryParams.get('hotelID');
var when = queryParams.get('when');
var departure = queryParams.get('departure');
var destination = queryParams.get('destination');
var adults = queryParams.get('adults');
var children_under_3 = queryParams.get('childrenUnder3')
var children_under_10 = queryParams.get('childrenUnder10')
var children_under_18 = queryParams.get('childrenUnder18')

const convertDate = (inputFormat) => {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getFullYear()), pad(d.getMonth()+1), d.getDate()].join('-')
}


export class OfferDetails extends Component {
    constructor(props) {
        super(props);
        this.state = {
            offer: {'tripOffer': {
                'startDate': "",
                'endDate': "",
                'hotelOffer': {
                    'roomsConfigurations': []
                },
                'transportOffers': []    
                }},
            
            selectedRoomType: "",
            roomPrices: [],
            roomPricesConverted: {},
            roomPricesConvertedWithId: {},
            selectedTransportType: "",
            transportPrices: [],
            transportPricesConverted: {},
            transportCitiesConverted: {},
            transportFromIdConverted: {},
            transportToIdConverted: {}
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
        if(departure === "" || departure === "dowolnie"){
            departure = "any"
        }
        if(destination === "" || destination === "dowolnie"){
            destination = "any"
        }
        searchParams.append("hotelID", hotelID);
        searchParams.append("startDate", startDate);
        searchParams.append("endDate", endDate);
        searchParams.append("departure", departure);
        searchParams.append("destination", destination);
        searchParams.append("adults", adults);
        searchParams.append("childrenUnder3", children_under_3);
        searchParams.append("childrenUnder10", children_under_10);
        searchParams.append("childrenUnder18", children_under_18);

        createAPIEndpoint(ENDPOINTS.getTrip + '?' + searchParams).fetch().then((res) => {
            this.setState({ offer: res.data});
            this.setState({ hotelID: res.data.tripOffer.hotelOffer.id});
            this.setState({ startDate: res.data.tripOffer.startDate});
            this.setState({ endDate: res.data.tripOffer.endDate});
            this.setState({ roomPrices: res.data.tripOffer.hotelOffer.roomsConfigurations});
            this.setState({ selectedRoomType: res.data.tripOffer.hotelOffer.roomsConfigurations[0].name});
            this.setState({ transportPrices: res.data.tripOffer.transportOffers});
            this.setState({ selectedTransportType: res.data.tripOffer.transportOffers[0].transportName});
        });
        
        
        const key = Object.keys(localStorage)
        const user = localStorage.getItem(key)
        if(user === null){
            this.isLogged = false
        }
        else{
            this.isLogged = true
            this.setState({user: user})
        }
    }

    handleTypesChange = () => {
        let selectRoomType = document.querySelector('#selectRoomType')
        var optionsRoomType = selectRoomType.getElementsByTagName('option');
        this.setState({ selectedRoomType: optionsRoomType[selectRoomType.selectedIndex].value});

        let selectTransportType = document.querySelector('#selectTransport')
        var optionsTransportType = selectTransportType.getElementsByTagName('option');
        this.setState({ selectedTransportType: optionsTransportType[selectTransportType.selectedIndex].value});
    }

    handleReserve(event) {
        const key = Object.keys(localStorage)
        const userFromStorage = localStorage.getItem(key)
        const parsedUser = JSON.parse(userFromStorage);
        event.preventDefault();
        var reservationObject = {
            'hotelId': this.state.hotelID,
            'startDate': this.state.startDate,
            'endDate': this.state.endDate,
            "roomTypeId": this.state.roomPricesConvertedWithId[this.state.selectedRoomType],
            "transportFromId": this.state.transportFromIdConverted[this.state.selectedTransportType],
            "transportToId": this.state.transportToIdConverted[this.state.selectedTransportType],
            "username": parsedUser.username,
            "adults": parseInt(adults),
            "childrenUnder3": parseInt(children_under_3),
            "childrenUnder10": parseInt(children_under_10), 
            "childrenUnder18": parseInt(children_under_18)
        }
        fetch(BASE_URL + ENDPOINTS.reserve, {
            method: 'POST',
            body: JSON.stringify(reservationObject),
            headers: {
                'Content-Type': 'application/json',
            }
        }).then(
            response => {
                response.json()
                    .then(
                        resp => {
                            console.log(resp.success)
                            if (resp.success === true){
                                const searchParams = new URLSearchParams();
                                searchParams.append("price", resp.price);
                                searchParams.append("username", parsedUser.username);
                                searchParams.append("reservationId", resp.reservationId);
                                window.location.href = "/payment?" + searchParams;
                            }
                            else{
                                window.location.href = "/reservationError";
                            }
                        }
                        
                    )
        })
    }
    
    convertRoomPricesList = () =>{
        this.state.roomPrices.map(
            room => {
                this.state.roomPricesConverted[room.name] = room.price
                this.state.roomPricesConvertedWithId[room.name] = room.roomtypeId
            })
    }

    convertTransportPricesList = () =>{
        this.state.transportPrices.map(
            transport => {
                this.state.transportPricesConverted[transport.transportName] = transport.price
                this.state.transportCitiesConverted[transport.transportName] = transport.departureCity
                this.state.transportFromIdConverted[transport.transportName] = transport.transportIDFrom
                this.state.transportToIdConverted[transport.transportName] = transport.transportIDTo
            })
    }
    
    render() {
        this.state.roomTypeList = this.state.offer.tripOffer.hotelOffer.roomsConfigurations
        this.state.transportTypeList = this.state.offer.tripOffer.transportOffers
        this.convertRoomPricesList()
        this.convertTransportPricesList()
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Szczegóły oferty</h3>
                <div className="row mt-3">
                    <div className="col-sm offerDetails">
                        <img src="https://images.pexels.com/photos/20787/pexels-photo.jpg?auto=compress&cs=tinysrgb&h=350" alt="new"/>
                    </div>

                    <div className="col border border-dark list-group-item text-center">
                        <h3 className="mt-4 mx-auto">Sprawdź cenę</h3>
                        <label> Typ pokoju</label>
                        <select id="selectRoomType">
                            {this.state.roomTypeList.map(room => (
                                <option key={room.name}>{room.name}</option>
                            ))}
                        </select>
                        <label className="mt-5"> Rodzaj transportu</label>
                        <select id="selectTransport">
                            {this.state.transportTypeList.map(transport => (
                                <option key={transport.transportName}>{transport.transportName}</option>
                            ))}
                        </select>
                        <button onClick={this.handleTypesChange} className="mt-5 mx-auto row center-column">Przelicz</button>
                        
                    </div>
                    <form className="col border border-dark list-group-item text-center reservationForm" onSubmit={this.handleReserve.bind(this)}>
                    <h5 className="mt-5">Cena: </h5>
                    <h5 className="mt-5">{this.state.roomPricesConverted[this.state.selectedRoomType] + this.state.transportPricesConverted[this.state.selectedTransportType]} PLN</h5>
                    <p className={(!this.isLogged ?  'mt-5 text-danger' : 'd-none')}> Zaloguj się, aby dokonać rezerwacji</p>
                    <input className={(this.isLogged ?  'mt-5 mx-auto row center-column' : 'd-none')} type="submit" value="Rezerwuj"/>
                    </form>
                </div>
                <div className="row mt-3">
                    <div className="col center-column text-center">
                        <h3>
                            Najważniejsze informacje o wyciecze:
                        </h3>
                        <div className="tripInfo">
                            <p>Informacje o ofercie</p>
                            <p>Kraj: {this.state.offer.tripOffer.hotelOffer.destinationCountry}</p>
                            <p>Miasto: {this.state.offer.tripOffer.hotelOffer.destinationCity}</p>
                        </div>
                        <div className="hotelInfo">
                            <p>Informacje o hotelu</p>
                            <p>Nazwa hotelu: {this.state.offer.tripOffer.hotelOffer.name} </p>
                            <p>Średnia z ocen: {this.state.offer.tripOffer.hotelOffer.rating}</p>
                            <p>Liczba gwiazdek: {this.state.offer.tripOffer.hotelOffer.stars}</p>
                            <p>Opis: {this.state.offer.tripOffer.hotelOffer.description}</p>
                        </div>
                        <div className="transportInfo">
                            <p>Informacje o transporcie</p>
                            <p>Miasto wyjazdu: {this.state.transportCitiesConverted[this.state.selectedTransportType]} </p>
                        </div>
                    </div>
                </div>
            </div>
            
        )
    }
}