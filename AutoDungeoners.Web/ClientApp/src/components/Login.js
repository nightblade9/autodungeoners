import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Login extends Component {
  static displayName = Login.name;

  constructor(props) {
    super(props);
    this.state = {
      "emailAddress": "",
      "password": ""
    };

    this.onEmailAddressChange = this.onEmailAddressChange.bind(this);
    this.onPasswordChange = this.onPasswordChange.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
  }

  onEmailAddressChange(event) {
    this.setState({emailAddress: event.target.value});
  }

  onPasswordChange(event) {
    this.setState({password: event.target.value});
  }

  onSubmit(event) {
    console.log(JSON.stringify(this.state));
    event.preventDefault();

    return fetch('api/login',
    {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(this.state)
    })
    .then((response) => {
      if (response.ok)
      {
        return response.json();
      }
      else
      {
        throw new Error("Login failed!");
      }
    })
    .then(data => {
      console.log("Login succeeded");
    })
    .catch(e => console.error("Login failed: " + e));
  }

  render() {
    return (
      <form onSubmit={this.onSubmit}>
        <h2>Login</h2>
        <p>Don't have an account yet? <Link to={`/register`}>Register here</Link>.</p>
        <label>Email Address:
          <input type="text" value={this.state.emailAddress} onChange={this.onEmailAddressChange} />
        </label>

        <label>Password:
          <input type="text" value={this.state.password} onChange={this.onPasswordChange} />
        </label>
        
        
        <input type="submit" value="Submit" />
      </form>
    );
  }
}
