import { BlackjackGamePlayer } from "./blackjack-game-player";
import { BlackjackHandSettlement } from "./blackjack-hand-settlement";
import { BlackjackHand } from "./blackjack-hand";

export interface BlackjackGame {
  name: string;
  dealerHand: BlackjackHand;
  dealerHas21: boolean;
  players: BlackjackGamePlayer[];
  maxPlayers: number;
  minWager: number;
  maxWager: number;
  isRoundInProgress: boolean;
  percentRemainingInDealerShoe: number;
  roundInProgressSettlements: BlackjackHandSettlement[];
}
