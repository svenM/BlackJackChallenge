# BLACKJACK CHALLENGE

## Lobby

### Client-Server actions

#### New Game

Request: POST /lobby/create-game {
  gameName: string,
  minWager: number,
  maxWager: number,
  startBalance: number
}
Response: OK

## Game

## Client-Server actions

#### Join Game

Request: GET /game/{gameId}
Response: {
  gameId: string,
  gameName: string,
  minWager: number,
  maxWager: number,
  turnLengthInSeconds: number,
  WagerPeriodInSeconds: number,
  currentPlayerName: string,
  currentPlayerBalance: number,
  hitButtonIsVisible: boolean,
  standButtonIsVisible: boolean,
  doubleDownButtonIsVisible: boolean,
  wagerInputIsVisible: boolean,
  wagerPeriodTimerIsVisible: boolean,
  endOfRoundTimerIsVisible: boolean,
  secondsAwaitingPlayerAction: number,
  secondsAwaitingWagers: number,
  dealer: {
    name: string,
    hand: {
      cards: Array<{
        rank: string,
        suit: string,
        suitCode: string
      }>,
      isBusted: boolean,
      isBlackjack: boolean,
      isSoft: boolean,
      score: string
    },
    canShowHand: boolean,
    percentOfCardsRemainingInShoe: number
  },
  players: Array<{
    id: string,
    name: string,
    balance: string,
    wager: string,
    isLive: boolean,
    hasAction: boolean,
    hand: {
      cards: Array<{
        rank: string,
        suit: string,
        suitCode: string
      }>,
      isBusted: boolean,
      isBlackjack: boolean,
      isSoft: boolean,
      score: string
    },
    position: number,
    recentWagerOutcome: string,
    secondsAwaitingAction: number
  }>
}

#### Deal

Request: GET /game/{gameId}/deal
Response: OK

#### End Round

Request: GET /game/{gameId}/end-round
Response: OK

#### Add Player

Request: POST /game/{gameId}/players { name: string, seat: string }
Response: OK

#### Remove Player

Request: DELETE /game/{gameId}/players
Response: OK

#### Stand

Request: DELETE /game/{gameId}/stand
Response: OK

#### Player Action

Request: DELETE /game/{gameId}/{request}
Response: OK

#### Player Bet

Request: DELETE /game/{gameId}/bet/{betAmount}
Response: OK