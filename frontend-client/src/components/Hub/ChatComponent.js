import React, { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'
import { Store } from 'react-notifications-component';

export class ChatComponent extends Component {

    constructor(props)
    {
        super(props);
        this.state = {
            connection: null,
            chat: null
        }
    }

    componentWillMount()
    {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:7048/hubs/test')
            .withAutomaticReconnect()
            .build();

        this.setState({connection: newConnection})
        
        console.log(this.props.hotelID)
        if (this.state.connection) {
            this.state.connection.start()
                .then(result => {
                    console.log('Connected!');

                    this.state.connection.on('SendMessage', message => {
                        const updatedChat = [...this.state.latestChat.current];
                        updatedChat.push(message);

                        this.setState({ chat: updatedChat})
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
    }
}