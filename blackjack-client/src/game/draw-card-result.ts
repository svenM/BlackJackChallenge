import { Card } from "./card";

export interface DrawCardResult {
  updatedDeck: Card[];
  card: Card;
}