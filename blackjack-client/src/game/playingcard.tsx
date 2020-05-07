import React from "react";

export interface PlayerCardProps {
  rank?: '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' | '10' | 'j' | 'q' | 'k' | 'a';
  suit?: 'diams' | 'hearts' | 'spades' | 'club';
  deck?: boolean;
  flipped?: boolean;
}

export const PlayingCard: React.FunctionComponent<PlayerCardProps> = ({rank, suit, deck, flipped}) => {
  const card = <div></div>;
  if (rank)
  return <React.Fragment>
    <div className="playingCards faceImages">

    </div>
  </React.Fragment>;
}
