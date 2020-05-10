import { PlayingCardProps, PlayingCard } from "./playingcard.component";
import React from "react";
import { Grid } from "@material-ui/core";
import './hand.css';

export interface HandProps {
  cards: PlayingCardProps[],
  isBusted: boolean;
  isBlackjack: boolean;
  isSoft: boolean;
  score: string;
}

export const Hand: React.FunctionComponent<HandProps> = ({cards, isBusted, isBlackjack, isSoft, score}) => {

  const playingCards = cards.map((c, i) => <div key={i}><PlayingCard {...c}></PlayingCard></div>);

  return <React.Fragment>
    <Grid item xs={12} className="hand">
      { playingCards }
    </Grid>
  </React.Fragment>;
}
