import React from 'react';
import { Grid } from '@material-ui/core';
import { HandProps } from './hand.component';
import shoe from './images/green_back.png'

export interface DealerProps {
  name: string;
  hand?: HandProps;
  canShowHand: boolean;
  percentOfCardsRemainingInSchoe: number;
}

export const Dealer: React.FunctionComponent<DealerProps> = ({name, hand, canShowHand, percentOfCardsRemainingInSchoe}) => {

  if (!hand || hand.cards.length === 0){
    return <React.Fragment></React.Fragment>;
  }



  // TODO
  // const getCardClass = (card: Card): string => {

  // };

  // const hand = (dealer.hand && dealer.hand.cards && dealer.hand.cards.length > 0 ?
  //   <Grid container direction="column" justify="space-around" alignItems="center" spacing={2} className="dealer__hand">
  //     {dealer.hand.cards.map(card => <Grid item xs={12} style={{width: '100%'}} className={getCardClass(card)}>

  //     </Grid>)}
  //   </Grid>
  //   : null )

  return <React.Fragment>
    <h5>{name}</h5>
    <img src={shoe} alt="shoe" />
  </React.Fragment>;
}

