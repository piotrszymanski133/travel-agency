import React, {Component, useState} from 'react'

const queryParams = new URLSearchParams(window.location.search);
var price = queryParams.get('price');
var username = queryParams.get('username');

export class Payment extends Component {
    constructor(props) {
        super(props)
    }

    handleSubmitPayment = (data) => {

    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h4 className="text-center mt-5">Rezerwacja przeszła pomyślnie. Masz 1 minutę, aby dokonać płatności.</h4>
                <div className="paymentForm">
                    <h5 className="text-center mb-5">Podaj numer karty, aby zapłacić za wycieczkę</h5>
                    <h5 className="text-center">Cena: {price} PLN</h5>
                    <form onSubmit={this.handleSubmitPayment}>
                        <label htmlFor="username">Numer karty: </label>
                        <input
                            type="text"
                            onChange={({ target }) => this.setState(target.value)}
                        />
                        <button className="button mt-3" type="submit">Zapłać</button>
                    </form>
                </div>
            </div>
        )
    }
}