import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import {Home} from './components/Home';
import {Offer} from './components/Offer';
import {Navigation} from './components/Navigation';
import {OfferDetails} from "./components/OfferDetails";
import {Destinations} from "./components/Destinations";
import {Reservation} from "./components/Reservation";
import {UserTrips} from "./components/UserTrips";
import Payment from "./components/Payment"
import Login from "./components/Login"
import Logout from "./components/Logout"
import LoginError from "./components/LoginError"
import ReservationError from "./components/ReservationError"
import PaymentOK from "./components/PaymentOK";
import PaymentError from "./components/PaymentError";
import PaymentErrorTimeout from "./components/PaymentErrorTimeout";


function App() {
  return (
      <BrowserRouter>
          <div className="container w-75 justify-content-center">
              
              <Navigation/>

              <Routes>
                  <Route path="/" element={<Home/>} exact/>
                  <Route path="/offer" element={<Offer/>}/>
                  <Route path="/offer_details" element={<OfferDetails/>}/>
                  <Route path="/destinations" element={<Destinations/>}/>
                  <Route path="/login" element={<Login/>}/>
                  <Route path="/logout" element={<Logout/>}/>
                  <Route path="/reservation" element={<Reservation/>}/>
                  <Route path="/loginError" element={<LoginError/>}/>
                  <Route path="/payment" element={<Payment/>}/>
                  <Route path="/reservationError" element={<ReservationError/>}/>
                  <Route path="/paymentOk" element={<PaymentOK/>}/>
                  <Route path="/paymentError" element={<PaymentError/>}/>
                  <Route path="/paymentErrorTimeout" element={<PaymentErrorTimeout/>}/>
                  <Route path="/userTrips" element={<UserTrips/>}/>
              </Routes>
          </div>
      </BrowserRouter>
  );
}

export default App;
