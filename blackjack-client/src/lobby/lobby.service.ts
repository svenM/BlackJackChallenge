import { CreateGameRequest } from "./create-game-request";
import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";

export class LobbyService {

  public createGame(request: CreateGameRequest): Observable<AxiosResponse<string>> {
    return Axios.post(`https://localhost:5001/api/lobby/newgame/${request.gameName}/${request.minBet}/${request.maxBet}`);
  }

}
