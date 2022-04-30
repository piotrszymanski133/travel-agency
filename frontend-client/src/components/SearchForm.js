import React, { Component } from "react";
import { Form, Field } from "@progress/kendo-react-form";
import countries from "./countries";


const Input = (fieldProps) => {
    const {
        fieldType, label, value,
        onChange, onBlur, onFocus
    } = fieldProps;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
            </label>
            <input
                type={fieldType}
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

const handleSubmit = (data, event) => {
    alert(`Kiedy?  ${data.when} \nDokąd?  ${data.country} \nIle osób?  ${data.people}`);
    event.preventDefault();
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
                            component={Input}/>

                        <Field
                            label="Dokąd?"
                            name="country"
                            component={DropDown}
                            options={countries}/>

                        <Field
                            label="Ile osób?"
                            name="people"
                            component={Input}/>

                        <input className="submitButton mt-4" type="submit" value="Szukaj" />
                        
                    </form>
                )}>
            </Form>
        );
    }
}