using Core.DTOs.GolferDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class GolferService : IGolferService
    {
        private readonly IGolferRepository _golferRepo;
        private readonly IRoundService _roundService;
        private readonly IMatchupRepository _matchupRepo;

        public GolferService(IGolferRepository golferRepo, IRoundService roundService, IMatchupRepository matchupRepo)
        {
            _golferRepo = golferRepo;
            _matchupRepo = matchupRepo;
            _roundService = roundService;
        }

        public async Task<CreateGolferResult> CreateGolfer(CreateGolferRequest request)
        {
            // Validate Form
            var validateErrors = ValidateGolferRequest(request);
            if (validateErrors.Any())
            {
                return CreateGolferResult.ValidationFailure(validateErrors);
            }

            // Check for duplicate golfer
            var existingGolfer = await _golferRepo.GetByName($"{request.FirstName} {request.LastName}");
            if (existingGolfer != null)
            {
                return CreateGolferResult.Failure(
                    $"A golfer named {request.FirstName} {request.LastName} already exists");
            }


            // Create Golfer
            var golfer = new Golfer
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Handicap = request.Handicap,
            };

            // Save Golfer to Database
            try
            {
                _golferRepo.Add(golfer);
                await _golferRepo.SaveChanges();

                return CreateGolferResult.Success(golfer);
            }
            catch (Exception ex)
            {
                return CreateGolferResult.Failure($"Failed to create golfer: {ex.Message}");
            }
        }

        public async Task<CreateGolferResult> UpdateGolfer(CreateGolferRequest request)
        {
            // Validate Form
            var validateErrors = ValidateGolferRequest(request);
            if (validateErrors.Any())
            {
                return CreateGolferResult.ValidationFailure(validateErrors);
            }

            // Grab golfer to update
            var golferToUpdate = await _golferRepo.GetById((int)request.GolferId);

            if (golferToUpdate == null)
            {
                return CreateGolferResult.Failure(
                    $"Could not find golfer with the ID {request.GolferId}." );
            }

            golferToUpdate.FirstName = request.FirstName.Trim();
            golferToUpdate.LastName = request.LastName.Trim();
            golferToUpdate.Handicap = request.Handicap;

            try
            {
                _golferRepo.Update(golferToUpdate);
                await _golferRepo.SaveChanges();

                return CreateGolferResult.Success(golferToUpdate);
            }
            catch (Exception ex)
            {
                return CreateGolferResult.Failure($"Failed to update golfer: {ex.Message}");
            }
        }

        public async Task<List<Golfer>> Search(string query)
        {
            return await _golferRepo.Search(query);
        }

        public async Task Delete(int golferId)
        {
            await _golferRepo.Delete(golferId);
            await _golferRepo.SaveChanges();
        }

        private List<string> ValidateGolferRequest(CreateGolferRequest request)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                errors.Add("Last name is required.");

            // Length validation
            if (request.FirstName?.Length > 25)
                errors.Add("First name cannot exceed 25 characters.");

            if (request.LastName?.Length > 25)
                errors.Add("Last name cannot exceed 25 characters.");

            // Handicap validation
            if (request.Handicap > 54)
                errors.Add("Handicap cannot exceed 54.");

            return errors;
        }

        public async Task<List<Golfer>> GetByLeague(int leagueId)
        {
            return await _golferRepo.GetByLeague(leagueId);
        }

        public async Task<Golfer> GetByName(string name)
        {
            return await _golferRepo.GetByName(name);
        }

        public async Task<List<Golfer>> GetAll()
        {
            return await _golferRepo.GetAll();
        }

        public async Task<List<Golfer>> GetAvailableSubs(int leagueId)
        {
            return await _golferRepo.GetAvailableSubs(leagueId);
        }

        /// <summary>
        /// Returns the strokes received for a course.
        /// </summary>
        /// <param name="golfer"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        public double GetCourseHandicap(Golfer golfer, Course course)
        {
            // Get Golfer Handicap
            double handicap = golfer.Handicap;

            if (course.NumHoles == 9)
            {
                handicap = handicap / 2;
            }

            // Get Course Slope 
            double slope = course.CourseSlope;

            // Get Course Rating
            double rating = course.CourseRating;

            // Get Course Par
            double par = course.CoursePar;

            // Const divider
            const int divider = 113;

            return handicap * (slope / divider) + (rating - par);
        }

        public async Task<Golfer> GetById(int golferId)
        {
            return await _golferRepo.GetById(golferId);
        }

        public async Task<GolferRoundViewModelRequest> GetViewModelData(GolferMatchupJunction junction)
        {
            // Get Golfer
            Golfer golfer = await _golferRepo.GetById(junction.GolferId);

            // Get gross score
            int grossScore = await _roundService.GetGrossScore(golfer.GolferId, junction.MatchupId);

            // get net score
            int netScore = await _roundService.GetNetScore(golfer.GolferId, junction.MatchupId);

            // get matchup name
            string matchupName = await _matchupRepo.GetMatchupName(junction.MatchupId);

            var request = new GolferRoundViewModelRequest
            {
                GolferMatchupJunction = junction,
                Golfer = golfer,
                GrossScore = grossScore,
                NetScore = netScore,
                MatchupName = matchupName
            };

            return request;
        }

        /// <summary>
        /// Grabs available rounds from database and updates handicap.
        /// </summary>
        /// <returns></returns>
        public async Task UpdateGolferHandicap(int golferId)
        {
            // Get up to last 20 score differentials
            var grossScores = await _roundService.GetLastTwentyRounds(golferId);

            int differential = 0;
            int adjustment = 0;

            // Grab 
            switch (grossScores.Count)
            {
                case 3 * 18:
                    differential = 1;
                    adjustment = 2;
                    break;

                case 4 * 18:
                    differential = 1;
                    adjustment = 1;
                    break;

                case 5 * 18:
                    differential = 1;
                    adjustment = 0;
                    break;

                case 6 * 18:
                    differential = 2;
                    adjustment = 1;
                    break;

                case 7 * 18:
                case 8 * 18:
                    differential = 2;
                    adjustment = 0;
                    break;

                case 9 * 18:
                case 10 * 18:
                case 11 * 18:
                    differential = 3;
                    adjustment = 0;
                    break;

                case 12 * 18:
                case 13 * 18:
                case 14 * 18:
                    differential = 4;
                    adjustment = 0;
                    break;

                case 15 * 18:
                case 16 * 18:
                    differential = 5;
                    adjustment = 0;
                    break;

                case 17 * 18:
                case 18 * 18:
                    differential = 6;
                    adjustment = 0;
                    break;

                case 19 * 18:
                    differential = 7;
                    adjustment = 0;
                    break;

                case 20 * 18:
                    differential = 8;
                    adjustment = 0;
                    break;

                default:
                    differential = 0;
                    adjustment = 0;
                    break;
            }
            
            // Not enough scores to process
            // Keep user entered handicap until enough data is found
            if (differential == 0)
            {
                return;
            }

            var usableScores = grossScores.Take(differential);

            var handicapIndex = (usableScores.Average() * 0.96) - adjustment;

            // Get golfer
            var golfer = await _golferRepo.GetById(golferId);
            golfer.Handicap = handicapIndex;

            // Save to database
            _golferRepo.Update(golfer);
        }
    }
}
