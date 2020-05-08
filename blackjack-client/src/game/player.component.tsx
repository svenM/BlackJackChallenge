import { HandProps } from "./hand.component";
import React from "react";

export interface PlayerProps {
  id: string;
  name: string;
  balance: number;
  wager: number;
  isLive: boolean;
  hasAction: boolean;
  hand: HandProps;
  position: number;
  recentWagerOutcome: string;
  secondsAwaitingAction: number;
}


export const Player: React.FunctionComponent<PlayerProps> = ({
  id, name, balance, wager, isLive, hasAction, hand, position, recentWagerOutcome, secondsAwaitingAction
}) => {

  return <React.Fragment>

  </React.Fragment>;
}
