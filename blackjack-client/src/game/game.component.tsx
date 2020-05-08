import React from 'react';
import { Button, TextField, ButtonGroup, Grid, Box } from '@material-ui/core';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';
import { GameService } from './game.service';
import { DealerProps, Dealer } from './dealer.component';
import { PlayerProps } from './player.component';
import * as _ from 'lodash';
import { BlackjackHand } from './blackjack-hand';
import { HandProps } from './hand.component';
import { PlayingCardProps, PlayingCard } from './playingcard.component';
import { Card } from './card';
import { BlackjackGamePlayer } from './blackjack-game-player';
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { WagerOutcome } from './wager-outcome';
import { CardSuit } from './cardsuit';
import { GameHeader } from './game-header.component';
import './game.css';
import { GameControlButtons } from './game-control-buttons.component';

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
  awaitingPlayerActionSince?: Date,
  awaitingNextRoundSince?: Date,
  playerIdsFromMissedRounds: string[];
  playerActionIsExpired: boolean;
  percentRemainingInDealerShoe: number;

  playerId?: string;

  player?: PlayerProps;
}

class Game extends React.Component<RouteComponentProps, GameState> {

  service: GameService = new GameService();

  componentDidMount() {
    const { match: { params } } = this.props;
    const gameId: string = (params as any).gameId;
    this.service.getGame(gameId).subscribe(game => {
      const player = { id: null };
      const playerId: string | null = player?.id;
      // determine current player
      const state: GameState = this.mapGameToState(gameId, playerId, game);
      this.setState(state);
    });
  }
  getTimeSpanToNowInSeconds(date: Date | undefined): number {
    if (!date){
      return -1;
    }
    let timeRemaining = new Date().valueOf() - date.valueOf();
    return Math.abs(Math.ceil(timeRemaining / 1000));
  }
  mapGameToState(gameId: string, playerId: string | null, game: BlackjackGame): GameState {
    const currentPlayer: BlackjackGamePlayer | undefined = _.find(game.players, a => a.id === playerId);
    let currentPlayerName: string = '';
    let currentPlayerHasAction: boolean = false;
    let currentPlayerHasBlackjack: boolean = false;
    let currentPlayerHasTwoCards: boolean = false;
    let currentPlayerBalance: number = 0;
    if (currentPlayer) {
      currentPlayerName = currentPlayer.alias;
      currentPlayerHasAction = currentPlayer.hasAction;
      currentPlayerBalance = currentPlayer.account.balance;
      if (currentPlayer.hand){
        currentPlayerHasBlackjack = currentPlayer.hand.isBlackjack;
        currentPlayerHasTwoCards = currentPlayer.hand.cards.length === 2;
      }
    }
    let endOfRoundTimerIsVisible: boolean = game.isRoundInProgress &&_.every( game.players, a => a.hasAction);
    let secondsAwaitingPlayerAction = this.getTimeSpanToNowInSeconds(game.awaitingPlayerActionSince);
    let state: GameState = {
      id: gameId,
      title: game.name || 'BLACKJACK',
      turnLengthInSeconds: game.turnLengthInSeconds,
      wagerPeriodInSeconds: game.bettingPeriodInSeconds,
      minWager: game.minWager,
      maxWager: game.maxWager,
      dealer: this.getDealerProps(game),
      secondsAwaitingPlayerAction: secondsAwaitingPlayerAction,
      secondsAwaitingWagers: this.getTimeSpanToNowInSeconds(game.awaitingNextRoundSince),
      players: this.getPlayers(game, secondsAwaitingPlayerAction),
      wagerPeriodTimerIsVisible: !game.isRoundInProgress && _.some(game.players, a => a.wager > 0),
      endOfRoundTimerIsVisible: endOfRoundTimerIsVisible,
      percentRemainingInDealerShoe: game.percentRemainingInDealerShoe,
      awaitingPlayerActionSince: game.awaitingPlayerActionSince,
      awaitingNextRoundSince: game.awaitingNextRoundSince,
      playerIdsFromMissedRounds: [],
      playerActionIsExpired: game.playerActionIsExpired,

      playerId: currentPlayer?.id,
      currentPlayerName: currentPlayerName,
      currentPlayerBalance: currentPlayerBalance,
      standButtonIsVisible: currentPlayerHasAction,
      hitButtonisVisible: currentPlayerHasAction && !currentPlayerHasBlackjack,
      doubleDownButtonisVisible: currentPlayerHasAction && currentPlayerHasTwoCards,
      wagerInputIsVisible: !endOfRoundTimerIsVisible && !currentPlayer?.isLive && (currentPlayer?.wager ?? 0) === 0,

      player: !currentPlayer ? undefined : this.getPlayerProps(game, currentPlayer, secondsAwaitingPlayerAction)
    };
    if (game.isRoundInProgress) {
      // reconstructRoundPlayerPlayerFields
      // reconstructPlayerGameFields
    }

    return state;
  }

  getPlayers(game: BlackjackGame, secondsAwaitingPlayerAction: number): PlayerProps[] {
    return game.players.map(p => this.getPlayerProps(game, p, secondsAwaitingPlayerAction));
  }
  getPlayerProps(game: BlackjackGame, player: BlackjackGamePlayer, secondsAwaitingPlayerAction: number): PlayerProps {

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

    const canShowHand: boolean = _.every(game.players, a => !a.hasAction);

    return {
      name: 'Dealer',
      hand: this.getHandProps(game.dealerHand, canShowHand),
      canShowHand: canShowHand,
      percentOfCardsRemainingInSchoe: game.percentRemainingInDealerShoe
    }
  }
  getHandProps(hand?: BlackjackHand, canShowHand?: boolean): HandProps {
    return {
      isBlackjack: hand?.isBlackjack ?? false,
      isBusted: hand?.isBusted ?? false,
      isSoft: hand?.isSoft ?? false,
      score: this.determineHandScore(hand),
      cards: this.getCards(hand, canShowHand)
    };
  }
  getCards(hand?: BlackjackHand, canShowHand?: boolean): PlayingCardProps[] {
    if (!hand) {
      return [];
    }
     return hand.cards.map((c, i) => this.getCardProps(c, i, canShowHand ?? true));
  }
  getCardProps(card: Card, cardNumber: number, canShowHand: boolean): PlayingCardProps {
    if (!card){
      return {
        rank: '-',
        suit: '-',
        suitCode: '-',
        showCard: false,
        isRotated: false
      };
    }
    let rank: string = card.rank.toString();
    let suit: string = '';
    let suitCode: string = '';
    switch (card.suit) {
      case CardSuit.Club:
        suit = 'clubs';
        suitCode = String.fromCharCode(5);
        break;
      case CardSuit.Diamond:
        suit = 'diams';
        suitCode = String.fromCharCode(4);
        break;
      case CardSuit.Heart:
        suit = 'hearts';
        suitCode = String.fromCharCode(3);
        break;
      case CardSuit.Spade:
        suit = 'spades';
        suitCode = String.fromCharCode(6);
        break;
    }
    return {
      rank: rank,
      suit: suit,
      suitCode: suitCode,
      showCard:  cardNumber > 1 || (canShowHand ?? false),
      isRotated: cardNumber % 2 === 0
    };
  }
  determineHandScore(hand?: BlackjackHand): string {
    if (hand?.isBusted ?? false) {
      return 'Busted';
    } else if (hand?.isBlackjack ?? false) {
      return 'Blackjack';
    } else if (hand?.isSoft ?? false) {
      const score1 = hand?.scoreHighLow.item1;
      const score2 = hand?.scoreHighLow.item2;
      let score = score1 ? score1.toString() : '';
      if (score2 && score2 <= 21) {
        score += ' or ' + score2.toString();
      }
      return score;
    }
    return '';
  }

  startRound() {
    if (this.state.percentRemainingInDealerShoe < 20) {
      this.refreshDealerShoe();
    }
    // TODO: don't deal if round is in progress or when no players have wagered
    this.service.deal(this.state.id).subscribe(
      // TODO: remember inactive players LiveBlackjackGame#49
      // remove inactive players LiveBlackjackGame#57-59
    );
  }

  endRound() {
    this.service.endRound(this.state.id).subscribe();
  }

  forceCurrentActionToStand() {
    if (this.state.playerId) {
      this.service.forceStand(this.state.id, this.state.playerId).subscribe();
    }
  }

  refreshDealerShoe() {
    throw new Error("Method not implemented.");
  }

  public render() {
    if (!this.state) {
      return <React.Fragment>One moment</React.Fragment>;
    }

    let wagerPeriodTimer: any;
    if (this.state.wagerPeriodTimerIsVisible) {
      wagerPeriodTimer = <div>The next hand will start in {this.state.wagerPeriodInSeconds - this.state.secondsAwaitingWagers}</div>
    }
    let recentOutcome: any;
    if (this.state.player && this.state.player.recentWagerOutcome) {
      switch (this.state.player.recentWagerOutcome){
        case WagerOutcome.Win.toString():
          recentOutcome = 'You win!';
          break;
        case WagerOutcome.Lose.toString():
          recentOutcome = 'You lose';
          break;
        case WagerOutcome.Draw.toString():
          recentOutcome = 'Draw';
          break;
      }
    }
    let turnNotification: any = this.state.hitButtonisVisible || this.state.standButtonIsVisible || this.state.doubleDownButtonisVisible ? <h3>It's your turn</h3> : undefined;
    let hitButton: any = this.state.hitButtonisVisible ? <Button color="primary">Hit</Button> : undefined;
    let standButton: any = this.state.standButtonIsVisible ? <Button color="primary">Stand</Button> : undefined;
    let doubleDownButton: any = this.state.doubleDownButtonisVisible ? <Button color="primary">Double Down</Button> : undefined;
    let wagerInput: any = this.state.wagerInputIsVisible ? <React.Fragment>
      <h3>Place your bet</h3>
        <b>$</b><input type="number" min={this.state.minWager} max={this.state.maxWager} name="bet" placeholder="5" />
        <Button color="primary" variant="contained">Place Bet</Button>
    </React.Fragment> : undefined;
    let gameControlButtons: any = <React.Fragment>
      <Grid item xs={12} className="game__timer">
        {wagerPeriodTimer}
      </Grid>
      <Grid item xs={12} className="game__notifications">
        {turnNotification}
      </Grid>
      <Grid item xs={12} className="game__buttons">
        <ButtonGroup variant="contained">
          {hitButton}
          {standButton}
          {doubleDownButton}
        </ButtonGroup>
      </Grid>
      <Grid item xs={12} className="game__wager-input">
        {wagerInput}
      </Grid>
    </React.Fragment>;

    return <React.Fragment>
      <Box className="game-container playingCards faceImages">
        <Grid container className="game">

          <GameHeader
            title={this.state.title}
            currentPlayerName={this.state.currentPlayerName}
            currentPlayerBalance={this.state.currentPlayerBalance}
            wagerInputIsVisible={this.state.wagerInputIsVisible}
            minWager={this.state.minWager}
            maxWager={this.state.maxWager}>
          </GameHeader>

          <Grid container>
            <Grid item xs={12} sm={6}>
              <GameControlButtons></GameControlButtons>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Dealer {...this.state.dealer}></Dealer>
            </Grid>
          </Grid>

          <Grid item xs={12} className="game__control-buttons">
            {gameControlButtons}
          </Grid>

        <Grid item xs={12} className="game__player_info">
            <Button variant="contained" color="primary">New game</Button>
        </Grid>
        <Grid item xs={12} className="game__player_info">
          {/* Balance: € {this.state.game.players?.balance} */}
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
      </Box>
    </React.Fragment>
  }
}

export default withRouter(Game)
