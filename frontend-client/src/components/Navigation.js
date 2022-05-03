import React,{Component} from 'react';
import {Navbar,Nav} from 'react-bootstrap';
import {NavLink} from 'react-router-dom';
import { ReactComponent as Logo } from '../icons/logo.svg';

export class Navigation extends Component{

    constructor(props) {
        super(props)
        this.state = {
            user: {}
        }
    }

    componentWillMount() {
        const key = Object.keys(localStorage)
        const user = localStorage.getItem(key)
        const isLogged = false
        if(user === null){
            this.isLogged = false
        }
        else{
            this.isLogged = true
            this.state.user = user
        }
    }
    
    render(){
        return(
            <Navbar bg="light" expand="lg" fixed={"top"}>
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="m-auto display-7">
                        <Logo width={64} height={64}/>
                        <NavLink className="d-inline p-3 text-primary text-decoration-none" to="/">
                            Strona główna
                        </NavLink>
                        <NavLink className="d-inline p-3 text-primary text-decoration-none" to="/destinations">
                            Nasze kierunki
                        </NavLink>
                    </Nav>
                    <Nav>
                        <NavLink className={(this.isLogged ? 'd-none d-inline p-3 text-primary text-decoration-none' : 'd-inline p-3 text-primary text-decoration-none')} to="/login">
                            Zaloguj
                        </NavLink>
                        <NavLink className={(this.isLogged ? 'd-inline p-3 text-primary text-decoration-none': 'd-none  d-inline p-3 text-primary text-decoration-none')} to="/logout">
                            Wyloguj
                        </NavLink>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        )
    }
}