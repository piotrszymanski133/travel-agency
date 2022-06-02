import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'
import { Store } from 'react-notifications-component';

const Purchase = (props) => {
    const [ connection, setConnection ] = useState(null);
    const hotelId = useRef(props)
    
    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:8081/hubs/test')
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        var hotelID = hotelId.current.hotelId
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');
                    connection.on('SendMessage', message => {
                        if(message.hotelId == hotelID){
                            Store.addNotification({
                                title: "Powiadomienie!",
                                message: message.message,
                                type: "info",
                                insert: "top",
                                container: "top-left",
                                animationIn: ["animate__animated", "animate__fadeIn"],
                                animationOut: ["animate__animated", "animate__fadeOut"],
                                dismiss: {
                                    duration: 8000,
                                    onScreen: true
                                }
                            });
                        }
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);
};

export default Purchase;