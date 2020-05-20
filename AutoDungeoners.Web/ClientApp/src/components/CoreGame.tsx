import React, { Component } from 'react';
import { RequireAuthentication } from './Authentication/RequireAuthentication';
import { GameUiUpdater } from '../components/GameUiUpdater'
import { IUser } from '../interfaces/IUser';
import JwtBody from '../interfaces/Jwt';
import Timer from "../functions/Timer";

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

    let userName = "";
    let token = localStorage.getItem("userInfo");
    if (token !=  null)
    {
      let decoded : JwtBody = jwtDecode(token);
      userName = decoded.email;
    }

    this.state = { user: undefined, userName: userName, isLoading: true };
  }

  componentDidMount() {
    this.fetchUser();
  }

  render() {   
    let contents = this.state.isLoading
      ? <p><em>Loading...</em></p>
      : this.renderUserStats(this.state.user);

    return (
      <div>
        <RequireAuthentication />
        <h1>{this.state.userName} the Dungeoner</h1>
        {contents}
        
      </div>
    );
  }

  renderUserStats(user? : IUser) {
    return (
      <div>
        <Timer intervalSeconds={60} callback={() => this.refreshState(user)} />
        <GameUiUpdater user={user!} onUpdate={() => this.forceUpdate()} />
        <ul>
          <li><strong>Gold: </strong> {user?.gold ?? 0}</li>
        </ul>
      </div>
    );
  }

  refreshState(user?: IUser) {
    this.setState({user: user});
  }

  async fetchUser() {
    const headers:Record<string, string> = {
      "Bearer" : localStorage.getItem("userInfo")!
    };

    const response = await fetch('api/User', {
      headers: headers
    });

    const data = await response.json();
    this.setState({ user: data, isLoading: false });
  }
}

export { CoreGame };