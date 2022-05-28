import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'
import { Store } from 'react-notifications-component';

const Chat = () => {
    const [ connection, setConnection ] = useState(null);
    const [ chat, setChat ] = useState([]);
    const latestChat = useRef(null);

    latestChat.current = chat;
    
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

                    connection.on('SendMessage', message => {
                        const updatedChat = [...latestChat.current];
                        updatedChat.push(message);

                        setChat(updatedChat);
                        Store.addNotification({
                            title: "Powiadomienie!",
                            message: message.message,
                            type: "info",
                            insert: "top",
                            container: "top-left",
                            animationIn: ["animate__animated", "animate__fadeIn"],
                            animationOut: ["animate__animated", "animate__fadeOut"],
                            dismiss: {
                                duration: 5000,
                                onScreen: true
                            }
                        });
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);
};

export default Chat;