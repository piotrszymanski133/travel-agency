import React, {Component, useEffect, useState} from 'react'
import {BASE_URL, createAPIEndpoint, ENDPOINTS} from "../api";
import Purchase from './Hub/Purchase'
import PopularCountry from './Hub/PopularCountry'
import PopularTripConfiguration from './Hub/PopularTripConfiguration'
import axios from 'axios';

import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'
import { Store } from 'react-notifications-component';

const queryParams = new URLSearchParams(window.location.search);
var hotelID = queryParams.get('hotelID');
var when = queryParams.get('when');
var departure = queryParams.get('departure');
var destination = queryParams.get('destination');
var adults = queryParams.get('adults');
var childrenUnder3 = queryParams.get('childrenUnder3')
var childrenUnder10 = queryParams.get('childrenUnder10')
var childrenUnder18 = queryParams.get('childrenUnder18')

const convertDate = (inputFormat) => {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat)
    return [pad(d.getFullYear()), pad(d.getMonth()+1), d.getDate()].join('-')
}

const OfferDetailsHook = (props) => {
    const [ offer, setOffer ] = useState({ tripOffer: { startDate: "", endDate: "", hotelOffer: {
            roomConfigurations: []
            },
            transportOffers: []
        
        }});
    const [ startDate, setStartDate] = useState(null);
    const [ endDate, setEndDate ] = useState(null);
    const [ roomTypeList, setRoomTypeList] = useState([]);
    const [ transportTypeList, setTransportTypeList] = useState([]);
    const [ selectedRoomType, setSelectedRoomType ] = useState("");
    const [ selectedTransportType, setSelectedTransportType ] = useState("");
    const [ roomQuantities, setRoomQuantities ] = useState({});
    const [ roomPrices, setRoomPrices] = useState([])
    const [ roomPricesConverted, setRoomPricesConverted] = useState({});
    const [ roomPricesConvertedWithId, setRoomPricesConvertedWithId ] = useState({});
    const [ isLogged, setIsLogged ] = useState();
    const [ user, setUser ] = useState();
    const [ transportPrices, setTransportPrices ] = useState([]);
    const [ transportPricesConverted, setTransportPricesConverted ] = useState({});
    const [ transportCitiesConverted, setTransportCitiesConverted ] = useState({});
    const [ transportFromIdConverted, setTransportFromIdConverted ] = useState({});
    const [ transportToIdConverted, setTransportToIdConverted ] = useState({});
    const [ transportQuantities, setTransportQuantities] = useState({});
    const [ promoCode, setPromoCode ] = useState("");
    const [ isGetFinished, setIsGetFinished ] = useState(true);
    const [roomSelectedPrice, setRoomSelectedPrice] = useState(0)
    const [transportSelectedPrice, setTransportSelectedPrice] = useState(0)
    const [ price, setPrice ] = useState(0);
    const [ startDateTo, setStartDateTo] = useState()
    const [ endDateTo, setEndDateTo] = useState()
    
    const [ connection, setConnection ] = useState(null);
    const [ message, setMessage ] = useState(null);
    const [ roomTypeListMessage, setRoomTypeListMessage] = useState([]);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7048/hubs/test')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');
                    connection.on('SendHotelStateChangeMessage', message => {
                        var messageHotelId = message.hotelId
                        var convertedStartDate = new Date(startDate)
                        var convertedEndDate = new Date(endDate)
                        var messageStartDate = new Date(message.startDate)
                        var messageEndDate = new Date(message.endDate)
                        if(messageHotelId == hotelID){
                            if(convertedStartDate <= messageEndDate && messageStartDate <= convertedEndDate){
                                console.log("Hotel's parameters are equals")
                                connection.send('GetTrip', parseInt(hotelID), convertedStartDate, convertedEndDate,
                                    parseInt(adults), parseInt(childrenUnder3), parseInt(childrenUnder10),
                                    parseInt(childrenUnder18), departure.toString())
                                connection.on('SendTripOffer', message => {
                                    setMessage(message)
                                    setRoomPrices(message.tripOffer.hotelOffer.roomsConfigurations)
                                    setTransportPrices(message.tripOffer.transportOffers)
                                });
                            }
                        }
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    useEffect(() => {
        async function fetchData() {
            if (when !== null) {
                var date = when.split("-");
                var startDate = date[0].replace(/\s/g, "");
                var endDate = date[1].replace(/\s/g, "")
                startDate = convertDate(startDate)
                endDate = convertDate(endDate)
                setStartDate(startDate)
                setEndDate(endDate)
            }

            const searchParams = new URLSearchParams();
            if (destination === "" || destination === "dowolnie") {
                destination = "any"
            }
            searchParams.append("hotelID", hotelID);
            searchParams.append("startDate", startDate);
            searchParams.append("endDate", endDate);
            searchParams.append("departure", departure);
            searchParams.append("destination", destination);
            searchParams.append("adults", adults);
            searchParams.append("childrenUnder3", childrenUnder3);
            searchParams.append("childrenUnder10", childrenUnder10);
            searchParams.append("childrenUnder18", childrenUnder18);

            const result = await axios.get(BASE_URL+ ENDPOINTS.getTrip + '?' + searchParams);
            setOffer(result.data);
            setStartDateTo(result.data.tripOffer.startDate)
            setEndDateTo(result.data.tripOffer.endDate)
            setRoomTypeList(result.data.tripOffer.hotelOffer.roomsConfigurations)
            setSelectedRoomType(result.data.tripOffer.hotelOffer.roomsConfigurations[0].name)
            setTransportTypeList(result.data.tripOffer.transportOffers)
            setSelectedTransportType(result.data.tripOffer.transportOffers[0].transportName)
            setRoomPrices(result.data.tripOffer.hotelOffer.roomsConfigurations)
            setTransportPrices(result.data.tripOffer.transportOffers)
            

            const key = Object.keys(localStorage)
            const user = localStorage.getItem(key)
            if (user === null) {
                setIsLogged(false)
            } else {
                setIsLogged(true)
                setUser(user)
            }
        }
        fetchData();
        
    }, []);

    useEffect(() => {
        setRoomSelectedPrice(roomPricesConverted[selectedRoomType])
        setTransportSelectedPrice(transportPricesConverted[selectedTransportType])
        setPrice(roomSelectedPrice + transportSelectedPrice)

    });
    
    
    const handleTypesChange = () => {
        let selectRoomType = document.querySelector('#selectRoomType')
        var optionsRoomType = selectRoomType.getElementsByTagName('option');
        setSelectedRoomType(optionsRoomType[selectRoomType.selectedIndex].value)

        let selectTransportType = document.querySelector('#selectTransport')
        var optionsTransportType = selectTransportType.getElementsByTagName('option');
        setSelectedTransportType(optionsTransportType[selectTransportType.selectedIndex].value)

        setRoomSelectedPrice(roomPricesConverted[selectedRoomType])
        setTransportSelectedPrice(transportPricesConverted[selectedTransportType])
        setPrice(roomSelectedPrice + transportSelectedPrice)
    }
    
    const handleReserve = async event => {
        const key = Object.keys(localStorage)
        const userFromStorage = localStorage.getItem(key)
        const parsedUser = JSON.parse(userFromStorage);
        event.preventDefault();
        var reservationObject = {
            'hotelId': hotelID,
            'startDate': startDateTo,
            'endDate': endDateTo,
            "roomTypeId": roomPricesConvertedWithId[selectedRoomType],
            "transportFromId": transportFromIdConverted[selectedTransportType],
            "transportToId": transportToIdConverted[selectedTransportType],
            "username": parsedUser.username,
            "adults": parseInt(adults),
            "childrenUnder3": parseInt(childrenUnder3),
            "childrenUnder10": parseInt(childrenUnder10),
            "childrenUnder18": parseInt(childrenUnder18),
            "promoCode": promoCode
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

    
    return (
        roomPrices.map(
            room => {
                roomPricesConverted[room.name] = room.price
                roomPricesConvertedWithId[room.name] = room.roomtypeId
                roomQuantities[room.name] = room.quantity
            }),
        transportPrices.map(
            transport => {
                transportPricesConverted[transport.transportName] = transport.price
                transportCitiesConverted[transport.transportName] = transport.departureCity
                transportFromIdConverted[transport.transportName] = transport.transportIDFrom
                transportToIdConverted[transport.transportName] = transport.transportIDTo
                transportQuantities[transport.transportName] = transport.quantity
            }),
        <div className="p-5 mb-4 align-items-center">
            <PopularTripConfiguration/>
            <PopularCountry/>
            <Purchase hotelId={hotelID}/>
            <h3 className="text-center mt-5">Szczegóły oferty</h3>
            <div className="row mt-3">
                <div className="col-sm offerDetails">
                    <img className="offerDetailsImg" src="https://i.content4travel.com/cms/img/u/desktop/prodsliderphoto/fuepaja_0.jpg?version=2.6.19" alt="new"/>
                </div>
                <div className="col border border-dark list-group-item text-center">
                    <h3 className="mt-4 mx-auto">Sprawdź cenę</h3>
                    <label> Typ pokoju</label>
                    <select id="selectRoomType">
                        {roomTypeList.map(room => (
                            <option key={room.name}>{room.name}</option>
                        ))}
                    </select>
                    <p> Ilość: {roomQuantities[selectedRoomType]}</p>
                    <label className="mt-4"> Rodzaj transportu</label>
                    <select id="selectTransport">
                        {transportTypeList.map(transport => (
                            <option key={transport.transportName}>{transport.transportName}</option>
                        ))}
                    </select>
                    { selectedTransportType !== "Own"  &&
                    <p> Ilość: {transportQuantities[selectedTransportType]}</p>
                    }
                    <button onClick={handleTypesChange} className="mt-5 mx-auto row center-column">Zmień konfigurację</button>
                </div>
                <form className="col border border-dark list-group-item text-center reservationForm" onSubmit={handleReserve.bind(this)}>
                    <h5 className="mt-5">Cena: </h5>
                    <h5 className="mt-5">{ isGetFinished === true  && price} PLN</h5>
                    <label>Kod promocyjny: </label>
                    <input
                        type="text"
                        value={promoCode}
                        onChange={({ target }) => setPromoCode(target.value)}
                    />
                    <p className={(!isLogged ?  'mt-5 text-danger' : 'd-none')}> Zaloguj się, aby dokonać rezerwacji</p>
                    <input className={(isLogged ?  'mt-5 mx-auto row center-column' : 'd-none')} type="submit" value="Rezerwuj"/>
                </form>

            </div>

            <div className="row mt-3">
                <div className="col center-column text-center">
                    <h3>
                        Najważniejsze informacje o wyciecze:
                    </h3>
                    <div className="tripInfo">
                        <p>Informacje o ofercie</p>
                        <p>Kraj: {offer.tripOffer.hotelOffer.destinationCountry}</p>
                        <p>Miasto: {offer.tripOffer.hotelOffer.destinationCity}</p>
                    </div>
                    <div className="hotelInfo">
                        <p>Informacje o hotelu</p>
                        <p>Nazwa hotelu: {offer.tripOffer.hotelOffer.name} </p>
                        <p>Średnia z ocen: {offer.tripOffer.hotelOffer.rating}</p>
                        <p>Liczba gwiazdek: {offer.tripOffer.hotelOffer.stars}</p>
                        <p>Opis: {offer.tripOffer.hotelOffer.description}</p>
                    </div>
                    <div className="transportInfo">
                        <p>Informacje o transporcie</p>
                        
                    </div>
                </div>
            </div>
        </div>

    )
    
}

export default OfferDetailsHook;