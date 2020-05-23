import React from 'react';
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import JwtBody from '../../interfaces/Jwt';
import jwtDecode from 'jwt-decode';

interface TokenProps {
  token:string;
}

export function Identity(props:TokenProps) {
  return (
    <NavLink tag={Link} className="text-dark" to="/">{ props.token != null ? "Hi, " + (jwtDecode(props.token) as JwtBody).email : "Unauthenticated" }</NavLink>
  )
}