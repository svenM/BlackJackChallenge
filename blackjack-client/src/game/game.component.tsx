import React from 'react';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import { BlackjackGame } from './blackjack-game';
import { GameService } from './game.service';
import { DealerProps, Dealer } from './dealer.component';
import { PlayerProps, Player } from './player.component';
import * as _ from 'lodash';
import { BlackjackHand } from './blackjack-hand';
import { HandProps } from './hand.component';
import { PlayingCardProps } from './playingcard.component';
import { Card } from './card';
import { BlackjackGamePlayer } from './blackjack-game-player';
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { CardSuit } from './cardsuit';
import { GameHeader } from './game-header.component';
import './game.css';
import { GameControlButtons } from './game-control-buttons.component';
import { EmptySeat } from './empty-seat.component';
import { Box, Grid } from '@material-ui/core';

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
      const player = { id: undefined };
      const playerId: string | undefined = player?.id;
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
  mapGameToState(gameId: string, playerId: string | undefined, game: BlackjackGame): GameState {
    let endOfRoundTimerIsVisible: boolean = game.isRoundInProgress &&_.every( game.players, a => a.hasAction);
    let secondsAwaitingPlayerAction = this.getTimeSpanToNowInSeconds(game.awaitingPlayerActionSince);
    const currentPlayer: BlackjackGamePlayer | undefined = _.find(game.players, a => a.id === playerId);
    let currentPlayerName: string = '';
    let currentPlayerHasAction: boolean = false;
    let currentPlayerHasBlackjack: boolean = false;
    let currentPlayerHasTwoCards: boolean = false;
    let currentPlayerBalance: number = 0;
    let currentPlayerIsLive: boolean = false;
    let wagerInputIsVisible: boolean = false;
    if (currentPlayer) {
      currentPlayerName = currentPlayer.alias;
      currentPlayerHasAction = currentPlayer.hasAction;
      currentPlayerBalance = currentPlayer.account.balance;
      currentPlayerIsLive = currentPlayer.isLive;
      wagerInputIsVisible = !endOfRoundTimerIsVisible && !currentPlayerIsLive && (currentPlayer?.wager ?? 0) === 0;
      if (currentPlayer.hand){
        currentPlayerHasBlackjack = currentPlayer.hand.isBlackjack;
        currentPlayerHasTwoCards = currentPlayer.hand.cards.length === 2;
      }
    }
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
      wagerInputIsVisible: wagerInputIsVisible,

      player: !currentPlayer ? undefined : this.getPlayerProps(game, currentPlayer, secondsAwaitingPlayerAction)
    };
    if (game.isRoundInProgress) {
      // reconstructRoundPlayerPlayerFields
      // reconstructPlayerGameFields
    }
    console.log(state);
    return state;
  }

  getPlayers(game: BlackjackGame, secondsAwaitingPlayerAction: number): PlayerProps[] {
    return _.orderBy(game.players, x => x.position)
            .map(p => this.getPlayerProps(game, p, secondsAwaitingPlayerAction));
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
    this.service.deal(this.state.id).subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
  }

  endRound() {
    this.service.endRound(this.state.id).subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
  }

  forceCurrentActionToStand() {
    if (this.state.playerId) {
      this.service.forceStand(this.state.id, this.state.playerId).subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
    }
  }

  onHitClick() {
    if (this.state.playerId) {
      this.service.playerActionRequest(this.state.id, this.state.playerId, 'hit')
        .subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
    }
  }
  onStandClick() {
    if (this.state.playerId) {
      this.service.playerActionRequest(this.state.id, this.state.playerId, 'stand')
        .subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
    }
  }
  onDoubleDownClick() {
    if (this.state.playerId) {
      this.service.playerActionRequest(this.state.id, this.state.playerId, 'doubledown')
        .subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
    }
  }
  onPlacebetClick(betAmount: number) {
    if (this.state && this.state.playerId) {
      this.service.placeBet(this.state.id, this.state.playerId, betAmount)
        .subscribe(() => this.refreshGame(this.state.id, this.state.playerId));
    }
  }

  onPlayerJoin(seatNo: number, playerName: string) {
    if (seatNo && playerName) {
      this.service.joinGame(this.state.id, playerName, seatNo)
        .subscribe(p => this.refreshGame(this.state.id, p.id));
    }
  }

  getSeats() {
    const seats: any[] = [];
    for (let seatNo = 6; seatNo > 0; seatNo--) {
      const player: PlayerProps | undefined = _.find(this.state.players, x => x.position === seatNo);
      const canJoin: boolean = _.every(this.state.players, x => x.id !== this.state.playerId);
      console.log(seatNo, player, canJoin);
      if (player) {
        return <Player {...player}></Player>;
      } else {
        return <EmptySeat seatNo={seatNo} canJoin={canJoin} onJoin={this.onPlayerJoin.bind(this)}></EmptySeat>
      }
    }
    return seats;
  }

  refreshGame(gameId: string, playerId: string | undefined) {
    this.service.getGame(gameId).subscribe(game => {
      // determine current player
      const state: GameState = this.mapGameToState(gameId, playerId, game);
      this.setState(state);
    });
  }

  public render() {
    if (!this.state) {
      return <React.Fragment>One moment</React.Fragment>;
    }

    // TODO: number of players hardcoded?
    const seats = this.getSeats();

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
              <GameControlButtons
                wagerPeriodTimerIsVisible={this.state.wagerInputIsVisible}
                wagerPeriodInSeconds={this.state.wagerPeriodInSeconds}
                secondsAwaitingWagers={this.state.secondsAwaitingWagers}
                player={this.state.player}
                hitButtonisVisible={this.state.hitButtonisVisible}
                standButtonIsVisible={this.state.standButtonIsVisible}
                doubleDownButtonisVisible={this.state.doubleDownButtonisVisible}
                wagerInputIsVisible={this.state.wagerInputIsVisible}
                minWager={this.state.minWager}
                maxWager={this.state.maxWager}
                onHitClick={this.onHitClick.bind(this)}
                onStandClick={this.onStandClick.bind(this)}
                onDoubleDownClick={this.onDoubleDownClick.bind(this)}
                onPlacebetClick={this.onPlacebetClick.bind(this)}>
              </GameControlButtons>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Dealer {...this.state.dealer}></Dealer>
            </Grid>
          </Grid>

          {this.state.endOfRoundTimerIsVisible ? <div className="progressBar"></div> : ''}

          <br />

          <Grid item xs={12}>
            {seats}
          </Grid>

        </Grid>
      </Box>
    </React.Fragment>
  }
}

export default withRouter(Game)
