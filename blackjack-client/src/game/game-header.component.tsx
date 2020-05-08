import React from "react";
import { Grid, Button, Typography } from "@material-ui/core";
import { useHistory } from 'react-router-dom';
import './game-header.css';

export interface GameHeaderProps {
  title: string;
  currentPlayerName: string;
  currentPlayerBalance: number;
  wagerInputIsVisible: boolean;
  minWager: number;
  maxWager: number;
}

export const GameHeader: React.FunctionComponent<GameHeaderProps> = ({title, currentPlayerName, currentPlayerBalance, wagerInputIsVisible, minWager, maxWager}) => {

  const returnToLobbyButton = <Button variant="contained" color="secondary" onClick={useHistory().goBack} >&larr; Lobby</Button>;
  const playerInfo = <div>
    <Typography variant="h4" gutterBottom><b>{currentPlayerName}</b> ${currentPlayerBalance}</Typography>
    <Button variant="contained" color="secondary" disabled={!wagerInputIsVisible}>Quit game</Button>
  </div>;
  const gameTitle = <Typography variant="h4" gutterBottom>{title} Blackjack ${minWager} - ${maxWager}</Typography>;

  return <React.Fragment>
    <Grid container className="game-header">
      <Grid item xs={12} sm={6}>
        { currentPlayerName.length > 0 ? playerInfo : returnToLobbyButton }
      </Grid>
      <Grid item xs={12} sm={6}>
        { gameTitle }
      </Grid>
    </Grid>
  </React.Fragment>;
}
