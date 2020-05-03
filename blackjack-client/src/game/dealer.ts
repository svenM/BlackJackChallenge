import { Hand } from "./hand";

export interface Dealer {
  name: string,
  hand: Hand,
  canShowHand: boolean,
  percentOfCardsRemainingInShoe: number
}
