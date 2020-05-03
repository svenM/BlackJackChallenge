import { BlackjackHand } from "./blackjack-hand";
import { PlayerAccount } from "./player-account";

export interface BlackjackGamePlayer {
  id: string,
  position: number,
  account: PlayerAccount
  alias: string;
  isLive: boolean,
  hasAction: boolean,
  wager: string,
  hand: BlackjackHand,
}
