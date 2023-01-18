using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.Interface
{
    public interface IPlayerRepo
    {
        Task<List<Player>> GetAllPlayersByLastName(string LastName);
        Task<List<Player>> GetAllPlayers();
        Task<List<Player>> GetPlayerByTeam(int teamId);
        Task<Player> GetPlayerById(int id);
        Task<Player> GetPlayerByLastName(string lastName);
       Task AddPlayer(Player player);
    }
}