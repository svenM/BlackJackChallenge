import React from "react";
import { Grid, Button, TextField, ButtonGroup, Modal, Typography } from "@material-ui/core";
import './empty-seat.css';

export interface EmptySeatProps {
  seatNo: number;
  canJoin: boolean;
  onJoin: (position: number, name: string) => void;
}

export const EmptySeat: React.FunctionComponent<EmptySeatProps> = ({seatNo, canJoin, onJoin}) => {

  const [playerName, setPlayerName] = React.useState('');
  const [playerModalOpen, togglePlayerModal] = React.useState(false);
  const askName = () => {
    togglePlayerModal(true);
  };
  const cancelJoinGame = () => {
    togglePlayerModal(false);
  };
  const handlePlayerNameChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPlayerName(event.target.value);
  };
  const handleJoinClicked = () => {
    if (playerName){
      onJoin(seatNo, playerName);
    }
  };

  const joinButton = canJoin ? <Grid item xs={12}>
    <Button color="primary" variant="contained" onClick={askName}>Join</Button>
  </Grid> : '';

  return <React.Fragment>
    <Grid item xs={12}>
      <Typography variant="h6" gutterBottom>{ `Seat ${seatNo}`  }</Typography>
      <div className='emptySeat'></div>
      {joinButton}
    </Grid>
    <Modal open={playerModalOpen}>
        <Grid container
              direction="row"
              justify="space-around"
              alignItems="center"
              spacing={1}
              style={{
                top: `50%`,
                left: `50%`,
                transform: `translate(-50%, -50%)`,
                position: 'absolute',
                width: 400,
                height: 300,
                backgroundColor: '#fff',
                boxShadow: '5px 10px #888',
                padding: '10'}}>
          <Grid item xs={12}>
            <h2>Join game</h2>
            <p>Please enter your name</p>
            <TextField label="Name"
                      defaultValue={playerName}
                      onChange={handlePlayerNameChanged}>>
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <ButtonGroup variant="contained">
              <Button color="primary" onClick={handleJoinClicked}>Join game</Button>
              <Button color="secondary" onClick={cancelJoinGame}>Cancel</Button>
            </ButtonGroup>
          </Grid>
        </Grid>
      </Modal>
  </React.Fragment>;
}
