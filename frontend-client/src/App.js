import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import { Home } from './components/Home';
import { Offer } from './components/Offer';
import { Navigation } from './components/Navigation';
import {OfferDetails} from "./components/OfferDetails";

function App() {
  return (
      <BrowserRouter>
          <div className="container w-75 justify-content-center">
              
              <Navigation/>

              <Routes>
                  <Route path="/" element={<Home/>} exact/>
                  <Route path="/offer" element={<Offer/>}/>
                  <Route path="/offer_details" element={<OfferDetails/>}/>
              </Routes>
          </div>
      </BrowserRouter>
  );
}

export default App;
