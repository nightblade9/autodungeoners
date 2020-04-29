import React, { Component } from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
var jwtDecode = require('jwt-decode');

export class Identity extends Component {
  static displayName = Identity.name;

  constructor(props) {
    super(props);
    this.state = { token: localStorage.getItem("userInfo") };
    // TODO: check token is signed and valid (non-expired)
  }

  render() {
    return (
        <NavLink tag={Link} className="text-dark" to="/">{ this.state.token != null ? "Hi, " + jwtDecode.call("jwt_decode", this.state.token).email : "Unauthenticated" }</NavLink>
    );
  }
}
