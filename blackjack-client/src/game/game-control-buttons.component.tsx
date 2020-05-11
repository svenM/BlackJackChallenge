import React from "react";
import { PlayerProps } from "./player.component";
import { WagerOutcome } from "./wager-outcome";
import { Button, Grid, ButtonGroup, Typography, Input, InputAdornment } from "@material-ui/core";

export interface GameControlButtons {
  wagerPeriodTimerIsVisible: boolean;
  wagerPeriodInSeconds: number;
  secondsAwaitingWagers: number
  player?: PlayerProps,
  hitButtonisVisible: boolean;
  standButtonIsVisible: boolean;
  doubleDownButtonisVisible: boolean;
  wagerInputIsVisible: boolean;
  nextRoundButtonIsVisible: boolean;
  minWager: number;
  maxWager: number;
  onHitClick: () => void;
  onStandClick: () => void;
  onDoubleDownClick: () => void;
  onPlacebetClick: (betValue: number) => void;
  onNewRoundClick: () => void;
}

export const GameControlButtons: React.FunctionComponent<GameControlButtons> = ({
  wagerPeriodTimerIsVisible,
  wagerPeriodInSeconds,
  secondsAwaitingWagers,
  player,
  hitButtonisVisible,
  standButtonIsVisible,
  doubleDownButtonisVisible,
  wagerInputIsVisible,
  nextRoundButtonIsVisible,
  minWager,
  maxWager,
  onHitClick,
  onStandClick,
  onDoubleDownClick,
  onPlacebetClick,
  onNewRoundClick,
}) => {

  const [betValue, setBetValue] = React.useState(0);

  const onBetValueChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
    const val = event.target.value;
    if (val) {
      setBetValue(parseInt(val, 10));
    }
  };

  const onBetClick = () => {
    onPlacebetClick(betValue);
  }

  let wagerPeriodTimer: any;
  // if (wagerPeriodTimerIsVisible) {
  //   wagerPeriodTimer = <div>
  //     The next hand will start in { wagerPeriodInSeconds - secondsAwaitingWagers} seconds
  //   </div>;
  // }
  let recentOutcome: any = '';
  if (player && player.recentWagerOutcome) {
    switch (player.recentWagerOutcome){
      case WagerOutcome.Win.toString():
        recentOutcome = 'You win!';
        break;
      case WagerOutcome.Lose.toString():
        recentOutcome = 'You lose';
        break;
      case WagerOutcome.Draw.toString():
        recentOutcome = 'Draw';
        break;
    }
  }
  let turnNotification: any = hitButtonisVisible || standButtonIsVisible || doubleDownButtonisVisible ? <h3>It's your turn</h3> : undefined;
  let hitButton: any = hitButtonisVisible ? <Button color="primary" onClick={onHitClick}>Hit</Button> : undefined;
  let standButton: any = standButtonIsVisible ? <Button color="primary" onClick={onStandClick}>Stand</Button> : undefined;
  let doubleDownButton: any = doubleDownButtonisVisible ? <Button color="primary" onClick={onDoubleDownClick}>Double Down</Button> : undefined;
  let wagerInput: any = wagerInputIsVisible ? <React.Fragment>
    <h3>Place your bet</h3>
      <Input startAdornment={<InputAdornment position="start"><b>$</b></InputAdornment>} inputProps={{min: {minWager}, max: {maxWager}}} value={betValue} onChange={onBetValueChanged} />
      <Button color="primary" variant="contained" onClick={onBetClick}>Place Bet</Button>
  </React.Fragment> : undefined;
  let nextRoundButton: any = nextRoundButtonIsVisible ? <Button color="primary" variant="contained" onClick={onNewRoundClick}>Next round</Button> : undefined;
  return <React.Fragment>
    <Grid item xs={12}>
      {wagerPeriodTimer}
    </Grid>
    <Grid item xs={12}>
      <Typography variant="h4" gutterBottom>{recentOutcome}</Typography>
    </Grid>
    <Grid item xs={12}>
      {turnNotification}
    </Grid>
    <Grid item xs={12}>
      <ButtonGroup variant="contained">
        {hitButton}
        {standButton}
        {doubleDownButton}
      </ButtonGroup>
    </Grid>
    <Grid item xs={12}>
      {nextRoundButton}
    </Grid>
    <Grid item xs={12}>
      {wagerInput}
    </Grid>
  </React.Fragment>;
}
