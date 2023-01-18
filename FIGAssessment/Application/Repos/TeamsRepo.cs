using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repos
{
    public class TeamsRepo : ITeamsRepo
    {
        private readonly DataContext _context;
        public TeamsRepo(DataContext context)
        {
            _context = context;
            
        }
        public async Task<bool> ValidatePlayerAdd(int teamID, int playerID)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamID);
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerID);
            if(team == null || player == null){
                return false;
            }
            var teams = await _context.Teams.Include(t => t.Players).ToListAsync();
            foreach(var t in teams)
            {
                if(t.Players.Any(p => p.Id == playerID))
                {
                    Console.WriteLine("Player already apart of a team \n");
                    return false;

                }
            }
            return true;

        }

        public async Task<bool> ValidatePlayerCount(int teamID)
        {
            var teamPlayers = await  _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == teamID);
            return (teamPlayers.Players.Count < 8);
        }
        public async Task AddPlayerToTeam(int teamId, int playerId)
        {
            var player =  _context.Players.FirstOrDefault(p => p.Id == playerId);
            var team =  _context.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == teamId);
          
            
                _context.Attach(team);

               // teamList.Add(player);
                team.Players.Add(player);

             _context.SaveChanges();
           
            

        }
        public async Task<Team> RemovePlayerFromTeam(int teamId, int playerId)
        {
            var player =  _context.Players.FirstOrDefault(p => p.Id == playerId);
            var team =  _context.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == teamId);
          
            
                _context.Attach(team);

               // teamList.Add(player);
                team.Players.Remove(player);

             _context.SaveChanges();

             return team;
           
            

        }
        public async Task<bool> ValidateTeamAdd(Team team)
        {
            return !(_context.Teams.Any(t => t.Location.Equals(team.Location,StringComparison.InvariantCultureIgnoreCase) && t.Name.Equals(team.Name,StringComparison.InvariantCultureIgnoreCase)));
        }
        public async Task CreateTeam(Team team)
        {
            
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            
        }

        public async Task<List<Team>> GetTeamOrderBy(string orderClause)
        {
            if(orderClause.Equals("name",StringComparison.InvariantCultureIgnoreCase)){
                return await _context.Teams.Include(t => t.Players).OrderBy(t => t.Name).ToListAsync();
            }else if(orderClause.Equals("location",StringComparison.InvariantCultureIgnoreCase))
                return await _context.Teams.Include(t => t.Players).OrderBy(t => t.Location).ToListAsync();
            else
                return null;
        }

        
        public Task<List<Team>> GetAllTeams()
        {
            return _context.Teams.Include(t => t.Players).ToListAsync();
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == teamId);
        }
    }
}