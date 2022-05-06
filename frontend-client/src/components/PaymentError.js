import React from "react";

const queryParams = new URLSearchParams(window.location.search);
var userName = queryParams.get('userName');
var reservationId = queryParams.get('reservationId');

const PaymentError = ()  =>{
    return (
        <div className="logout text-center">
            <h3>Pojawił się błąd podczas rezerwacji o id {reservationId} w podanym czasie dla user {userName}.</h3>
        </div>
    );

};

export default PaymentError;