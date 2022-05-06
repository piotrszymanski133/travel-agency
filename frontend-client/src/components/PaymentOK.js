import React from "react";

const queryParams = new URLSearchParams(window.location.search);
var userName = queryParams.get('userName');
var reservationId = queryParams.get('reservationId');

const PaymentOK = ()  =>{
    return (
        <div className="logout text-center">
            <h3>Płatność przebiegła prawidłowo. Udało się zakupić ofertę dla użytkownika {userName}</h3>
        </div>
    );

};

export default PaymentOK;