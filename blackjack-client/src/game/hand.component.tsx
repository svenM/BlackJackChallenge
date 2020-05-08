import { PlayingCardProps, PlayingCard } from "./playingcard.component";
import React from "react";

export interface HandProps {
  cards: PlayingCardProps[],
  isBusted: boolean;
  isBlackjack: boolean;
  isSoft: boolean;
  score: string;
}


export const Hand: React.FunctionComponent<HandProps> = ({cards, isBusted, isBlackjack, isSoft, score}) => {
const playingCards = cards.map(c => <PlayingCard {...c}></PlayingCard>);
  return <React.Fragment>
      { playingCards }
  </React.Fragment>;
}
