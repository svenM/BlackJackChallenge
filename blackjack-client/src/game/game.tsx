import React from 'react';
import { Button, TextField, ButtonGroup, Grid } from '@material-ui/core';
import { Router } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';

interface GameProps {}

interface GameState {
  game: BlackjackGame;
}

export default class Game extends React.Component<GameProps, GameState> {

  constructor (props: GameProps) {
    super(props);

    // state ophalen
  }

  public render() {
      return <React.Fragment>
        {/* <Grid container direction="row" justify="space-around" alignItems="center" spacing={5} className="game">
          <Grid item xs={12} className="game__info">
            Game id: {gameId} <Button color="primary">New game</Button>
          </Grid>
        <Grid item xs={12} className="game__player_info">
          Balance: € {player1?.balance}
        </Grid>
        <Grid item xs={12} className="game__place_bet">
          <form>
            <TextField type="number" name="bet" placeholder="5"></TextField>
            <Button color="primary" variant="contained">Place Bet</Button>
          </form>
        </Grid>
        <Grid item xs={12} className="game__buttons">
          <ButtonGroup variant="contained">
            <Button color="primary">Hit</Button>
            <Button color="secondary">Stand</Button>
          </ButtonGroup>
        </Grid>
        <Grid item xs={12} className="game__end">
          <Button color="primary" variant="contained">Continue</Button>
        </Grid>
        <Grid item xs={12} className="game__player_hand">
          Your Hand
        </Grid>
        <Grid item xs={12} className="game__dealer_hand">
          Dealer's Hand
        </Grid>
        <Grid item xs={12} className="game__messages">

        </Grid>
        </Grid> */}
      </React.Fragment>
  }
}
