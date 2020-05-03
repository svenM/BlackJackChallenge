import React from 'react';
import { Button, TextField, ButtonGroup, Grid } from '@material-ui/core';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';
import { GameService } from './game.service';
import { AxiosResponse } from 'axios';
import { DealerProps } from './dealer.component';
import { PlayerProps } from './player.component';
import * as _ from 'lodash';

export interface GameState {
  id: string,
  title: string,
  minWager: number,
  maxWager: number,
  turnLengthInSeconds: number,
  wagerPeriodInSeconds: number,
  currentPlayerName: string,
  currentPlayerBalance: string,
  hitButtonisVisible: boolean;
  standButtonIsVisible: boolean;
  doubleDownButtonisVisible: boolean;
  wagerInputIsVisible: boolean;
  wagerPeriodTimerIsVisible: boolean;
  endOfRoundTimerIsVisible: boolean;
  secondsAwaitingPlayerAction: number;
  secondsAwaitingWagers: number,
  dealer: DealerProps,
  players: PlayerProps[],

  showLobbyButton: boolean;
}

class Game extends React.Component<RouteComponentProps, GameState> {

  service: GameService = new GameService();

  componentDidMount() {
    const { match: { params } } = this.props;
    const gameId: string = (params as any).gameId;
    this.service.getGame(gameId).subscribe((response: AxiosResponse<BlackjackGame>) => {
      const game = response.data;
      const state: GameState = {
        id: gameId,
        title: game.name || 'BLACKJACK',
        turnLengthInSeconds: 30,
        wagerPeriodInSeconds: 10,
        minWager: game.minWager,
        maxWager: game.maxWager,
        dealer: {
          name: 'Dealer',
          hand: game.dealerHand ? {
            isBlackjack: game.dealerHand.isBlackjack ?? false,
            isBusted: game.dealerHand.isBusted ?? false,
            isSoft: game.dealerHand.isSoft ?? false,
            score: (game.dealerHand.isBusted ?? false) ? 'Busted' :
                   ( (game.dealerHand.isBlackjack ?? false) ? 'Blackjack' :
                   (game.dealerHand.isSoft ?? false) ?  )
            cards: game.dealerHand.cards.map(c => {

            }),

          } : null,
          canShowHand: _.every(game.players, a => !a.hasAction),
          percentOfCardsRemainingInSchoe: game.percentRemainingInDealerShoe
        }
      };
      this.setState(state);


      // determine current player

    });
  }

  public render() {
    if (!this.state || !this.state.game) {
      return <React.Fragment>One moment</React.Fragment>;
    }
    return <React.Fragment>
      <Grid container direction="column" justify="space-around" alignItems="center" spacing={2} className="game">
        <Grid item xs={12} style={{width: '100%'}} className="game__info">
        Game link: <TextField style={{width: '100%'}} defaultValue={`http://localhost:3000/game/${this.state.game.id}`} InputProps={{readOnly: true}}></TextField>
        </Grid>
      <Grid item xs={12} className="game__player_info">
          { this.state.showLobbyButton ?
            <Button variant="contained" color="secondary">To Lobby</Button>
            : null }
          <Button variant="contained" color="primary">New game</Button>
      </Grid>
      <Grid item xs={12} className="game__player_info">
        {/* Balance: € {this.state.game.players?.balance} */}
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
      </Grid>
    </React.Fragment>
  }
}

export default withRouter(Game)
