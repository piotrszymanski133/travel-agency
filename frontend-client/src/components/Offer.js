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
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h2 className="text-center mt-5">Znalezione oferty</h2>
                <div className = "row">
                    <table className = "table table-striped table-bordered">
                        <thead>
                        <tr>
                            <th> Hotel ID </th>
                            <th> Hotel name </th>
                            <th> Transport ID </th>
                            <th> Transport name </th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            this.state.offers.trips.map(
                                offer =>
                                    <tr key = {offer.hotel.id}>
                                        <td> { offer.hotel.id} </td>
                                        <td> { offer.hotel.name} </td>
                                        <td> { offer.transport.id} </td>
                                        <td> { offer.transport.name} </td>
                                    </tr>
                            )
                        }
                        </tbody>
                    </table>

                </div>

            </div>
        )
    }
}