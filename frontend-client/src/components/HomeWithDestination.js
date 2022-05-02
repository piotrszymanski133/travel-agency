import React,{Component} from 'react';
import {DestinationsSearchForm} from "./DestinationsSearchForm";

const Aux = props => props.children;

export class HomeWithDestination extends Component{

    render(){
        return[
            <div key={1} className="baner">
                <p>Biuro Podróży <strong> ITAKA </strong> - Twoje wymarzone wakacje. Wyjedź z nami na wczasy all inclusive! </p>
            </div>,
            <div key={2} className="mt-4">
                <DestinationsSearchForm className="row"></DestinationsSearchForm>
            </div>
        ]
    }
}