import React, { Component } from 'react';
import { RequireAuthentication } from './Authentication/RequireAuthentication';
import { IUser } from '../interfaces/IUser';
import jwtDecode from 'jwt-decode';


interface IProps {
  token: string,
}

interface IState {
  user?: IUser,
  isLoading: boolean,
  userName: string,
}

class CoreGame extends Component<IProps, IState> {
  static displayName = CoreGame.name;

  constructor(props:IProps) {
    super(props);
    this.state = { user: undefined, userName: "", isLoading: true };

    var token = localStorage.getItem("userInfo");
    if (token !=  null)
    {
      var decoded = jwtDecode(token);
      this.setState({ userName: decoded.email });
    }
  }

  componentDidMount() {
    this.fetchUser();
  }

  render() {   
    let contents = this.state.isLoading
      ? <p><em>Loading...</em></p>
      : CoreGame.renderUserStats(this.state.user);

    return (
      <div>
        <RequireAuthentication />
        <h1>{this.state.userName} the Dungeoner</h1>
        {contents}
      </div>
    );
  }

  static renderUserStats(user)
  {
    return (
      <ul>
        <li><strong>Gold: </strong> {user.gold}</li>
      </ul>
    );
  }

  async fetchUser() {
    const headers:Record<string, string> = {
      "Bearer": localStorage.getItem("userInfo")
    };

    const response = await fetch('api/User', {
      headers: headers
    });

    const data = await response.json();
    this.setState({ user: data.currentUser, isLoading: false });
  }
}

export { CoreGame }