import React, { Component } from 'react';
import { Button, Typography } from '@material-ui/core';

type State = {
  currentCount: number
}

export class Counter extends Component<any, State> {
  static displayName = Counter.name;

  constructor(props: any) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <div>
        <Typography variant="h2" paragraph={true}>Counter</Typography>
        <Typography variant="subtitle1" paragraph={true}>This is a simple example of a React component.</Typography>

        <Typography paragraph={true} >Current count: <strong>{this.state.currentCount}</strong></Typography>

        <Button variant="contained" color="primary" onClick={this.incrementCounter}>Increment</Button>
      </div>
    );
  }
}
