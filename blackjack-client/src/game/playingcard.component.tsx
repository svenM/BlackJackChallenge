import React from 'react';
import './cards.css';
import { CardRank } from './cardrank';
import { CardSuit } from './cardsuit';
import './playingcard.css';

export interface PlayingCardProps {
  rank?: CardRank;
  suit?: CardSuit;
  showCard: boolean;
  isRotated: boolean;
}

export const PlayingCard: React.FunctionComponent<PlayingCardProps> = ({rank, suit, showCard, isRotated}) => {

  let containerCssClass = 'tablespot-hand-card-container';
  let cardCssClass = 'card back';
  let rankContent = '-';
  let suitContent = '-';

  const getRankContent = (rank: CardRank) => {
    switch (rank) {
      case CardRank.Ace:
        return 'A';
      case CardRank.Two:
        return '2';
      case CardRank.Three:
        return '3';
      case CardRank.Four:
        return '4';
      case CardRank.Five:
        return '5';
      case CardRank.Six:
        return '6';
      case CardRank.Seven:
        return '7';
      case CardRank.Eight:
        return '8';
      case CardRank.Nine:
        return '9';
      case CardRank.Ten:
        return '10';
      case CardRank.Jack:
        return 'J';
      case CardRank.Queen:
        return 'Q';
      case CardRank.King:
        return 'K';
    }
  }
  const getSuitContent = (rank: CardRank, suit: CardSuit) => {
    if (rank === CardRank.Jack || rank === CardRank.Queen || rank === CardRank.King) {
      switch (suit) {
        case CardSuit.Club:
          return '♣';
        case CardSuit.Diamond:
          return '♦';
        case CardSuit.Heart:
          return '♥';
        case CardSuit.Spade:
          return '♠';
      }
    }
    return ' ';
  };

  const getSuitCode = (suit: CardSuit) => {
    switch (suit) {
      case CardSuit.Club:
        return 'clubs';
      case CardSuit.Diamond:
        return 'diams';
      case CardSuit.Heart:
        return 'hearts';
      case CardSuit.Spade:
        return 'spades';
    }
  }

  const getCardClass = (rank: CardRank, suit: CardSuit) => {
    const rankCode = getRankContent(rank).toLowerCase();
    const suitCode = getSuitCode(suit);
    return `card rank-${rankCode} ${suitCode}`
  }

  if (isRotated) {
    containerCssClass = `${containerCssClass} rotate5degrees`;
  }

  if (showCard && (rank !== undefined) && (suit !== undefined)) {
    cardCssClass = getCardClass(rank, suit);
    rankContent = getRankContent(rank);
    suitContent = getSuitContent(rank, suit);
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
