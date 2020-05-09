import React from 'react';
import './cards.css';

export interface PlayingCardProps {
  rank: string;
  suit: string;
  suitCode: string;
  showCard: boolean;
  isRotated: boolean;
}

export const PlayingCard: React.FunctionComponent<PlayingCardProps> = ({rank, suit, suitCode, showCard, isRotated}) => {

  let containerCssClass = 'tablespot-hand-card-container';
  let cardCssClass = 'card back';
  let rankContent = '-';
  let suitContent = '-';

  if (isRotated) {
    containerCssClass = `${containerCssClass} rotate5degrees`;
  }

  if (showCard) {
    cardCssClass = `card rank-${rank} ${suit}`;
    rankContent = rank.toUpperCase();
    suitContent = suitCode;
  }

  return <React.Fragment>
    <div className={containerCssClass}>
      <div className={cardCssClass}>
        <span className="rank">{rankContent}</span>
        <span className="suit">{suitContent}</span>
      </div>
    </div>
  </React.Fragment>;
}
