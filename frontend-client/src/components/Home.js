import React,{Component} from 'react';
import {SearchForm} from './SearchForm';
import {NewSearchForm} from './NewSearchForm';

const Aux = props => props.children;

export class Home extends Component{

    render(){
        return(
            <div className="mt-5">
                <SearchForm className="row"></SearchForm>
                <NewSearchForm className="row"></NewSearchForm>
            </div>
        )
    }
}