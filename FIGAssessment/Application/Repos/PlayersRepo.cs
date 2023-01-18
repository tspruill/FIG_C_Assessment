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
    public class PlayersRepo : IPlayerRepo
    {
        public DataContext _context { get; }
        public PlayersRepo(DataContext context)
        {
            _context = context;
            
        }

        public  async Task AddPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Player> GetPlayerById(int id)
        {
            return await  _context.Players.Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            return await _context.Players.Include(p => p.Team).ToListAsync();
        }

        public async Task<List<Player>> GetAllPlayersByLastName(string LastName)
        {
            return await _context.Players.Where(p => p.LastName == LastName).ToListAsync();
        }

        public async Task<List<Player>> GetPlayerByTeam(int teamId)
        {
            var players = (from p in _context.Players.AsNoTracking()
                                        where p.Team.Id == teamId
                                        select p).ToListAsync();
            return await players;
        }

        public async Task<Player> GetPlayerByLastName(string lastName)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.LastName == lastName);
        }
    }
}