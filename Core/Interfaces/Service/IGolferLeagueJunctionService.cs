using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Service
{
    public interface IGolferLeagueJunctionService
    {
        Task<bool> GolferExistsInLeague(int golferId, int leagueId);
        Task DeleteByGolferId(int golferId);
        void Add(GolferLeagueJunction golferLeagueJunction);
        Task SaveChanges();
    }
}
