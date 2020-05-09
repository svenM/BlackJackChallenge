import React from 'react';
import { Button, TextField, ButtonGroup, Grid, Typography } from '@material-ui/core';
import Modal from '@material-ui/core/Modal';
import { CreateGameRequest } from './create-game-request';
import { LobbyService } from './lobby.service';
import { Game } from './game';

interface LobbyProps {}
interface LobbyState {
  createGameRequest: CreateGameRequest;
  createGameOpen: any;
  joinGameOpen: any;
  gameId: string | null;
  gamesList: Game[];
}

export default class Lobby extends React.Component<LobbyProps, LobbyState> {

  service: LobbyService;

  constructor(props: LobbyProps) {
    super(props);
    this.state = {
      createGameOpen: false,
      joinGameOpen: false,
      createGameRequest: {
        gameName: '',
        minBet: 5,
        maxBet: 100
      },
      gameId: null,
      gamesList: []
    };
    this.service = new LobbyService();
  }

  componentDidMount() {
    this.service.getGames().subscribe(games => this.setState({...this.state, gamesList: games}));
  }

  showCreateGameModal() {
    this.setState({createGameOpen: true});
  };
  hideCreateGameModal() {
    this.setState({createGameOpen: false});
  };
  createGame() {
    this.service.createGame(this.state.createGameRequest)
      .subscribe(gameId => {
        this.setState({gameId})
        this.enterGame();
      });
  }
  enterGame() {
    if (this.state.gameId){
      window.location.href=`/game/${this.state.gameId}`;
    }
  };
  showJoinGameModal() {
    this.setState({joinGameOpen: true});
  };
  hideJoinGameModal() {
    this.setState({joinGameOpen: false});
  };

  handleGameNameChanged(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({createGameRequest: {...this.state.createGameRequest, gameName: event.currentTarget.value}});
  }

  handleMinWagerChanged(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({createGameRequest: {...this.state.createGameRequest, minBet: parseInt(event.currentTarget.value, 10)}});
  }

  handleMaxWagerChanged(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({createGameRequest: {...this.state.createGameRequest, maxBet: parseInt(event.currentTarget.value, 10)}});
  }
  handleGameIdChanged(event: React.ChangeEvent<HTMLInputElement>) {
    this.setState({gameId: event.currentTarget.value});
  }

  getGameLink(id: string) {
    return () => {
      this.setState({...this.state, gameId: id});
      this.enterGame();
    };
  }

  render() {

  const games = this.state.gamesList.map(g => <Typography key={g.id} variant="h6"><Button onClick={this.getGameLink(g.id)}>{g.name}</Button></Typography>);

    return <React.Fragment>
      <Grid className="lobby" container direction="row" spacing={5}>
        <Grid item className="lobby__title" xs={12}>
          <h1 style={{fontFamily: 'monofett', fontSize: 90, fontStyle: 'italic'}}>BLACKJACK</h1>
        </Grid>
        <Grid item xs={12} className="lobby__buttons">
          {/* <ButtonGroup color="primary" variant="contained"> */}
            <Button color="primary" variant="contained" onClick={this.showCreateGameModal.bind(this)}>Create new game</Button>
            {/* <Button color="primary" onClick={this.showJoinGameModal.bind(this)}>Join game</Button> */}
            {/* </ButtonGroup> */}
        </Grid>
        <Grid item xs={12} className="lobby__games_list">
        <Typography variant="h4" gutterBottom>Games List</Typography>
          {games}
        </Grid>
      </Grid>
      <Modal open={this.state.createGameOpen}>
        <Grid container
              direction="row"
              justify="space-around"
              alignItems="center"
              spacing={2}
              style={{
                top: `50%`,
                left: `50%`,
                transform: `translate(-50%, -50%)`,
                position: 'absolute',
                width: 600,
                height: 400,
                backgroundColor: '#fff',
                boxShadow: '5px 10px #888',
                padding: '2, 4, 3'}}>
          <Grid item xs={12}>
            <h2>Create new game</h2>
          </Grid>
          <Grid item xs={12}>Set up a new game. When the game is created, you will receive a link that you can share to invite other people.</Grid>
          <Grid item xs={12}>
            <TextField label="Game Name"
                      defaultValue={this.state.createGameRequest.gameName}
                      onChange={this.handleGameNameChanged.bind(this)}>>
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <TextField label="Min Wager"
                      defaultValue={this.state.createGameRequest.minBet}
                      onChange={this.handleMinWagerChanged.bind(this)}>>
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <TextField label="Max Wager"
                      defaultValue={this.state.createGameRequest.maxBet}
                      onChange={this.handleMaxWagerChanged.bind(this)}>>
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <ButtonGroup variant="contained">
              <Button color="primary" onClick={this.createGame.bind(this)}>Create game</Button>
              <Button color="secondary" onClick={this.hideCreateGameModal.bind(this)}>Cancel</Button>
            </ButtonGroup>
          </Grid>
        </Grid>
      </Modal>
      <Modal open={this.state.joinGameOpen}>
        <Grid container
              direction="row"
              justify="space-around"
              alignItems="center"
              spacing={2}
              style={{
                top: `50%`,
                left: `50%`,
                transform: `translate(-50%, -50%)`,
                position: 'absolute',
                width: 600,
                height: 400,
                backgroundColor: '#fff',
                boxShadow: '5px 10px #888',
                padding: '2, 4, 3'}}>
          <Grid item xs={12}>
            <h2>Join game</h2>
          </Grid>
          <Grid item xs={12}>Enter the game Id you received.</Grid>
          <Grid item xs={12}>
            <TextField label="Game Name"
                      defaultValue={this.state.createGameRequest.gameName}
                      onChange={this.handleGameIdChanged.bind(this)}>
            </TextField>
          </Grid>
          <Grid item xs={12}>
            <ButtonGroup variant="contained">
              <Button color="primary" onClick={this.enterGame.bind(this)}>Join game</Button>
              <Button color="secondary" onClick={this.hideJoinGameModal.bind(this)}>Cancel</Button>
            </ButtonGroup>
          </Grid>
        </Grid>
      </Modal>
    </React.Fragment>
  }
}
