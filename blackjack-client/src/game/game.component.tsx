import React, { useImperativeHandle } from 'react';
import { Button, TextField, ButtonGroup, Grid } from '@material-ui/core';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';
import { GameService } from './game.service';
import { AxiosResponse } from 'axios';
import { DealerProps } from './dealer.component';
import { PlayerProps } from './player.component';
import * as _ from 'lodash';
import { CardRank } from './cardrank';
import { BlackjackHand } from './blackjack-hand';
import { HandProps } from './hand.component';
import { CardProps } from './card.component';
import { Card } from './card';
import { CardImages } from './card-images';
import { BlackjackGamePlayer } from './blackjack-game-player';
import { PlayerAccount } from './player-account';
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { WagerOutcome } from "./wager-outcome";

export interface GameState {
  id: string;
  title: string;
  minWager: number;
  maxWager: number;
  turnLengthInSeconds: number;
  wagerPeriodInSeconds: number;
  currentPlayerName: string;
  currentPlayerBalance: number;
  hitButtonisVisible: boolean;
  standButtonIsVisible: boolean;
  doubleDownButtonisVisible: boolean;
  wagerInputIsVisible: boolean;
  wagerPeriodTimerIsVisible: boolean;
  endOfRoundTimerIsVisible: boolean;
  secondsAwaitingPlayerAction: number;
  secondsAwaitingWagers: number;
  dealer: DealerProps;
  players: PlayerProps[];

  percenRemainingInDealerShoe: number;
  lobbyButtonVisible: boolean;
}

class Game extends React.Component<RouteComponentProps, GameState> {

  service: GameService = new GameService();

  componentDidMount() {
    const { match: { params } } = this.props;
    const gameId: string = (params as any).gameId;
    this.service.getGame(gameId).subscribe((response: AxiosResponse<BlackjackGame>) => {
      const game = response.data;
      const player = { id: null };
      const playerId: string | null = player?.id;
      // determine current player
      const state: GameState = this.mapGameToState(gameId, playerId, game);
      this.setState(state);
    });
  }
  getTimeSpanToNowInSeconds(date: Date): number {
    throw new Error("Method not implemented.");
  }
  mapGameToState(gameId: string, playerId: string | null, game: BlackjackGame): GameState {
    const currentPlayer: BlackjackGamePlayer | undefined = _.find(game.players, a => a.id === playerId);
    let currentPlayerName: string = '';
    let currentPlayerHasAction: boolean = false;
    let currentPlayerHasBlackjack: boolean = false;
    let currentPlayerHasTwoCards: boolean = false;
    if (currentPlayer) {
      currentPlayerName = currentPlayer.alias;
      currentPlayerHasAction = currentPlayer.hasAction;
      currentPlayerHasBlackjack = currentPlayer.hand?.isBlackjack;
      currentPlayerHasTwoCards = currentPlayer?.hand.cards.length === 2;
    }
    let endOfRoundTimerIsVisible: boolean = game.isRoundInProgress &&_.every( game.players, a => a.hasAction);
    let secondsAwaitingPlayerAction: number = this.getTimeSpanToNowInSeconds(new Date());// TODO

    return {
      id: gameId,
      title: game.name || 'BLACKJACK',
      turnLengthInSeconds: 30,
      wagerPeriodInSeconds: 10,
      minWager: game.minWager,
      maxWager: game.maxWager,
      dealer: this.getDealerProps(game),
      secondsAwaitingPlayerAction: secondsAwaitingPlayerAction,
      secondsAwaitingWagers: this.getTimeSpanToNowInSeconds(new Date()),// TODO
      players: this.getPlayers(game, secondsAwaitingPlayerAction),
      wagerPeriodTimerIsVisible: !game.isRoundInProgress && _.some(game.players, a => a.wager > 0),
      endOfRoundTimerIsVisible: endOfRoundTimerIsVisible,
      currentPlayerName: currentPlayerName,
      currentPlayerBalance: this.getPlayerBalance(currentPlayer),
      standButtonIsVisible: currentPlayerHasAction,
      hitButtonisVisible: currentPlayerHasAction && !currentPlayerHasBlackjack,
      doubleDownButtonisVisible: currentPlayerHasAction && currentPlayerHasTwoCards,
      wagerInputIsVisible: !endOfRoundTimerIsVisible && !currentPlayer?.isLive && currentPlayer?.wager === 0,
      percenRemainingInDealerShoe: 100,
      lobbyButtonVisible: true
    };
  }
  getPlayerBalance(currentPlayer: BlackjackGamePlayer | undefined): number {
    if (!currentPlayer || !currentPlayer.account) {
      return 0;
    }
    return currentPlayer.account.balance;
  }
  getPlayers(game: BlackjackGame, secondsAwaitingPlayerAction: number): PlayerProps[] {
    return game.players.map(p => this.getPlayerProps(game, p, secondsAwaitingPlayerAction));
  }
  getPlayerProps(game: BlackjackGame, player: BlackjackGamePlayer, secondsAwaitingPlayerAction: number): PlayerProps {
    if (!game) {
      throw new Error('Game not defined');
    }
    if (!player){
      throw new Error('Player not defined');
    }
    const settlement: BlackjackHandSettlement | undefined = _.find(game.roundInProgressSettlements, a => a.playerPosition === player.position);
    const secondsAwaitingAction: number = player.hasAction ? secondsAwaitingPlayerAction : -1;
    const result: PlayerProps = {
      id: player.id,
      name: player.alias,
      balance: player.account.balance,
      isLive: player.isLive,
      hasAction: player.hasAction,
      position: player.position,
      secondsAwaitingAction: secondsAwaitingAction,
      hand: !settlement ? this.getHandProps(player.hand) : this.getHandProps(settlement.playerHand),
      wager: !settlement ? player.wager : settlement.wagerAmount,
      recentWagerOutcome: !settlement ? '' : settlement.wagerOutcome.toString()
    };

    return result
  }
  getDealerProps(game: BlackjackGame): DealerProps {
    if (!game) {
      throw new Error('Game not defined');
    }
    return {
      name: 'Dealer',
      hand: this.getHandProps(game.dealerHand),
      canShowHand: _.every(game.players, a => !a.hasAction),
      percentOfCardsRemainingInSchoe: game.percentRemainingInDealerShoe
    }
  }
  getHandProps(hand: BlackjackHand): HandProps {
    if (!hand) {
      throw new Error('Hand not defined');
    }
    return {
      isBlackjack: hand.isBlackjack ?? false,
      isBusted: hand.isBusted ?? false,
      isSoft: hand.isSoft ?? false,
      score: this.determineHandScore(hand),
      cards: this.getCards(hand),
    };
  }
  getCards(hand: BlackjackHand): CardProps[] {
    if (!hand || !hand.cards) {
      return [];
    }
     return hand.cards.map(c => this.getCardProps(c));
  }
  getCardProps(card: Card): CardProps {
    if (!card) {
      throw new Error('Card not defined');
    }
    return {
      rank: card.rank.toString(),
      suit: card.suit.toString(),
      imagePath: CardImages.getCardImagePath(card.suit, card.rank)
    };
  }
  determineHandScore(hand: BlackjackHand): string {
    if (!hand) {
      throw new Error('Hand not defined');
    }
    if (hand.isBusted) {
      return 'Busted';
    } else if (hand.isBlackjack) {
      return 'Blackjack';
    } else if (hand.isSoft) {
      let score = hand.scoreHighLow.item1.toString();
      if (hand.scoreHighLow.item2 <= 21) {
        score += ' or ' + hand.scoreHighLow.item2.toString();
      }
      return score;
    }
    return '';
  }

  startRound() {
    if (this.state.percenRemainingInDealerShoe < 20) {
      this.refreshDealerShoe();
    }

    this.service.deal(this.state.id).subscribe(
      // TODO: remember inactive players LiveBlackjackGame#49
      // remove inactive players LiveBlackjackGame#57-59
    );
  }

  endRound() {
    this.service.endRound(this.state.id).subscribe(
      // TODO: update timings LiveBlackjackGame#65-66
    );
  }

  forceCurrentActionToStand() {
    // TODO keep player id and pass to service
    //this.service.forceStand(this.state.id)
  }

  refreshDealerShoe() {
    throw new Error("Method not implemented.");
  }

  public render() {
    if (!this.state) {
      return <React.Fragment>One moment</React.Fragment>;
    }
    return <React.Fragment>
      <Grid container direction="column" justify="space-around" alignItems="center" spacing={2} className="game">
        <Grid item xs={12} style={{width: '100%'}} className="game__info">
        Game link: <TextField style={{width: '100%'}} defaultValue={`http://localhost:3000/game/${this.state.id}`} InputProps={{readOnly: true}}></TextField>
        </Grid>
      <Grid item xs={12} className="game__player_info">
          { this.state.lobbyButtonVisible ?
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
