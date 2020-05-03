import { Dealer } from "./dealer";
import { Player } from "./player";

export interface BlackjackGame {
  gameId: string,
  gameName: string,
  minWager: number,
  maxWager: number,
  turnLengthInSeconds: number,
  WagerPeriodInSeconds: number,
  currentPlayerName: string,
  currentPlayerBalance: number,
  hitButtonIsVisible: boolean,
  standButtonIsVisible: boolean,
  doubleDownButtonIsVisible: boolean,
  wagerInputIsVisible: boolean,
  wagerPeriodTimerIsVisible: boolean,
  endOfRoundTimerIsVisible: boolean,
  secondsAwaitingPlayerAction: number,
  secondsAwaitingWagers: number,
  dealer: Dealer,
  players: Player[]
}
