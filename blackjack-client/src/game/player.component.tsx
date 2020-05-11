import { HandProps, Hand } from "./hand.component";
import React from "react";
import { Grid, Typography } from "@material-ui/core";
import './player.css';

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

  const tablespotCssClass = 'tablespot' + (hasAction ? '-withAction' : '');
  const timerBar = secondsAwaitingAction >= 0 ?
    <div className="actionProgressBarContainer"><div className="actionProgressBar"></div></div> :
    <div className="actionProgressBar"></div>;
  const outcome = recentWagerOutcome ?
    <div className={`tablespot-outcome tablespot-outcome-${recentWagerOutcome.toLowerCase()}`}>{recentWagerOutcome}</div> : '';

  const emptyHand = <div className="tablespot-hand">
    <div className='emptySeat'></div>
  </div>;

  const score = !!hand.score ? (isNaN(parseInt(hand.score, 10))) ? hand.score : `Total: ${hand.score}` : '';

  return <React.Fragment>
    <Grid item xs={12} className={tablespotCssClass}>
      <Grid item xs={12} className="tablespot-hand">
        <Typography variant="h6" gutterBottom>{ `Seat ${position}`  }</Typography>
        { !hand || hand.cards.length === 0 ? emptyHand : <Hand {...hand}></Hand>}
        {timerBar}
        {outcome}
      </Grid>
      <Grid item xs={12} className="tablespot-player">
        <Typography variant="h6" gutterBottom>{ name }</Typography>
      </Grid>
      <Grid item xs={12} className="tablespot-wager">
        <Typography variant="h6" gutterBottom>{ score }</Typography>
        <Typography variant="h6" gutterBottom>{ wager ? `Bet € ${wager}` : '' }</Typography>
      </Grid>
    </Grid>
  </React.Fragment>;
}
