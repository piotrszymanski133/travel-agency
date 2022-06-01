import React, {Component} from 'react';
import {SearchForm} from './SearchForm';

const Aux = props => props.children;

export class Home extends Component{

    render(){
        return[
            <div key={1} className="baner">
                <p>Biuro Podróży <strong> ITAKA </strong> - Twoje wymarzone wakacje. Wyjedź z nami na wczasy all inclusive! </p>
            </div>,
            <div key={2} className="mt-4">
                <SearchForm className="row"></SearchForm>
            </div>
        ]
    }
}