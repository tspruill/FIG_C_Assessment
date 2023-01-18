using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsRepo _teamsRepo;
       public TeamsController(ITeamsRepo teamsRepo)
       {
            _teamsRepo = teamsRepo;
        
       } 
          [HttpGet("GetAllTeams")]
        public async Task<ActionResult<List<Team>>> GetAllTeams()
        {
            return Ok(await _teamsRepo.GetAllTeams());
        }
        [HttpGet("{id}")]
       public async Task<ActionResult> GetTeamById(int id)
       {
            var result = await _teamsRepo.GetTeamById(id);
            return result == null ? NotFound() : Ok(result);
       }
        [HttpDelete("{teamdId}/{playerId}")]
        public async Task<ActionResult> RemovePlayerFromTeam(int teamdId, int playerId)
       {
            try{
            
                var team = await _teamsRepo.RemovePlayerFromTeam(teamdId,playerId);
                return Ok(team);
            

        }catch(Exception e){
            return StatusCode(500);
        }
       }
       [HttpGet]
       public async Task<ActionResult<List<Team>>> OrderBy([FromQuery]string orderBy)
       {
           var result = await _teamsRepo.GetTeamOrderBy(orderBy);
           return result == null ? NotFound() : Ok(result);

       }
       [HttpGet("teamid/{teamdId}/playerId/{playerId}")]
       public async Task<ActionResult> AddPlayerToTeam(int teamdId, int playerId)
       {
        try{
            if(await _teamsRepo.ValidatePlayerCount(teamdId))
            {
                if(await _teamsRepo.ValidatePlayerAdd(teamdId,playerId))
                {
                    await _teamsRepo.AddPlayerToTeam(teamdId,playerId);
                    return RedirectToAction("GetTeamById",new {id = teamdId});
                }else
                    return StatusCode(500, "Player on existing team");
            }else
                return StatusCode(500, "Team already has 8 players cannot exceed capacity");


        }catch(Exception e){
            return StatusCode(500);
        }
       }
       [HttpPost]
       public async Task<ActionResult> CreateTeam(Team team)
       {
        if(await _teamsRepo.ValidateTeamAdd(team)){
            await _teamsRepo.CreateTeam(team);
            return CreatedAtAction(nameof(GetTeamById),new { id = team.Id},team);
        }else 
           return StatusCode(500,"Team with same Name or Location exist");
       }
    }
}