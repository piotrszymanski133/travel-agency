import React, { Component} from "react";
import { Form, Field } from "@progress/kendo-react-form";
import countries from "./countries";
import ReactDOM from "react-dom/client";
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

const DropDownDeparture = ({ label, options,
                      onChange, onBlur, onFocus}) => {
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <select
                id="selectDeparture"
                onChange={onChange}>
                {options.map(option => (
                    <option key={option}>{option}</option>
                ))}
            </select>
        </div>
    )
}

const DropDownDestination = ({ label, value, options,
                      onChange, onBlur, onFocus}) => {
    
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <select
                id="selectDestination"
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


export class OfferSearchForm extends Component {

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        const queryParams = new URLSearchParams(window.location.search);
        const destination = queryParams.get('destination');
        const departure = queryParams.get('departure');
        let selectDeparture = document.querySelector('#selectDeparture')
        let selectDestination = document.querySelector('#selectDestination')
        var optionsDeparture = selectDeparture.getElementsByTagName('option');
        var optionsDestination = selectDestination.getElementsByTagName('option');
        var indexDeparture = 0;
        var indexDestination = 0;
        for(var i=0; i< optionsDeparture.length; i++){
            if(departure === optionsDeparture[i].value){
                indexDeparture = i;
            }
        }
        for(var i=0; i< optionsDestination.length; i++){
            if(destination === optionsDestination[i].value){
                indexDestination = i;
            }
        }
        selectDeparture.selectedIndex = indexDeparture;
        selectDestination.selectedIndex = indexDestination;
    }

    handleSubmit = (data, event) => {
        event.preventDefault();
        fetch('http://localhost:8081/Trip', {
            method: 'POST',
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(data)
        }).then(() => {
            console.log("Data was sent")
            const searchParams = new URLSearchParams();
            searchParams.append("when", data.when);
            searchParams.append("departure", data.departure);
            searchParams.append("destination", data.destination);
            searchParams.append("adults", data.adults);
            searchParams.append("children_under_3", data.children_under_3);
            searchParams.append("children_under_10", data.children_under_10);
            searchParams.append("children_under_18", data.children_under_18);
            window.location.href = "/offer/?" + searchParams;
        })
    }

    render() {
        return (
            <Form
                onSubmit={this.handleSubmit.bind(this)}
                initialValues={{
                    when: "dowolnie",departure: "dowolnie", adults: 0,
                    children_under_3:0, children_under_10:0, children_under_18:0
                }}
                render={(formRenderProps) => (
                    <form className="row offerSearchForm" onSubmit={formRenderProps.onSubmit}>

                        <div className="col-auto">
                        <Field
                            label="Kiedy?"
                            name="when"
                            component={DateInput}/>

                        <Field
                            label="Skąd?"
                            name="departure"
                            component={DropDownDeparture}
                            options={countries}/>

                        <Field
                            label="Dokąd?"
                            name="destination"
                            component={DropDownDestination}
                            options={countries}/>
                        </div>

                        <div className="col-auto">
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
                        </div>
                        
                        <div>
                            <input className="submitButtonOffer" type="submit" value="Szukaj" />
                        </div>
                        
                        

                    </form>
                )}>
            </Form>
        );
    }
}