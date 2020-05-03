import { Card } from "./card";

export interface Hand {
  cards: Card[],
  isBusted: boolean,
  isBlackjack: boolean,
  isSoft: boolean,
  score: string
}
