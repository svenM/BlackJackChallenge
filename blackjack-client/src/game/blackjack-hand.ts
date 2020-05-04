import { Card } from "./card";

export interface BlackjackHand {
  cards: Card[],
  scoreHighLow: {item1: number; item2: number};
  score: number
  isBlackjack: boolean,
  isBusted: boolean,
  isSoft: boolean,
}
