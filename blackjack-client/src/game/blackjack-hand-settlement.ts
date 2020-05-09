import { BlackjackHand } from "./blackjack-hand";
import { WagerOutcome } from "./wager-outcome";

export interface BlackjackHandSettlement {
  playerPosition: number;
  playerHand: BlackjackHand;
  dalerHand: BlackjackHand;
  wagerOutcome: WagerOutcome;
  wagerAmount: number;
}
