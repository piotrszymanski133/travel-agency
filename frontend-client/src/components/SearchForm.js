import React, { Component } from 'react'
import countries from "./countries";
import Select from 'react-select';
import { Form, Field } from "@progress/kendo-react-form";


export class SearchForm extends Component {
    constructor(props) {
        super(props);
        this.state = {value: ''};

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        this.setState({value: event.target.value});
    }

    handleSubmit(event) {
        alert('Podano następujące imię: ' + this.state.value);
        event.preventDefault();
    }

    render() {
        return [
            <div className="mt-5">
                <p>Biuro Podróży <strong> ITAKA </strong> - Twoje wymarzone wakacje. Wyjedź z nami na wczasy all inclusive! </p>
            </div>,
            <form onSubmit={this.handleSubmit}>
                <h5>Znajdź wakacje marzeń!</h5>
                <label>Kiedy?</label>
                <label>Dokąd?</label>
                <Select options={ countries } />
                <label>Ile osób?</label>
                <input className="submitButton mt-4" type="submit" value="Szukaj" />
            </form>
            
        ];
    }
}