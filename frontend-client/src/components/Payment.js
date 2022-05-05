import React, {Component, useState} from 'react'


export class Payment extends Component {
    constructor(props) {
        super(props)
        this.state = {
            cardNumber: ""
        }
        
    }

    handleSubmitPayment = (data) => {

    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Rezerwacja przeszła pomyślnie. Masz 1 minutę, aby dokonać płatności.</h3>
                <div className="paymentForm">
                    <h3 className="text-center mb-5">Podaj numer karty, aby zapłacić za wycieczkę</h3>
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