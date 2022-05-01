import React, {Component} from "react";
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
        const current = new Date();return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <DateRangePicker onApply={onChange} onCancel={(event, picker)=>handleDateCancel(event, picker)} initialSettings={ {minDate:current}}>
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
    
const requiredValidator = (value) => {
    return value ? "" : "Podaj liczbę osób!";
}

export class SearchForm extends Component {

    handleSubmit = (data, event) => {
        console.log(data)
        //alert(`Kiedy?  ${data.when} \nSkąd?  ${data.departure} \nDokąd?  ${data.destination} \nIle osób dorosłych?  ${data.adults} \nIle dzieci poniżej 3 roku życia?  ${data.children_under_3} \nIle dzieci w wieku 3-10 lat?  ${data.children_under_10} \nIle dzieci w wieku 10-18 lat?  ${data.children_under_18}`);
        event.preventDefault();
        fetch('http://localhost:8081/WeatherForecast', {
            method: 'POST',
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(data)
        }).then(() => {
            console.log("Data was sent")
            window.location.href = "/offer?" + data.when;
        })
    }
    
    render() {
        return (
            <Form
                onSubmit={this.handleSubmit.bind(this)}
                initialValues={{
                    when: "dowolnie",departure: "dowolnie", destination: "dowolnie",adults: "",
                    children_under_3:"", children_under_10:"", children_under_18:""
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
                            options={countries}/>

                        <Field
                            label="Dokąd?"
                            name="destination"
                            component={DropDown}
                            options={countries}/>

                        <Field
                            label="Ile osób dorosłych?"
                            name="adults"
                            fieldType="number"
                            minValue="1"
                            maxValue="10"
                            component={NumberInputAdults}
                            validator={requiredValidator}/>

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