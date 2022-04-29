import {BrowserRouter, Route, Routes} from 'react-router-dom';
import './App.css';
import { Home } from './components/Home';
import { Offer } from './components/Offer';
import { Navigation } from './components/Navigation';

function App() {
  return (
      <BrowserRouter>
          <div className="container">
              
              <Navigation/>

              <Routes>
                  <Route path="/" element={<Home/>} exact />
                  <Route path="/offer" element={<Offer/>}/>
              </Routes>
          </div>
      </BrowserRouter>
  );
}

export default App;
