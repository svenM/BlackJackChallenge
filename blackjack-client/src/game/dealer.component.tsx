import React from 'react';
import { Grid } from '@material-ui/core';
import { HandProps, Hand } from './hand.component';
import { PlayingCard } from './playingcard.component';
import './dealer.css';

export interface DealerProps {
  name: string;
  hand: HandProps;
  canShowHand: boolean;
  percentOfCardsRemainingInSchoe: number;
}

export const Dealer: React.FunctionComponent<DealerProps> = ({name, hand, canShowHand, percentOfCardsRemainingInSchoe}) => {

  const singleFlippedCard = <div className="tablespot-hand">
    <PlayingCard {...{rank: '', suit: '', suitCode: '', showCard: false, isRotated: false }}></PlayingCard>
  </div>;

  const dealerHand = <div className="tablespot-hand-card-container">
    <Hand {...hand}></Hand>
  </div>;

  const deckSize = (percentOfCardsRemainingInSchoe * 32) / 100;
  const deck = new Array(deckSize).map(_ => <li><div className="card back">*</div></li>);

  return <React.Fragment>
    <Grid container className="tablespot">
      <Grid item xs={12} className="tablespot-hand">
        { !hand || hand.cards.length === 0 ? singleFlippedCard : dealerHand}
      </Grid>
    <Grid item xs={12} className="tablespot-wager">
        { canShowHand && hand ? hand.score : '' }
      </Grid>
      <Grid item xs={12} className="tablespot-player">
        { name }
      </Grid>
      <Grid item xs={12}>
        <em>{ percentOfCardsRemainingInSchoe }% of shoe remaining</em>
        <ul className="deck">
          {deck}
        </ul>
      </Grid>
    </Grid>
  </React.Fragment>;
}

