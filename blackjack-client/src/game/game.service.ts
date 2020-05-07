import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { BlackjackGame } from './blackjack-game';
import { PlayerAccount } from './player-account';
import { map } from 'rxjs/operators';
import { PlayerAction } from './player-action';

export class GameService {

  public getGame(gameId: string): Observable<BlackjackGame> {
    return Axios.get(`https://localhost:5001/${gameId}/details`)
      .pipe(map((response: AxiosResponse<BlackjackGame>) => response.data));
  }

  public getHint(gameId: string, playerId: string): Observable<PlayerAction> {
    return Axios.get(`https://localhost:5001/${gameId}/hint/${playerId}`)
      .pipe(map((response: AxiosResponse<PlayerAction>) => response.data));
  }

  public joinGame(gameId: string, playerName: string, seatNo: number): Observable<PlayerAccount> {
    return Axios.put(`https://localhost:5001/${gameId}/join/${playerName}/${seatNo}`)
      .pipe(map((response: AxiosResponse<PlayerAccount>) => response.data));
  }

  public placeBet(gameId: string, playerId: string, amount: number): Observable<any> {
    return Axios.post(`https://localhost:5001/${gameId}/player/${playerId}/bet/${amount}`);
  }

  public forceStand(gameId: string, playerId: string): Observable<any> {
    return Axios.post(`https://localhost:5001/${gameId}/player/${playerId}/stand`);
  }

  public deal(gameId: string): Observable<any> {
    return Axios.put(`https://localhost:5001/${gameId}/deal`);
  }

  public endRound(gameId: string): Observable<any> {
    return Axios.put(`https://localhost:5001/${gameId}/end`);
  }

  public leaveGame(gameId: string, playerId: string): Observable<any> {
    return Axios.delete(`https://localhost:5001/${gameId}/remove/${playerId}`);
  }

  public playerActionRequest(gameId: string, playerId: string, request: 'hit' | 'doubledown' | 'stand'): Observable<any> {
    return Axios.post(`https://localhost:5001/${gameId}/request/${playerId}/${request}`);
  }
}
