using Core.DTOs.GolferDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IGolferService
    {
        Task<CreateGolferResult> CreateGolfer(CreateGolferRequest request);
        Task<CreateGolferResult> UpdateGolfer(CreateGolferRequest request);
        Task<List<Golfer>> Search(string query);
        Task Delete(int golferId);
        Task<List<Golfer>> GetByLeague(int leagueId);
        Task<Golfer> GetByName(string name);
        Task<List<Golfer>> GetAll();
        Task<List<Golfer>> GetAvailableSubs(int leagueId);
        double GetCourseHandicap(Golfer golfer, Course course);
        Task<Golfer> GetById(int golferId);
        Task<GolferRoundViewModelRequest> GetViewModelData(GolferMatchupJunction junction);
        Task UpdateGolferHandicap(int golferId);
    }
}
