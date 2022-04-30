import React, { Component } from "react";
import { Form, Field } from "@progress/kendo-react-form";
import countries from "./countries";
import DateRangePicker from 'react-bootstrap-daterangepicker';
// a tool like webpack, you can do the following:
import 'bootstrap/dist/css/bootstrap.css';
// you will also need the css that comes with bootstrap-daterangepicker
import 'bootstrap-daterangepicker/daterangepicker.css';


const DateInput = (fieldProps) => {

    const {
        label, onBlur, value, onChange, onFocus} = fieldProps;
    const current = new Date();
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
                <DateRangePicker onApply={onChange} initialSettings={ {minDate:current }}>
                    <input 
                        type="text"
                        value={value}
                        className="form-control"/>
                </DateRangePicker>
            </label>
        </div>
    );
};

const NumberInput = (fieldProps) => {
    const {
        fieldType, minValue, label, value, visited, valid,
        onChange, onBlur, onFocus, validationMessage,
    } = fieldProps;
    const invalid = !valid && visited;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
                <input
                    type={fieldType}
                    min={minValue}
                    className={invalid ? "invalid" : ""}
                    value={value}
                    onChange={onChange} />
            </label>
            { invalid &&
            (<div className="required">{validationMessage}</div>) }
        </div>
    );
};

const DropDown = ({ label, value, options,
                      onChange, onBlur, onFocus}) => {
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <select
                value={value}
                onChange={onChange}>
                {options.map(option => (
                    <option key={option}>{option}</option>
                ))}
            </select>
        </div>
    )
}
 
const handleSubmit = (data, event) => {
    console.log(data)
    alert(`Kiedy?  ${data.when} \nDokąd?  ${data.country} \nIle osób?  ${data.people}`);
    event.preventDefault();
}

const requiredValidator = (value) => {
    return value ? "" : "Podaj liczbę osób!";
}

export class SearchForm extends Component {
    
    render() {
        return (
            <Form
                onSubmit={handleSubmit}
                initialValues={{
                    when: "dowolnie", country: "dowolnie", people: ""
                }}
                render={(formRenderProps) => (
                    <form onSubmit={formRenderProps.onSubmit}>
                        <h5>Znajdź wakacje marzeń!</h5>

                        <Field
                            label="Kiedy?"
                            name="when"
                            component={DateInput}/>

                        <Field
                            label="Dokąd?"
                            name="country"
                            component={DropDown}
                            options={countries}/>

                        <Field
                            label="Ile osób?"
                            name="people"
                            fieldType="number"
                            minValue="1"
                            component={NumberInput}
                            validator={requiredValidator}/>

                        <input className="submitButton mt-4" type="submit" value="Szukaj" />
                        
                    </form>
                )}>
            </Form>
        );
    }
}