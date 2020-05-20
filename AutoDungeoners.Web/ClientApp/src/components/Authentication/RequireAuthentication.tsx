import React, { Component } from 'react';
import  { Redirect } from 'react-router-dom';

/// Annotate this in your render views to force redirecting to login if the user is not authenticated
export class RequireAuthentication extends Component {
    static displayName:string = RequireAuthentication.name;
    render = () => {
        if (localStorage.getItem("userInfo") == null)
        {
            return  <Redirect  to="/login" />
        }
        return null;
    }
}