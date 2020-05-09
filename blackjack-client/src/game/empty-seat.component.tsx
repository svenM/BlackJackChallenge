import React from "react";
import { Grid, Input, Button } from "@material-ui/core";


export interface EmptySeatProps {
  seatNo: number;
  canJoin: boolean;
  onJoin: (position: number, name: string) => void;
}


export const EmptySeat: React.FunctionComponent<EmptySeatProps> = ({seatNo, canJoin, onJoin}) => {

  const [playerName, setPlayerName] = React.useState('');

  const onPlayerNameChanged = (event: React.ChangeEvent<HTMLInputElement>) => {
    const val = event.target.value;
    if (val) {
      setPlayerName(val);
    }
  };

  const onJoinClicked = () => {
    if (playerName){
      onJoin(seatNo, playerName);
    }
  }

  return <React.Fragment>
    <Grid item xs={6} sm={2}>
      <Grid item xs={6}>
        <Input onChange={onPlayerNameChanged} />
      </Grid>
      <Grid item xs={6}>
        <Button color="primary" variant="contained" onClick={onJoinClicked}>Join</Button>
      </Grid>
    </Grid>
  </React.Fragment>;
}
