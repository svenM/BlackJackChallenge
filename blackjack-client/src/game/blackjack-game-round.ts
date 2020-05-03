import { BlackjackGameRoundPlayer } from "./blackjack-game-round-player";
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { BlackjackHand } from "./blackjack-hand";

export interface BlackjackGameRound {
  roundPlayers: BlackjackGameRoundPlayer[];
  dealerHand: BlackjackHand;
  settlements: BlackjackHandSettlement[]
  isInitialized: boolean;
  dealerHas21: boolean;
}
