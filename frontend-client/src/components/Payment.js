import React, {useEffect, useState} from 'react'
import {BASE_URL, ENDPOINTS} from "../api";

const queryParams = new URLSearchParams(window.location.search);
var price = queryParams.get('price');
var reservationId = queryParams.get('reservationId');


const Payment = ()  => {

    const [cardNumber, setCardNumber] = useState();
    const [user, setUser] = useState();

    useEffect(() => {
        const loggedInUser = localStorage.getItem("user");
        if (loggedInUser) {
            const foundUser = JSON.parse(loggedInUser);
            setUser(foundUser);
        }
    }, []);

    const handleSubmitPayment = async e => {
            e.preventDefault();
            var reservationObject = {
                'userName': user.username,
                'reservationId': reservationId,
                'cardNumber': cardNumber
            }
            fetch(BASE_URL + ENDPOINTS.payment, {
                method: 'POST',
                body: JSON.stringify(reservationObject),
                headers: {
                    'Content-Type': 'application/json',
                }
            }).then(
                response => {
                    response.json()
                        .then(
                            resp => {
                                console.log(resp.success)
                                const searchParams = new URLSearchParams();
                                searchParams.append("userName", user.username);
                                searchParams.append("reservationId", reservationId);
                                if (resp.success === true) {
                                    window.location.href = "/paymentOk?" + searchParams;
                                }
                            }
                        )
                    if(response.status === 500) {
                        window.location.href = "/paymentError";
                    }
                })
        }

        return (
            <div className="p-5 mb-4 align-items-center">
                <h4 className="text-center mt-5">Rezerwacja przeszła pomyślnie. Masz 1 minutę, aby dokonać
                    płatności.</h4>
                <div className="paymentForm">
                    <h5 className="text-center mb-5">Podaj numer karty, aby zapłacić za wycieczkę o
                        id {reservationId}</h5>
                    <h5 className="text-center">Cena: {price} PLN</h5>
                    <form onSubmit={handleSubmitPayment}>
                        <label htmlFor="username">Numer karty: </label>
                        <input
                            type="text"
                            type="tel" pattern="\d*" maxLength="16"
                            onChange={({target}) => setCardNumber(target.value)}
                        />
                        <button className="button mt-3" type="submit">Zapłać</button>
                    </form>
                </div>
            </div>
        );
};

export default Payment;