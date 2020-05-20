import React, { Component, ChangeEvent } from 'react';

type State = {
  emailAddress: string,
  password: string
}

 export class Register extends Component<any, State> {
    static displayName = Register.name;
  
    constructor(props: any) {
      super(props);
      this.state = {
        emailAddress: "",
        password: ""
      };

    this.onEmailAddressChange = this.onEmailAddressChange.bind(this);
    this.onPasswordChange = this.onPasswordChange.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
  }

  onEmailAddressChange(event: ChangeEvent<HTMLInputElement>) {
    this.setState({emailAddress: event.target.value});
  }

  onPasswordChange(event: ChangeEvent<HTMLInputElement>) {
    this.setState({password: event.target.value});
  }

  onSubmit(event: ChangeEvent<HTMLFormElement>) {
    console.log(JSON.stringify(this.state));
    event.preventDefault();

    return fetch('api/register',
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
        throw new Error("Registration failed!");
      }
    })
    .then(data => {
      console.log("Registration succeeded");
    })
    .catch(e => console.error("Registration failed: " + e));
  }

  render() {
    return (
      <form onSubmit={this.onSubmit}>
        <h2>Register</h2>
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
