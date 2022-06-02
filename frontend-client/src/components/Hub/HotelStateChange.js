import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'
import { Store } from 'react-notifications-component';

const HotelStateChange = (props) => {
    const [ connection, setConnection ] = useState(null);
    const hotelId = useRef(props.hotelId).current
    const startDate = new Date(useRef(props.startDate).current)
    const endDate = new Date(useRef(props.endDate).current)
    const adults = useRef(props.adults).current
    const childrenUnder3 = useRef(props.childrenUnder3).current
    const childrenUnder10 = useRef(props.childrenUnder10).current
    const childrenUnder18 = useRef(props.childrenUnder18).current
    const departure = useRef(props.departure).current
    const [ message, setMessage ] = useState(null);
    
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
                        var messageStartDate = new Date(message.startDate)
                        var messageEndDate = new Date(message.endDate)
                        console.log(startDate)
                        console.log(endDate)
                        console.log(messageStartDate)
                        console.log(messageEndDate)
                        if(messageHotelId == hotelId){
                            if(startDate <= messageEndDate && messageStartDate <= endDate){
                                console.log("Hotel's parameters are equals")
                                connection.send('GetTrip', parseInt(hotelId), startDate, endDate,
                                                parseInt(adults), parseInt(childrenUnder3), parseInt(childrenUnder10),
                                                parseInt(childrenUnder18), departure.toString())
                                connection.on('SendTripOffer', message => {
                                    console.log(message)
                                    setMessage(message)
                                });
                            }
                        }
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);
};

export default HotelStateChange;