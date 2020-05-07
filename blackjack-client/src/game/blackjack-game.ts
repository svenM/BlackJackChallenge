import { BlackjackGamePlayer } from "./blackjack-game-player";
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { BlackjackHand } from "./blackjack-hand";
import { BlackjackGameDealer } from "./blackjack-game-dealer";

export interface BlackjackGame {
  id: string,
  name: string;
  title: string;
  turnLengthInSeconds: number;
  bettingPeriodInSeconds: number;
  awaitingPlayerActionSince?: Date;
  awaitingNextRoundSince?: Date;
  playerActionIsExpired: boolean;
  dealerHand?: BlackjackHand;
  dealerHas21: boolean;
  players: BlackjackGamePlayer[];
  dealer: BlackjackGameDealer;
  maxPlayers: number;
  minWager: number;
  maxWager: number;
  isRoundInProgress: boolean;
  percentRemainingInDealerShoe: number;
  roundInProgressSettlements: BlackjackHandSettlement[];
}
