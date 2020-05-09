import { HandProps, Hand } from "./hand.component";
import React from "react";
import { Grid, Typography } from "@material-ui/core";
import { PlayingCard } from "./playingcard.component";

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

  const singleFlippedCard = <div className="tablespot-hand">
    <PlayingCard {...{rank: '', suit: '', suitCode: '', showCard: false, isRotated: false }}></PlayingCard>
  </div>;

  const playerHand = <div className="tablespot-hand-card-container">
    <Hand {...hand}></Hand>
  </div>;

  return <React.Fragment>
    <Grid item xs={6} sm={2} className={tablespotCssClass}>
      {timerBar}
      {outcome}
      <Grid item xs={12} className="tablespot-hand">
        { !hand || hand.cards.length === 0 ? singleFlippedCard : playerHand}
      </Grid>
      <Grid item xs={12} className="tablespot-wager">
        <Typography variant="h4" gutterBottom>{ hand ? hand.score : '' }</Typography>
        <Typography variant="h4" gutterBottom>{ wager ? `$ ${wager}` : '' }</Typography>
      </Grid>
      <Grid item xs={12} className="tablespot-player">
        <Typography variant="h4" gutterBottom>{ `$ ${balance}` }</Typography>
        <Typography variant="h4" gutterBottom>{ name }</Typography>
      </Grid>
    </Grid>
  </React.Fragment>;
}
