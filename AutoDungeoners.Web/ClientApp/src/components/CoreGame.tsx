import React, { Component } from 'react';
import { RequireAuthentication } from './Authentication/RequireAuthentication';
import { GameUiUpdater } from '../components/GameUiUpdater'
import { IUser } from '../interfaces/IUser';
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

    var userName = "";
    var token = localStorage.getItem("userInfo");
    if (token !=  null)
    {
      var decoded = jwtDecode(token);
      userName = decoded.email;;
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

  renderUserStats(user) {
    return (
      <div>
        <Timer intervalSeconds={60} callback={() => this.refreshState(user)} />
        <GameUiUpdater user={user} onUpdate={() => this.forceUpdate()} />
        <ul>
          <li><strong>Gold: </strong> {user.gold}</li>
        </ul>
      </div>
    );
  }

  refreshState(user) {
    this.setState({user: user});
  }

  async fetchUser() {
    const headers:Record<string, string> = {
      "Bearer": localStorage.getItem("userInfo")
    };

    const response = await fetch('api/User', {
      headers: headers
    });

    const data = await response.json();
    this.setState({ user: data, isLoading: false });
  }
}

export { CoreGame };