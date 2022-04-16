import React, { Component } from 'react'
import { createAPIEndpoint, ENDPOINTS, BASE_URL } from '../api'

export class Offer extends Component {
    constructor(props) {
        super(props)

        this.state = {
            offers: []
        }
    }
    
    componentDidMount(){
        createAPIEndpoint(ENDPOINTS.weatherForecast).fetch().then((res) => {
            this.setState({ offers: res.data});
        });
    }

    render() {
        return (
            <div>
                <h2 className="text-center">Offers</h2>
                <div className = "row">
                    <table className = "table table-striped table-bordered">
                        <thead>
                        <tr>
                            <th> Date </th>
                            <th> Temperature in C </th>
                            <th> Temperature in F </th>
                            <th> Summary </th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            this.state.offers.map(
                                offer =>
                                    <tr key = {offer.id}>
                                        <td> { offer.date} </td>
                                        <td> { offer.temperatureC} </td>
                                        <td> { offer.temperatureF} </td>
                                        <td> { offer.summary} </td>
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