using Core.DTOs.TeamDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepo;
        private readonly ILeagueService _leagueService;
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly IGolferLeagueJunctionService _golferLeagueService;

        public TeamService(ITeamRepository teamRepo, ILeagueService leagueService, IGolferTeamJunctionService golferTeamJunctionService, IGolferLeagueJunctionService golferLeagueService)
        {
            _teamRepo = teamRepo;
            _leagueService = leagueService;
            _golferTeamJunctionService = golferTeamJunctionService;
            _golferLeagueService = golferLeagueService;
        }

        public async Task<CreateTeamResult> CreateTeam(CreateTeamRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateTeamRequest(request);
            if (validateErrors.Any())
            {
                return CreateTeamResult.ValidationFailure(validateErrors);
            }

            // Check for duplicate team
            var teamsInLeague = await _teamRepo.GetTeamsByLeague(request.LeagueId);
            foreach (var teamInLeague in teamsInLeague)
            {
                if (teamInLeague.TeamName == request.TeamName)
                {
                    return CreateTeamResult.Failure(
                        $"Team name: {request.TeamName} already exists in that league.");
                }
            }

            // Create Team
            var team = new Team
            {
                TeamName = request.TeamName,
                LeagueId = request.LeagueId,
            };

            // Check if golfer already exists in league
            foreach(Golfer golfer in request.Golfers)
            {
                // Check for Golfer League conflicts
                var conflict = await _golferLeagueService.GolferExistsInLeague(golfer.GolferId, request.LeagueId);

                if (conflict)
                {
                    return CreateTeamResult.Failure(
                        $"{golfer.FullName} already exists in this league.");
                }
            }

            // Save to Database
            try
            {
                _teamRepo.Add(team);
                await _teamRepo.SaveChanges();

                // Create GolferTeamJunctions
                CreateGolferTeamJunction(request, team.TeamId);

                // Create GolferLeagueJunctions
                CreateGolferLeagueJunction(request);

                return CreateTeamResult.Success(team);
            }
            catch (Exception ex)
            {
                return CreateTeamResult.Failure(
                    $"Failed to create team: {ex.Message}");
            }
        }

        public async Task<CreateTeamResult> UpdateTeam(CreateTeamRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateTeamRequest(request);
            if (validateErrors.Any())
            {
                return CreateTeamResult.ValidationFailure(validateErrors);
            }

            // Grab League To Update
            if (request.TeamId == null)
            {
                return CreateTeamResult.Failure(
                    $"Could not find a team with the ID {request.TeamId}");
            }

            var teamToUpdate = await _teamRepo.GetById((int)request.TeamId);

            if (teamToUpdate == null)
            {
                return CreateTeamResult.Failure(
                    $"Could not find a team with the ID {request.TeamId}");
            }

            teamToUpdate.TeamName = request.TeamName;
            teamToUpdate.LeagueId = request.LeagueId;

            // Save to Database
            try
            {
                _teamRepo.Update(teamToUpdate);
                await _teamRepo.SaveChanges();

                // Replace GolferTeamJunctions
                CreateGolferTeamJunction(request, teamToUpdate.TeamId);

                // Replace GolferLeagueJunctions
                CreateGolferLeagueJunction(request);

                return CreateTeamResult.Success(teamToUpdate);
            }
            catch (Exception ex)
            {
                return CreateTeamResult.Failure(
                    $"Failed to update team: {ex.Message}");
            }
        }

        private async Task<List<string>> ValidateTeamRequest(CreateTeamRequest request)
        {
            var errors = new List<string>();

            // Required Fields
            if (string.IsNullOrEmpty(request.TeamName))
            {
                errors.Add("Team name is required.");
            }

            // League Exists
            var league = await _leagueService.GetById(request.LeagueId);
            if (league == null)
            {
                errors.Add("League is required");
            }

            return errors;
        }

        private void CreateGolferTeamJunction(CreateTeamRequest request, int teamId)
        {
            // Delete any instance with this TeamId
            _golferTeamJunctionService.DeleteByTeamId(teamId);

            foreach (Golfer golfer in request.Golfers)
            {
                // Create new junction
                var junction = new GolferTeamJunction
                {
                    GolferId = golfer.GolferId,
                    TeamId = teamId
                };

                _golferTeamJunctionService.Add(junction);
            }

            _golferTeamJunctionService.SaveChanges();
        }

        private void CreateGolferLeagueJunction(CreateTeamRequest request)
        {
            foreach (Golfer golfer in request.Golfers)
            {
                // Delete any instance with this GolferId
                _golferLeagueService.DeleteByGolferId(golfer.GolferId);

                // Create new junction
                var junction = new GolferLeagueJunction
                {
                    GolferId = golfer.GolferId,
                    LeagueId = request.LeagueId
                };

                _golferLeagueService.Add(junction);
            }

            _golferLeagueService.SaveChanges();
        }

        public async Task<List<Team>> GetTeamsByLeague(int leagueId)
        {
            return await _teamRepo.GetTeamsByLeague(leagueId);
        }

        public async Task<DeleteTeamResult> Delete(int teamId)
        {
            try
            {
                await _teamRepo.Delete(teamId);
                await _teamRepo.SaveChanges();

                return DeleteTeamResult.Success();
            }
            catch (Exception ex)
            {
                return DeleteTeamResult.Failure(
                    $"Failed to delete team: {ex.Message}");
            }
        }

        public async Task<List<Team>> GetAll()
        {
            return await _teamRepo.GetAll();
        }

        public async Task<Team> GetByGolferLeague(int golferId, int leagueId)
        {
            return await _teamRepo.GetByGolferLeague(golferId, leagueId);
        }
    }
}
