import React, { Component } from "react";
import { Form, Field } from "@progress/kendo-react-form";
import countries from "./countries";


const Input = (fieldProps) => {
    const {
        fieldType, label, value, visited, touched, valid,
        onChange, onBlur, onFocus, validationMessage,
    } = fieldProps;
    const invalid = !valid && visited;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
                <input
                    type={fieldType}
                    className={invalid ? "invalid" : ""}
                    value={value}
                    onChange={onChange} />
            </label>
            { invalid &&
            (<div className="required">{validationMessage}</div>) }
        </div>
    );
};

const DropDown = ({ label, value, valid, visited, options,
                      onChange, onBlur, onFocus, validationMessage, }) => {
    const invalid = !valid && visited;
    return (
        <div onBlur={onBlur} onFocus={onFocus}>
            <label>
                { label }
                <select
                    className={invalid ? "invalid" : ""}
                    value={value}
                    onChange={onChange}>
                    {options.map(option => (
                        <option key={option}>{option}</option>
                    ))}
                </select>
            </label>
        </div>
    )
}

const handleSubmit = (data, event) => {
    alert('Wybrano kraj: : ' + data.country);
    event.preventDefault();
}


export class NewSearchForm extends Component {

    render() {
        return (
            <Form
                onSubmit={handleSubmit}
                initialValues={{
                    email: "", password: "", country: "", acceptedTerms: false
                }}
                render={(formRenderProps) => (
                    <form onSubmit={formRenderProps.onSubmit}>
                        <h5>Znajdź wakacje marzeń!</h5>

                        <Field
                            label="Email:"
                            name="email"
                            fieldType="email"
                            component={Input}/>

                        <Field
                            label="Dokąd?"
                            name="country"
                            component={DropDown}
                            options={countries}/>

                        <input className="submitButton mt-4" type="submit" value="Szukaj" />
                        
                    </form>
                )}>
            </Form>
        );
    }
}