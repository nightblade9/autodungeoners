import React, { Component } from 'react';
import { RequireAuthentication } from './Authentication/RequireAuthentication';
var jwtDecode = require('jwt-decode');

export class CoreGame extends Component {
  static displayName = CoreGame.name;

  constructor(props) {
    super(props);
    this.state = {};

    var token = localStorage.getItem("userInfo");
    if (token !=  null)
    {
      var decoded = jwtDecode(token);
      this.state.userName = decoded.email;
    }
  }

  render() {   
    return (
      <div>
        <RequireAuthentication />
        <h1>{this.state.userName} the Dungeoner</h1>
      </div>
    );
  }
}
