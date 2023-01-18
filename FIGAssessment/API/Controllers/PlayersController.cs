using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepo _playerRepo;
        public PlayersController(IPlayerRepo playerRepo)
        {
            _playerRepo = playerRepo;
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetPlayerById(int id)
        {
            var result = await _playerRepo.GetPlayerById(id);
            return result == null ? NotFound(): Ok(result);
        }
        [HttpGet("GetAllPlayers")]
        public async Task<ActionResult<List<Player>>> GetAllPlayers()
        {
            return Ok(await _playerRepo.GetAllPlayers());
        }
         [HttpGet("ByTeam/{teamId}")]
        public async Task<ActionResult<List<Player>>> GetPlayersOnTeam(int teamId)
        {
            return Ok(await _playerRepo.GetPlayerByTeam(teamId));
        }
        [HttpGet]
        public async Task<IActionResult>GetPlayerByLastName([FromQuery]string lastname)
        {
            var result = await _playerRepo.GetAllPlayersByLastName(lastname);
            return result == null ? NotFound() : Ok(result);
        }
        [HttpPost]
         public async Task<ActionResult<Player>> CreatePlayer(Player player)
         {
           await _playerRepo.AddPlayer(player);
           return CreatedAtAction(nameof(GetPlayerById), new {id = player.Id}, player);
         }
    }
}