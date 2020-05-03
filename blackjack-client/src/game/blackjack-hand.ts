import { Card } from "./card";

export interface BlackjackHand {
  cards: Card[],
  scoreHighLow: {low: number; high: number};
  score: string
  isBlackjack: boolean,
  isBusted: boolean,
  isSoft: boolean,
}
