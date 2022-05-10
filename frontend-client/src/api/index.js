import axios from 'axios'

export const BASE_URL = 'http://localhost:17577/';

export const ENDPOINTS = {
    trip: 'Trip',
    getTrip: 'Trip/GetTrip',
    getDestinations: 'Trip/GetDestinations',
    login: 'Login',
    reserve: 'Trip/ReserveTrip',
    payment: 'Trip/Payment',
    getUserTrips: 'Trip/GetUserTrips'
}

export const createAPIEndpoint = endpoint => {

    let url = BASE_URL + endpoint;
    return {
        fetch: () => axios.get(url),
        post: () => axios.post(url)
    }
}