import React, { ChangeEvent, useState } from 'react';
import { Link as RouterLink, RouteComponentProps, useHistory } from 'react-router-dom';
import { Container, CssBaseline, Avatar, Typography, TextField, FormControlLabel, Checkbox, Button, Grid, makeStyles, Link } from '@material-ui/core';
import { LockOutlined as LockOutlinedIcon } from '@material-ui/icons'

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: '100%', // Fix IE 11 issue.
    marginTop: theme.spacing(1),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
}));

export function Login() {
  const classes = useStyles();

  // use state hooks
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  // for routing
  const history = useHistory();

  const onSumbit = (event: ChangeEvent<HTMLFormElement>) => {
    let state = { emailAddress: email, password: password };

    console.log(JSON.stringify(state));

    event.preventDefault();

    return fetch('api/login',
      {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(state)
      })
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        else {
          throw new Error("Login failed!");
        }
      })
      .then(data => {
        console.log("Login succeeded: " + JSON.stringify(data));
        var token = data.token;
        localStorage.setItem("userInfo", token);
        history.push("/");
      })
      .catch(e => console.error("Login failed: " + e));
  }

  return (
    <Container component="main" maxWidth="xs">
      <div className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign in
      </Typography>
        <form className={classes.form} noValidate onSubmit={onSumbit}>
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email Address"
            name="email"
            autoComplete="email"
            autoFocus
            onChange={e => setEmail(e.target.value)}
          />
          <TextField
            variant="outlined"
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            id="password"
            autoComplete="current-password"
            onChange={e => setPassword(e.target.value)}
          />
          <FormControlLabel
            control={<Checkbox value="remember" color="primary" />}
            label="Remember me"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            className={classes.submit}
          >
            Sign In
        </Button>
          <Grid container>
            <Grid item xs>
              <Link variant="body2">
                Forgot password?
            </Link>
            </Grid>
            <Grid item>
              <Link to="/register" variant="body2" component={RouterLink}>
                Don't have an account? Sign Up
            </Link>
            </Grid>
          </Grid>
        </form>
      </div>
    </Container>
  );
}