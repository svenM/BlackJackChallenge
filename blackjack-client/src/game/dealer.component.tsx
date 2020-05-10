import React from 'react';
import { Grid, Typography } from '@material-ui/core';
import { HandProps, Hand } from './hand.component';
import './dealer.css';

export interface DealerProps {
  name: string;
  hand: HandProps;
  canShowHand: boolean;
  percentOfCardsRemainingInSchoe: number;
}

export const Dealer: React.FunctionComponent<DealerProps> = ({name, hand, canShowHand, percentOfCardsRemainingInSchoe}) => {

  const emptyHand = <div className="tablespot-hand">
    <div className='emptySeat'></div>
  </div>;

  const deckSize = (percentOfCardsRemainingInSchoe * 32) / 100;
  const deck = Array.from(Array(deckSize).keys()).map(key => <li key={key}><div className="card back">*</div></li>);

  return <React.Fragment>
    <Grid container className="tablespot">
      <Grid item xs={6}>
        <Grid item xs={12} className="tablespot-player">
          <Typography variant="h4" gutterBottom>{ name }</Typography>
        </Grid>
        <Grid item xs={12} className="tablespot-hand">
          { !hand || hand.cards.length === 0 ? emptyHand : <Hand {...hand}></Hand>}
        </Grid>
        <Grid item xs={12} className="tablespot-wager">
        <Typography variant="h6" gutterBottom>Total: { canShowHand && hand ? hand.score : '' }</Typography>
        </Grid>
      </Grid>
      <Grid item xs={6}>
        <Typography variant="h6" gutterBottom>Deck</Typography>
        <ul className="deck">
          {deck}
        </ul>
      </Grid>
    </Grid>
  </React.Fragment>;
}

