import { BlackjackGamePlayer } from "./blackjack-game-player";

export interface BlackjackGameRoundPlayer {
  player: BlackjackGamePlayer;
  wager: number;
  hasAction: boolean;
}
