import { CardRank } from "./cardrank";
import { CardSuit } from "./cardsuit";

export interface CardProps {
  rank: CardRank;
  suit: CardSuit;
  numericValue: number;
}
