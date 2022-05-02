import axios from 'axios'

export const BASE_URL = 'http://localhost:8081/';

export const ENDPOINTS = {
    trip: 'Trip'
}

export const createAPIEndpoint = endpoint => {

    let url = BASE_URL + endpoint + '/';
    return {
        fetch: () => axios.get(url),
    }
}