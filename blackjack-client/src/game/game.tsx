import React from 'react';
import { Button, TextField, ButtonGroup, Grid } from '@material-ui/core';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';
import { GameService } from './game.service';
import { AxiosResponse } from 'axios';

interface GameState {
  game: BlackjackGame;
}

class Game extends React.Component<RouteComponentProps, GameState> {

  service: GameService = new GameService();

  componentDidMount() {
    const { match: { params } } = this.props;
    this.service.getGame((params as any).gameId).subscribe((response: AxiosResponse<BlackjackGame>) => {
      const game = response.data;
      this.setState({game});
      console.log(this.state.game);
    });
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

export default withRouter(Game)
