import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from 'react-router-dom';
import './App.css';
import Lobby from './lobby/lobby.component';
import Game from './game/game.component';
import CssBaseline from '@material-ui/core/CssBaseline';
import { Container } from '@material-ui/core';

function App() {

  return (
    <React.Fragment>
      <CssBaseline />
      <Container maxWidth="sm">
        <Router>
          <Switch>
            <Route path="/game/:gameId">
              <Game />
            </Route>
            <Route path="/">
              <Lobby />
            </Route>
          </Switch>
        </Router>
      </Container>
    </React.Fragment>
  );
}

export default App;
