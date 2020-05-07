import { CreateGameRequest } from "./create-game-request";
import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { Game } from "./game";
import { map } from 'rxjs/operators';

export class LobbyService {

  public getGames(): Observable<Game[]> {
    return Axios.get(`https://localhost:5001/api/lobby/list`)
      .pipe(map((response: AxiosResponse<any[]>) => response.data.map((d: any) => {
        return { id: d.id, name: d.name } as Game;
      })));
  }

  public createGame(request: CreateGameRequest): Observable<string> {
    return Axios.post(`https://localhost:5001/api/lobby/newgame/${request.gameName}/${request.minBet}/${request.maxBet}`)
      .pipe(map((response: AxiosResponse<string>) => response.data as string));
  }

  public deleteGame(gameId: string): Observable<any> {
    return Axios.delete(`https://localhost:5001/api/lobby/deleteGame/${gameId}`);
  }
}
