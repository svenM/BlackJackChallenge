import { CardSuit } from "./cardsuit"
import { CardRank } from "./cardrank"

export class CardImages {
  public static getCardImagePath(suit: CardSuit, rank: CardRank): string {
    let fileName: string = '';

    const rankNum = rank as number;
    if (rankNum >= CardRank.Two && rankNum <= CardRank.Ten){
      fileName = rankNum.toString();
    } else {
      fileName = rank.toString().substring(0, 1);
    }

    fileName += suit.toString().substring(0, 1);

    fileName += '.png';

    return `./images/${fileName}`;
  }
}
