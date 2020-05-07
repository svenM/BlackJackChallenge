import { HandProps } from "./hand.component";

export interface PlayerProps {
  id: string;
  name: string;
  balance: number;
  wager: number;
  isLive: boolean;
  hasAction: boolean;
  hand?: HandProps;
  position: number;
  recentWagerOutcome: string;
  secondsAwaitingAction: number;
}
