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
  onQuit: () => void;
}

export const GameHeader: React.FunctionComponent<GameHeaderProps> = ({title, currentPlayerName, currentPlayerBalance, wagerInputIsVisible, minWager, maxWager, onQuit}) => {

  const returnToLobbyButton = <Button variant="contained" color="secondary" onClick={useHistory().goBack} >&larr; Lobby</Button>;
  const playerInfo = <div>
    <Typography variant="h4" gutterBottom><b>{currentPlayerName}</b> - Balance: € {currentPlayerBalance}</Typography>
    <Button variant="contained" color="secondary" onClick={onQuit} disabled={!wagerInputIsVisible}>Quit game</Button>
  </div>;
  const gameTitle = <Typography variant="h4" gutterBottom>Blackjack - {title} -  [ €{minWager} - €{maxWager} ]</Typography>;

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
