import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { BlackjackGame } from './blackjack-game';

export class GameService {

  public getGame(gameId: string): Observable<AxiosResponse<BlackjackGame>> {
    return Axios.get(`https://localhost:5001/api/lobby/detail/${gameId}`);
  }

}
