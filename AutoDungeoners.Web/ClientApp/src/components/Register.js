import React, { Component } from 'react';

export class Register extends Component {
  static displayName = Register.name;

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
  }

  render() {
    return (
      <form onSubmit={this.onSubmit}>
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
