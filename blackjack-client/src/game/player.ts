import { Hand } from "./hand";

export interface Player {
  id: string,
  name: string,
  balance: string,
  wager: string,
  isLive: boolean,
  hasAction: boolean,
  hand: Hand,
  position: number,
  recentWagerOutcome: string,
  secondsAwaitingAction: number
}
