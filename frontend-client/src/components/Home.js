import React,{Component} from 'react';
import {SearchForm} from "./SearchForm";

const Aux = props => props.children;

export class Home extends Component{

    render(){
        return(
            <SearchForm></SearchForm>,
            <div className="position-absolute top-50 m-0">
                <p>Biuro Podróży <strong> ITAKA </strong> - Twoje wymarzone wakacje. Wyjedź z nami na wczasy all inclusive! </p>
            </div>
        )
    }
}