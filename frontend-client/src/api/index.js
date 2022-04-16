import axios from 'axios'

export const BASE_URL = 'https://localhost:5001/';

export const ENDPOINTS = {
    weatherForecast: 'WeatherForecast'
}

export const createAPIEndpoint = endpoint => {

    let url = BASE_URL + endpoint + '/';
    return {
        fetch: () => axios.get(url),
    }
}