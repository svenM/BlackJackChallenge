import { Axios } from 'axios-observable';
import { Observable } from "rxjs";
import { AxiosResponse } from "axios";
import { BlackjackGame } from './blackjack-game';
import { PlayerAccount } from './player-account';

export class GameService {

  public getGame(gameId: string): Observable<AxiosResponse<BlackjackGame>> {
    return Axios.get(`https://localhost:5001/${gameId}/details`);
  }

  public addGame(gameId: string, playerName: string, seatNo: number): Observable<AxiosResponse<PlayerAccount>> {
    return Axios.put(`https://localhost:5001/api/game/${gameId}/join?playerName=${playerName}&seatNo=${seatNo}`);
  }

  public placeBet(gameId: string, playerId: string, amount: number): Observable<AxiosResponse<any>> {
    return Axios.post(`https://localhost:5001/api/game/${gameId}/player/${playerId}/bet/${amount}`);
  }

  public forceStand(gameId: string, playerId: string): Observable<AxiosResponse<any>> {
    return Axios.post(`https://localhost:5001/api/game/${gameId}/player/${playerId}/stand`);
  }

  public deal(gameId: string): Observable<AxiosResponse<BlackjackGame>> {
    return Axios.get(`https://localhost:5001/api/game/${gameId}/deal`);
  }

  public endRound(gameId: string): Observable<AxiosResponse<BlackjackGame>> {
    return Axios.get(`https://localhost:5001/api/game/${gameId}/end`);
  }

  public removePlayer(gameId: string, playerId: string): Observable<AxiosResponse<any>> {
    return Axios.delete(`https://localhost:5001/api/game/${gameId}/remove/${playerId}`);
  }

  public playerActionRequest(gameId: string, playerId: string, request: 'hit' | 'doubledown' | 'stand'): Observable<AxiosResponse<any>> {
    return Axios.delete(`https://localhost:5001/api/game/${gameId}/request/${playerId}/${request}`);
  }
}
