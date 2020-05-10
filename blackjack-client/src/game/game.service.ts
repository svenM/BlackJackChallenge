import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { BlackjackGame } from './blackjack-game';
import { PlayerAccount } from './player-account';
import { map } from 'rxjs/operators';
import { PlayerAction } from './player-action';

export class GameService {

  public getGame(gameId: string): Observable<BlackjackGame> {
    return Axios.get(`https://localhost/blackjackapi/${gameId}/details`)
      .pipe(map((response: AxiosResponse<BlackjackGame>) => response.data));
  }

  public getHint(gameId: string, playerId: string): Observable<PlayerAction> {
    return Axios.get(`https://localhost/blackjackapi/${gameId}/hint/${playerId}`)
      .pipe(map((response: AxiosResponse<PlayerAction>) => response.data));
  }

  public joinGame(gameId: string, playerName: string, seatNo: number): Observable<PlayerAccount> {
    return Axios.put(`https://localhost/blackjackapi/${gameId}/join/${playerName}/${seatNo}`)
      .pipe(map((response: AxiosResponse<PlayerAccount>) => response.data));
  }

  public placeBet(gameId: string, playerId: string, amount: number): Observable<any> {
    return Axios.post(`https://localhost/blackjackapi/${gameId}/player/${playerId}/bet/${amount}`);
  }

  public forceStand(gameId: string, playerId: string): Observable<any> {
    return Axios.post(`https://localhost/blackjackapi/${gameId}/player/${playerId}/stand`);
  }

  public deal(gameId: string): Observable<any> {
    return Axios.put(`https://localhost/blackjackapi/${gameId}/deal`);
  }

  public endRound(gameId: string): Observable<any> {
    return Axios.put(`https://localhost/blackjackapi/${gameId}/endround`);
  }

  public leaveGame(gameId: string, playerId: string): Observable<any> {
    return Axios.delete(`https://localhost/blackjackapi/${gameId}/remove/${playerId}`);
  }

  public playerActionRequest(gameId: string, playerId: string, request: 'hit' | 'doubledown' | 'stand'): Observable<any> {
    return Axios.post(`https://localhost/blackjackapi/${gameId}/request/${playerId}/${request}`);
  }
}
