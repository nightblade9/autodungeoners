import React, { Props } from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import jwtDecode from 'jwt-decode';

export function Identity(props) {
  return (
    <NavLink tag={Link} className="text-dark" to="/">{ props.token != null ? "Hi, " + jwtDecode(props.token).email : "Unauthenticated" }</NavLink>
  )
}
