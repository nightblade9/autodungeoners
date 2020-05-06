import React, { Component } from 'react';
import  { Redirect } from 'react-router-dom';

export class RequireAuthentication extends Component {
    static displayName = RequireAuthentication.name;
    render() {
        if (localStorage.getItem("userInfo") == null)
        {
            return  <Redirect  to="/login" />
        }
        return null;
    }
}