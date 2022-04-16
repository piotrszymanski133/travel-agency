import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import { Home } from './components/Home';
import { About } from './components/About';
import { Offer } from './components/Offer';
import { Navigation } from './components/Navigation';

function App() {
  return (
      <BrowserRouter>
          <div className="container">
              <h3 className="m-3 d-flex justify-content-center">
                  Itaka
              </h3>

              <Navigation/>

              <Routes>
                  <Route path="/" element={<Home/>} exact />
                  <Route path='/about' element={ <About />}/>
                  <Route path='/offer' element={ <Offer />}/>
              </Routes>
          </div>
      </BrowserRouter>
  );
}

export default App;
