using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Interface
{
    public interface ITeamsRepo
    {
        Task CreateTeam(Team team);
        Task AddPlayerToTeam(int teamId, int playerId);
        Task<Team> GetTeamById(int teamId);
        Task<List<Team>> GetTeamOrderBy(string orderClause);
        Task<bool> ValidatePlayerAdd(int teamID, int playerID);
        Task<bool> ValidatePlayerCount(int teamID);
        Task<bool> ValidateTeamAdd(Team team);
        Task<Team> RemovePlayerFromTeam(int teamId, int playerId);
        Task<List<Team>> GetAllTeams();

    }
}