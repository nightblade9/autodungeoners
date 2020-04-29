import React, { Component } from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

export class Identity extends Component {
  static displayName = Identity.name;

  constructor(props) {
    super(props);
    this.state = { token: localStorage.getItem("userInfo") };
  }

  render() {
    return (
        <NavLink tag={Link} className="text-dark" to="/">{ this.state.token == null ? "Unauthenticated" : "Logged in" }</NavLink>
    );
  }
}
