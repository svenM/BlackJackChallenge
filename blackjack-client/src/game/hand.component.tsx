import { CardProps } from "./card.component";

export interface HandProps {
  cards: CardProps[],
  isBusted: boolean;
  isBlackjack: boolean;
  isSoft: boolean;
  score: string;
}
