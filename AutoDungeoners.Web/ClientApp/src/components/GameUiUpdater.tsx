import React, { Component } from 'react';
import { IUser } from '../interfaces/IUser';
import Timer from "../functions/Timer";

interface IProps {
    user?: IUser,
    onUpdate: any
}

// Reuse props as state since they're equivalent
class GameUiUpdater extends Component<IProps, IProps> {
    static displayName = GameUiUpdater.name;

    constructor(props:IProps) {
        super(props);
        this.state = { user: props.user, onUpdate: props.onUpdate };
    }

    async onTick() {
        this.state.user.gold += 1;
        this.setState({user: this.state.user});
        // Force parent to re-render
        this.state.onUpdate();
    }

    render() {
        return (
            <Timer intervalSeconds={1} callback={() => this.onTick()} />
        );
    }
}

export { GameUiUpdater };