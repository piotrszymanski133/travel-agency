import React,{Component} from 'react';
import {SearchForm} from "./SearchForm";

export class Home extends Component{

    render(){
        return[
            <SearchForm></SearchForm>,
            <div className="position-absolute top-50">
                <p>Biuro Podróży <strong> ITAKA </strong> - Twoje wymarzone wakacje. Wyjedź z nami na wczasy all inclusive! </p>
            </div>
        ]
    }
}