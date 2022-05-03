import React, {Component} from "react";
import { Form, Field } from "@progress/kendo-react-form";
import destination_countries from "./destinations_countries";
import departures_countries from "./departures_countries";
import DateRangePicker from 'react-bootstrap-daterangepicker';
// a tool like webpack, you can do the following:
import 'bootstrap/dist/css/bootstrap.css';
// you will also need the css that comes with bootstrap-daterangepicker
import 'bootstrap-daterangepicker/daterangepicker.css';

const current = new Date();
//var startDate = new Date();
//var endDate = new Date();
//startDate.setDate(current.getDay() + 7)
//endDate.setDate(current.getDay() + 14)

const DateInput = (fieldProps) => {
    const {
        label, onBlur, value, onChange, onFocus} = fieldProps;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <DateRangePicker onApply={onChange} onCancel={(event, picker)=>handleDateCancel(event, picker)} initialSettings={ {minDate:current, startDate: '5/20/2022', endDate: '5/27/2022'}}>
                <input readOnly
                       value={value}
                       type="text"/>
            </DateRangePicker>
        </div>
    );
};

const NumberInputAdults = (fieldProps) => {
    const {
        fieldType, minValue, maxValue, label, value, visited, valid,
        onChange, onBlur, onFocus, validationMessage,
    } = fieldProps;
    const invalid = !valid && visited;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <input
                type={fieldType}
                min={minValue}
                max={maxValue}
                className={invalid ? "invalid" : ""}
                value={value}
                onChange={onChange} />
            { invalid &&
            (<div className="required">{validationMessage}</div>) }
        </div>
    );
};

const NumberInputChildren = (fieldProps) => {
    const {
        fieldType, minValue, maxValue, label, value,
        onChange, onBlur, onFocus, 
    } = fieldProps;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <input
                type={fieldType}
                min={minValue}
                max={maxValue}
                value={value}
                onChange={onChange} />
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

const handleDateCancel = (event, picker) => {
    picker.setStartDate(new Date())
    picker.setEndDate(new Date())
}

export class SearchForm extends Component {
    
    handleSubmit = (data) => {
        const searchParams = new URLSearchParams();
        searchParams.append("when", data.when);
        searchParams.append("departure", data.departure);
        searchParams.append("destination", data.destination);
        searchParams.append("adults", data.adults);
        searchParams.append("childrenUnder3", data.children_under_3);
        searchParams.append("childrenUnder10", data.children_under_10);
        searchParams.append("childrenUnder18", data.children_under_18);
        window.location.href = "/offer?" + searchParams;
    }
    
    render() {
        return (
            <Form
                onSubmit={this.handleSubmit.bind(this)}
                initialValues={{
                    when: "05/20/2022 - 05/27/2022", departure: "dowolnie", destination: "dowolnie", adults: "0",
                    children_under_3:"0", children_under_10:"0", children_under_18:"0"
                }}
                render={(formRenderProps) => (
                    <form onSubmit={formRenderProps.onSubmit}>
                        <h5>Znajdź wakacje marzeń!</h5>

                        <Field
                            label="Kiedy?"
                            name="when"
                            component={DateInput}/>

                        <Field
                            label="Skąd?"
                            name="departure"
                            component={DropDown}
                            options={departures_countries}/>

                        <Field
                            label="Dokąd?"
                            name="destination"
                            component={DropDown}
                            options={destination_countries}/>

                        <Field
                            label="Ile osób dorosłych?"
                            name="adults"
                            fieldType="number"
                            minValue="1"
                            maxValue="10"
                            component={NumberInputAdults}/>

                        <Field
                            label="Ile dzieci poniżej 3 roku życia?"
                            name="children_under_3"
                            fieldType="number"
                            minValue="0"
                            maxValue="5"
                            component={NumberInputChildren}/>

                        <Field
                            label="Ile dzieci w wieku 3-10 lat?"
                            name="children_under_10"
                            fieldType="number"
                            minValue="0"
                            maxValue="5"
                            component={NumberInputChildren}/>

                        <Field
                            label="Ile dzieci w wieku 10-18 lat?"
                            name="children_under_18"
                            fieldType="number"
                            minValue="0"
                            maxValue="5"
                            component={NumberInputChildren}/>

                        <input className="submitButton mt-4" type="submit" value="Szukaj" />
                        
                    </form>
                )}>
            </Form>
        );
    }
}