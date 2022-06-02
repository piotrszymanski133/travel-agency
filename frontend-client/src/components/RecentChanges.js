import React, { useState, useEffect, useRef } from 'react';
import {BASE_URL, createAPIEndpoint, ENDPOINTS} from "../api";
import axios from "axios";
import { v4 as uuidv4 } from 'uuid';

const RecentChanges = (props) => {
    const [ hotelChanges, setHotelChanges ] = useState([]);
    const [ transportChanges, setTransportChanges ] = useState([]);

    useEffect(() => {
        async function fetchData() {
            const result = await axios.get(BASE_URL+ ENDPOINTS.getLastChanges);
            setHotelChanges(result.data._changeHotel);
            setTransportChanges(result.data._changeTransport);
            console.log(result.data._changeTransport)
        }
        fetchData();

    }, []);
    
    return(
        <div className="p-5 mb-4 align-items-center">
            <h3 className="text-center mt-5">Ostatnie zmiany dla hoteli</h3>
            <table className = "table table-striped table-bordered mt-4">
                <thead>
                <tr>
                    <th> Change Quantity </th>
                    <th> Hotel ID</th>
                    <th> StartDate </th>
                    <th> EndDate </th>
                </tr>
                </thead>
                <tbody>
                {
                    hotelChanges.map(
                        hotel =>
                            <tr key={uuidv4()}>
                                <td> {hotel.changeQuantity} </td>
                                <td> {hotel.hotelId} </td>
                                <td> {hotel.startDate} </td>
                                <td> {hotel.endDate} </td>

                            </tr>
                    )
                }
                </tbody>
            </table>
            <h3 className="text-center mt-5">Ostatnie zmiany dla transportu</h3>
            <table className = "table table-striped table-bordered mt-4">
                <thead>
                <tr>
                    <th> Change Places </th>
                    <th> Transport ID </th>
                    <th> Destination Places ID</th>
                    <th> Source Places ID </th>
                    <th> EndDate </th>
                </tr>
                </thead>
                <tbody>
                {
                    transportChanges.map(
                        transport =>
                            <tr key={uuidv4()}>
                                <td> {transport.changePlaces} </td>
                                <td> {transport.transportId} </td>
                                <td> {transport.destinationPlacesId} </td>
                                <td> {transport.sourcePlacesId} </td>
                                <td> {transport.transporttype} </td>

                            </tr>
                    )
                }
                </tbody>
            </table>
        </div>
    )
};

export default RecentChanges;