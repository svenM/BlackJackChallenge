
using BlackJackApi.DAL;
using BlackJackApi.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlackJackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LobbyController : ControllerBase
    {
        private readonly IBlackJackDAL _blackJackDAL;

        public LobbyController(IBlackJackDAL blackJackDAL)
        {
            this._blackJackDAL = blackJackDAL;
        }

        [HttpGet]
        [Route("list")]
        public ActionResult<List<LiveBlackjackGame>> GetGameList()
        {
            return Ok(_blackJackDAL.GetGames());
        }

        [HttpPost]
        [Route("newgame/{name}/{minbet}/{maxbet}")]
        public ActionResult<string> CreateNewGame(string name, int minbet, int maxbet)
        {
            if (minbet <= 0) return BadRequest("Min bet < 0");
            if (maxbet < minbet) return BadRequest("Max bet < min bet");
            if (maxbet > 200) return BadRequest("Max bet > 200");

            {
                var game = new LiveBlackjackGame(name, minbet, maxbet, 30, 10);
                _blackJackDAL.AddGame(game);
                return game.Id;
            }
        }

        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult<LiveBlackjackGame> Detail(string id)
        {
            var game = _blackJackDAL.GetGame(id);
            if(game != null)
            {
                return Ok(game);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpDelete]
        [Route ("deletegame/{id}")]
        public ActionResult Delete(string id)
        {
            _blackJackDAL.RemoveGame(id);
            return Ok();
        }

    }
}
