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
        fieldType, minValue, label, value, visited, valid,
        onChange, onBlur, onFocus, validationMessage,
    } = fieldProps;
    const invalid = !valid && visited;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
                <DateRangePicker
                    placeholder="Select Date Range"
                    initialSettings={{ startDate: '4/30/2022'}}>
                    <button>dowolnie</button>
                </DateRangePicker>
            </label>
            { invalid &&
            (<div className="required">{validationMessage}</div>) }
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
                value={value.name_pl}
                onChange={onChange}>
                {options.map(option => (
                    <option key={option}>{option}</option>
                ))}
            </select>
        </div>
    )
}

const handleSubmit = (data, event) => {
    alert(`Kiedy?  ${data.when} \nDokąd?  ${data.country} \nIle osób?  ${data.people}`);
    event.preventDefault();
}

const requiredValidator = (value) => {
    return value ? "" : "This field is required";
}

export class SearchForm extends Component {
    
    render() {
        return (
            <Form
                onSubmit={handleSubmit}
                initialValues={{
                    when: "", country: "", people: ""
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