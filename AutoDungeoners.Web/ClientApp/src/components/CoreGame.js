import React, { Component } from 'react';
import { RequireAuthentication } from './Authentication/RequireAuthentication';
var jwtDecode = require('jwt-decode');

export class CoreGame extends Component {
  static displayName = CoreGame.name;

  constructor(props) {
    super(props);
    this.state = { user: null, isLoading: true };

    var token = localStorage.getItem("userInfo");
    if (token !=  null)
    {
      var decoded = jwtDecode(token);
      this.state.userName = decoded.email;
    }
  }

  componentDidMount() {
    this.fetchUser();
  }

  render() {   
    let contents = this.state.isLoading
      ? <p><em>Loading...</em></p>
      : CoreGame.renderUserStats(this.state.user);

    return (
      <div>
        <RequireAuthentication />
        <h1>{this.state.userName} the Dungeoner</h1>
        {contents}
      </div>
    );
  }

  static renderUserStats(user)
  {
    return (
      <ul>
        <li><strong>Gold: </strong> {user.gold}</li>
      </ul>
    );
  }

  async fetchUser() {
    const response = await fetch('api/User', {
      headers: {
        "Bearer": localStorage.getItem("userInfo")
      }
    });
    const data = await response.json();
    this.setState({ user: data.currentUser, isLoading: false });
  }
}
