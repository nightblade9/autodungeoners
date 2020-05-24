import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import { CssBaseline, Container } from '@material-ui/core';


export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <NavMenu />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
