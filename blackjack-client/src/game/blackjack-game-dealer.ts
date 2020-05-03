import { BlackjackGameRound } from "./blackjack-game-round";

export interface BlackjackGameDealer {
  roundInProgress: BlackjackGameRound;
  percentRemainingInShoe: number
}
