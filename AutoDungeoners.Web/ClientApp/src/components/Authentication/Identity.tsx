import React from 'react';
import { Link } from 'react-router-dom';
import JwtBody from '../../interfaces/Jwt';
import jwtDecode from 'jwt-decode';

interface TokenProps {
  token: string;
}

export function Identity(props: TokenProps) {
  return (
    <Link to="/">{props.token != null ? "Hi, " + (jwtDecode(props.token) as JwtBody).email : "Unauthenticated"}</Link>
  )
}
