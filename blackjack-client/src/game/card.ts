import { CardRank } from "./cardrank";
import { CardSuit } from "./cardsuit";

export interface Card {
  rank: CardRank;
  suit: CardSuit;
  numericValue: number;
}
