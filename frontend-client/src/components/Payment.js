import React, { Component } from 'react'


export class Payment extends Component {
    constructor(props) {
        super(props)
        
    }
    
    render() {
        return (
            <div className="p-5 mb-4 align-items-center">
                <h3 className="text-center mt-5">Rezerwacja przeszła pomyślnie. Masz 1 minutę, aby dokonać płatności.</h3>
            </div>
        )
    }
}