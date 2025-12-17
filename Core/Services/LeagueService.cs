using Core.DTOs.LeagueDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepo;
        private readonly ICourseRepository _courseRepo;
        private readonly ILeagueSettingRepository _settingRepo;
        private readonly IMatchupRepository _matchupRepo;
        private readonly ITeamMatchupJunctionRepository _teamMatchupJunctionRepo;
        private readonly IGolferTeamJunctionRepository _golferTeamJunctionRepo;
        private readonly IGolferMatchupJunctionRepository _golferMatchupJunctionRepo;

        public LeagueService(ILeagueRepository leagueRepo, ILeagueSettingRepository leagueSettingRepo, ICourseRepository courseRepo, IMatchupRepository matchupRepo, ITeamMatchupJunctionRepository teamMatchupJunctionRepo, IGolferTeamJunctionRepository golferTeamJunctionRepo, IGolferMatchupJunctionRepository golferMatchupJunctionRepo)
        {
            _leagueRepo = leagueRepo;
            _courseRepo = courseRepo;
            _settingRepo = leagueSettingRepo;
            _matchupRepo = matchupRepo;
            _teamMatchupJunctionRepo = teamMatchupJunctionRepo;
            _golferTeamJunctionRepo = golferTeamJunctionRepo;
            _golferMatchupJunctionRepo = golferMatchupJunctionRepo;
        }

        public async Task<CreateLeagueResult> CreateLeague(CreateLeagueRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateLeagueRequest(request);
            if (validateErrors.Any())
            {
                return CreateLeagueResult.ValidationFailure(validateErrors);
            }

            // Check for duplicate league
            var existingLeague = await _leagueRepo.GetByName(request.LeagueName);
            if (existingLeague != null)
            {
                return CreateLeagueResult.Failure(
                $"A league by the name {request.LeagueName} already exists.");
            }

            // Create League
            var league = new League
            {
                LeagueName = request.LeagueName.Trim(),
                CourseId = request.CourseId,
                LeagueSettingsId = request.LeagueSettingsId,
            };

            // Save To Database
            try
            {
                _leagueRepo.Add(league);
                await _leagueRepo.SaveChanges();

                return CreateLeagueResult.Success(league);
            }
            catch (Exception ex)
            {
                return CreateLeagueResult.Failure($"Failed to create league: {ex.Message}");
            }
        }

        public async Task<CreateLeagueScheduleResult> CreateLeagueSchedule(List<Team> teams, int leagueId, int seasonId)
        {
            try
            {
                // Add a bye week for odd number of teams
                if (teams.Count % 2 == 1) teams.Add(new Team
                {
                    TeamName = "Bye Week",
                    LeagueId = leagueId,
                });

                int numRounds = teams.Count - 1;
                int opponentIndex = teams.Count / 2; // Plus team 1 index

                for (int week = 1; week <= numRounds; week++)
                {
                    for (int i = 0; i < teams.Count / 2; i++)
                    {
                        if (teams[i].TeamName != "Bye Week" && teams[opponentIndex + i].TeamName != "Bye Week") // Do not make a matchup for a bye week
                        {
                            // Create Matchup
                            Matchup newMatchup = new Matchup
                            {
                                MatchupName = $"{teams[i].TeamName} vs {teams[opponentIndex + i].TeamName}",
                                LeagueId = leagueId,
                                SeasonId = seasonId,
                                Week = week
                            };

                            // Save to database to get id for junction tables
                            _matchupRepo.Add(newMatchup);

                            await _matchupRepo.SaveChanges();

                            // Create team junction tables
                            var team1 = new TeamMatchupJunction
                            {
                                TeamId = teams[i].TeamId,
                                MatchupId = newMatchup.MatchupId,
                            };

                            var team2 = new TeamMatchupJunction
                            {
                                TeamId = teams[opponentIndex + i].TeamId,
                                MatchupId = newMatchup.MatchupId,
                            };

                            // Add matchup junctions to database
                            _teamMatchupJunctionRepo.Add(team1);
                            _teamMatchupJunctionRepo.Add(team2);

                            // Get golfersIds involved in this matchup
                            var golfers = await _golferTeamJunctionRepo.GetAllGolfersByTeam(new List<int> { team1.TeamId, team2.TeamId });

                            // Create junctions for every golfer
                            foreach (var golfer in golfers)
                            {
                                var golferJunction = new GolferMatchupJunction
                                {
                                    GolferId = golfer.GolferId,
                                    MatchupId = newMatchup.MatchupId
                                };

                                _golferMatchupJunctionRepo.Add(golferJunction);

                            }


                            await _leagueRepo.SaveChanges();
                        }
                    }

                    // Increment schedule
                    List<Team> newTeams = new List<Team>();

                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (i != 0)
                        {
                            newTeams.Add(teams[i - 1 == 0 ? teams.Count - 1 : i - 1]); // Grab last element if attempting to grab first element, else grab previous element.
                        }
                        else
                        {
                            newTeams.Add(teams[i]);
                        }
                    }

                    teams = newTeams;

                }

                return CreateLeagueScheduleResult.Success();
            }
            catch (Exception ex)
            {
                return CreateLeagueScheduleResult.Failure($"Failed creating schedule: {ex.Message}.");
            }
        }

        public async Task<CreateLeagueResult> UpdateLeague(CreateLeagueRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateLeagueRequest(request);
            if (validateErrors.Any())
            {
                return CreateLeagueResult.ValidationFailure(validateErrors);
            }

            // Grab league to update
            if (request.LeagueId == null)
            {
                return CreateLeagueResult.Failure(
                $"Could not find a league with the ID {request.LeagueId}.");
            }

            var leagueToUpdate = await _leagueRepo.GetById((int)request.LeagueId);

            if (leagueToUpdate == null)
            {
                return CreateLeagueResult.Failure(
                $"Could not find a league with the ID {request.LeagueId}.");
            }

            // Update League Values
            leagueToUpdate.LeagueName = request.LeagueName;
            leagueToUpdate.LeagueSettingsId = request.LeagueSettingsId;
            leagueToUpdate.CourseId = request.CourseId;

            // Save To Database
            try
            {
                _leagueRepo.Update(leagueToUpdate);
                await _leagueRepo.SaveChanges();

                return CreateLeagueResult.Success(leagueToUpdate);
            }
            catch (Exception ex)
            {
                return CreateLeagueResult.Failure($"Failed to update league: {ex.Message}");
            }
        }

        public async Task<List<League>> GetAll()
        {
            return await _leagueRepo.GetAll();
        }

        public async Task<League> GetById(int leagueId)
        {
            return await _leagueRepo.GetById(leagueId);
        }

        private async Task<List<string>> ValidateLeagueRequest(CreateLeagueRequest request)
        {
            List<string> errors = new List<string>();

            // Required fields
            if (string.IsNullOrEmpty(request.LeagueName))
            {
                errors.Add("League name is required.");
            }

            var course = await _courseRepo.GetById(request.CourseId);
            if (course == null)
            {
                errors.Add("Course is required");
            }

            var settings = await _settingRepo.GetById(request.LeagueSettingsId);
            if (settings == null)
            {
                errors.Add("League settings is required");
            }

            return errors;
        }

        public async Task<DeleteLeagueResult> Delete(int leagueId)
        {
            try
            {
                await _leagueRepo.Delete(leagueId);
                await _leagueRepo.SaveChanges();

                return DeleteLeagueResult.Success();
            }
            catch (Exception ex)
            {
                return DeleteLeagueResult.Failure(
                    $"Failed to delete league: {ex.Message}");
            }
        }

        public async Task<League> GetByName(string name)
        {
            return await _leagueRepo.GetByName(name);
        }
    }
}
