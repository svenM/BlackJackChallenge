using BlackJackApi;
using BlackJackApi.DAL;
using BlackJackApi.Domain;
using BlackJackApi.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BlackJackApi.Controllers
{
    public class GameController : Controller
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public GameController(IBlackJackDAL blackJackDAL)
        {
            this._blackJackDAL = blackJackDAL;
        }

        [HttpPut]
        [Route("{gameId}/join/{playerName}/{seatNo}")]
        public ActionResult<PlayerAccount> Join(string gameId, string playerName, int seatNo)
        {
            try
            {

                var game = _blackJackDAL.GetGame(gameId);
                if (game == null) return NotFound("Game not found");
                double balance = 1000;

                var account = new PlayerAccount(
                    id: Guid.NewGuid().ToString().Replace("-", ""),
                    startingBalance: balance);

                game.AddPlayer(account, playerName, seatNo);
                _blackJackDAL.SaveGame(game);
                return Ok(account);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("{gameId}/hint/{playerId}")]
        public ActionResult<PlayerAction> GetHint(string gameId, string playerId)
        {
            try
            {
                var decider = new PlayerActionDecider();
                var game = _blackJackDAL.GetGame(gameId);
                if (game == null) return NotFound("Game not found");
                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null || player.Hand == null || !player.Hand.Cards.Any()) return NotFound("Player not found or no cards");

                return decider.DecideAction(player.Hand, game.DealerHand.Cards.First());
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
                var game = _blackJackDAL.GetGame(gameId);
                if (game == null) return NotFound("Game not found");
                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if(player == null)
                {
                    return BadRequest("Bad player");
                }
                game.PlayerWagerRequest(player, amount);
                _blackJackDAL.SaveGame(game);
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
                var game = _blackJackDAL.GetGame(gameId);
                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                game.ForceCurrentActionToStand();
                _blackJackDAL.SaveGame(game);
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
            var game = _blackJackDAL.GetGame(gameId);

            if (game == null)
                return NotFound();

            return Ok(game);
        }

        [HttpPut]
        [Route("{gameId}/deal")]
        public ActionResult Deal(string gameId)
        {
            try
            {
                var game = _blackJackDAL.GetGame(gameId);
                game.StartRound();
                _blackJackDAL.SaveGame(game);
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
                var game = _blackJackDAL.GetGame(gameId);
                game.EndRound();
                _blackJackDAL.SaveGame(game);
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
                var game = _blackJackDAL.GetGame(gameId);
                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                game.RemovePlayer(player);
                _blackJackDAL.SaveGame(game);
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
                var game = _blackJackDAL.GetGame(gameId);
                var player = game.Players.FirstOrDefault(p => p.Id == playerId);
                if (player == null)
                {
                    return BadRequest("Bad player");
                }
                game.PlayerActionRequest(player, request);
                _blackJackDAL.SaveGame(game);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
               
    }
}

