import { HandProps } from "./hand.component";

export interface PlayerProps {
  id: string;
  name: string;
  balance: string;
  wager: string;
  isLive: boolean;
  hasAction: boolean;
  hand: HandProps;
  position: number;
  recentWagerOutcome: string;
  secondsAwaitingAction: number;
}
