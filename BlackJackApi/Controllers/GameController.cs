using BlackJackApi.DAL;
using BlackJackApi.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blackjack.Mvc.Controllers
{
    public class GameController : Controller
    {
        private LiveBlackjackGame Game;

        private readonly IBlackJackDAL _blackJackDAL;

        public GameController(IBlackJackDAL blackJackDAL)
        {
            this._blackJackDAL = blackJackDAL;
        }
        /*
        private BlackjackGamePlayer _player;
        private BlackjackGamePlayer Player
        {
            get
            {
                if (_player == null)
                    _player = Game?.Players?.FirstOrDefault(a => a.Id == Session.SessionID);

                return _player;
            }
        }*/

        [HttpPut]
        [Route("{gameId}/join/{playerName}/{seatNo}")]
        public ActionResult<PlayerAccount> Join(string gameId, string playerName, int seatNo)
        {
            try
            {

                Game = _blackJackDAL.GetGame(gameId);
                if (Game == null) return NotFound("Game not found");
                double balance = 1000;

                var account = new PlayerAccount(
                    id: Guid.NewGuid().ToString().Replace("-", ""),
                    startingBalance: balance);

                Game.AddPlayer(account, playerName, seatNo);
                _blackJackDAL.SaveGame(Game);
                return Ok(account);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpPost]
        [Route("{gameId}/player/{playerId}/bet/{amount}")]
        public ActionResult PlayerBetRequest(string gameId, string playerId, int amount)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                var player = Game.Players.FirstOrDefault(p => p.Id == playerId);
                if(player == null)
                {
                    return BadRequest("Bad player");
                }
                Game.PlayerWagerRequest(player, amount);
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpPost]
        [Route("{gameId}/player/{playerId}/stand")]
        public ActionResult ForceStand(string gameId, string playerId)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                var player = Game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                Game.ForceCurrentActionToStand();
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpGet]
        [Route("{gameId}/details")]
        public ActionResult<LiveBlackjackGame> Refresh(string gameId)
        {
            Game = _blackJackDAL.GetGame(gameId);

            if (Game == null)
                return NotFound();

            return Ok(Game);
        }

        [HttpPut]
        [Route("{gameId}/deal")]
        public ActionResult Deal(string gameId)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                Game.StartRound();
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        [Route("{gameId}/endround")]
        public ActionResult EndRound(string gameId)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                Game.EndRound();
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete]
        [Route("{gameId}/remove/{playerId}")]
        public ActionResult RemoveGamePlayer(string gameId, string playerId)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                var player = Game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                Game.RemovePlayer(player);
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpPost]
        [Route("{gameId}/request/{playerId}/{request}")]
        public ActionResult PlayerActionRequest(string gameId, string playerId, string request)
        {
            try
            {
                Game = _blackJackDAL.GetGame(gameId);
                var player = Game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                Game.PlayerActionRequest(player, request);
                _blackJackDAL.SaveGame(Game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
               
    }
}

