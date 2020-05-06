import React, { Component } from 'react';
import  { Redirect } from 'react-router-dom';
var jwtDecode = require('jwt-decode');

export class CoreGame extends Component {
  static displayName = CoreGame.name;

  constructor(props) {
    super(props);
    this.state = {};

    var token = localStorage.getItem("userInfo");
    console.log("T=" + token);
    if (token !=  null)
    {
      var decoded = jwtDecode.call("jwt_decode", token);
      this.state.userName = decoded.email;
    }
  }

  render() {
    if (localStorage.getItem("userInfo") == null)
    {
      return  <Redirect  to="/login" />
    }

    return (
      <div>
        <h1>{this.state.userName}</h1>
      </div>
    );
  }
}
