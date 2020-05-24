import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { AppBar, Toolbar, IconButton, Typography, Button } from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu'
import { Link, useHistory } from 'react-router-dom';
import { Identity } from './Authentication/Identity';
import { inherits } from 'util';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
  navLink: {
    cursor: "pointer",
    color: "inherit",
    textDecoration: "inherit"
  }
}));

const NavButton = (props: { to: string, children: React.ReactNode | React.ReactNode[] }) => {
  return (
    <Button color="inherit" component={Link} to={props.to} >
      {props.children}
    </Button>
  )
}


export function NavMenu() {
  const classes = useStyles();
  const history = useHistory();

  return (
    <header>
      <AppBar position="static">
        <Toolbar>
          {/* Menu Icon */}
          <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
            <MenuIcon />
          </IconButton>

          {/* Title */}
          <Typography variant="h6" className={classes.title}>
            <Link to="/" className={classes.navLink}>
              Auto Dungeoners
            </Link>
          </Typography>

          {/* user info */}
          <Typography variant="h6">
            <Identity token={localStorage.getItem("userInfo")!} />
          </Typography>

          {/* all the links */}
          <NavButton to="/" >Home</NavButton>
          <NavButton to="/counter">Counter</NavButton>
          <NavButton to="/fetch-data">Fetch data</NavButton>
          <NavButton to="/login">Login</NavButton>
        </Toolbar>
      </AppBar>
    </header>
  )
}
