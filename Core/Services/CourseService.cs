using Core.DTOs.CourseDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;

        public CourseService(ICourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<CreateCourseResult> CreateCourse(CreateCourseRequest request)
        {
            // Check if holes exist
            if (request.CourseHoles == null || request.CourseHoles.Count == 0)
            {
                return CreateCourseResult.Failure
                    ("Could not find holes for this course.");
            }

            // Get number of holes
            request.NumHoles = request.CourseHoles.Count;

            // Get course par from holes
            request.CoursePar = request.CourseHoles.Select(h => h.Par).Sum();

            // Create course
            var course = new Course
            {
                CourseName = request.CourseName,
                CourseLocation = request.CourseLocation,
                CourseRating = request.CourseRating,
                CourseSlope = request.CourseSlope,
                NumHoles = request.NumHoles,
                CoursePar = request.CoursePar,
            };

            try
            {
                ValidateHolesHandicap(request);
            } catch (Exception ex)
            {
                return CreateCourseResult.Failure(ex.Message);
            }

            // Add Holes to Course
            course.Holes = request.CourseHoles;

            // Save to Database
            try
            {
                _courseRepo.Add(course);
                await _courseRepo.SaveChanges();

                return CreateCourseResult.Success(course);
            } catch (Exception ex)
            {
                    return CreateCourseResult.Failure(
                        $"Failed to create course: {ex.Message}");
            }
        }

        public async Task<CreateCourseResult> InitialCheck(CreateCourseRequest request)
        {
            var validateErrors = ValidateCourseRequest(request);
            if (validateErrors.Any())
            {
                return CreateCourseResult.ValidationFailure(validateErrors);
            }

            // Check for course with same name
            var existingCourse = await _courseRepo.GetByName(request.CourseName);

            if (existingCourse != null)
            {
                return CreateCourseResult.Failure(
                    $"Course name: {request.CourseName} is already being used.");
            }

            // Check Num Hole Info
            if (!request.KnownWarning)
            {
                // Display warning if Number of Holes is not 9 or 18
                List<int> acceptableNumHoles = new List<int>() { 9, 18 };
                if (!acceptableNumHoles.Contains(request.NumHoles))
                {
                    request.KnownWarning = true;
                    return CreateCourseResult.Warning($"Are you sure {request.CourseName} has {request.NumHoles} holes?");
                }
            }

            return CreateCourseResult.InitialSuccess();
        }

        public async Task<CreateCourseResult> UpdateCourse(CreateCourseRequest request)
        {
            if (request.CourseId == null)
            {
                return CreateCourseResult.Failure("Course does not exist.");
            }

            var validateErrors = ValidateCourseRequest(request);
            if (validateErrors.Any())
            {
                return CreateCourseResult.ValidationFailure(validateErrors);
            }

            // Check if holes exist
            if (request.CourseHoles == null || request.CourseHoles.Count == 0)
            {
                return CreateCourseResult.Failure
                    ("Could not find holes for this course.");
            }

            // Grab course to update
            var courseToUpdate = await _courseRepo.GetById((int)request.CourseId);

            if (courseToUpdate == null)
            {
                return CreateCourseResult.Failure(
                    $"{request.CourseName} does not exist.");
            }

            // Update Values
            courseToUpdate.CourseName = request.CourseName;
            courseToUpdate.CourseLocation = request.CourseLocation;
            courseToUpdate.CourseRating = request.CourseRating;
            courseToUpdate.CourseSlope = request.CourseSlope;
            courseToUpdate.NumHoles = request.CourseHoles.Count;
            courseToUpdate.CoursePar = request.CourseHoles.Select(h => h.Par).Sum();
            courseToUpdate.Holes = request.CourseHoles;

            // Save to Database
            try
            {
                _courseRepo.Update(courseToUpdate);
                await _courseRepo.SaveChanges();

                return CreateCourseResult.Success(courseToUpdate);
            }
            catch (Exception ex)
            {
                return CreateCourseResult.Failure(
                    $"Failed to update course: {ex.Message}");
            }
        }

        public async Task<DeleteCourseResult> DeleteCourse(int courseId)
        {
            try
            {
                await _courseRepo.Delete(courseId);
                await _courseRepo.SaveChanges();

                return DeleteCourseResult.Success();
            }
            catch (Exception ex)
            {
                return DeleteCourseResult.Failure(
                    $"Failed to delete course: {ex.Message}");
            }
        }

        private List<string> ValidateCourseRequest(CreateCourseRequest request)
        {
            var errors = new List<string>();

            // Required Fields
            if (string.IsNullOrEmpty(request.CourseName))
            {
                errors.Add("Course name is required.");
            }

            if (string.IsNullOrEmpty(request.CourseLocation))
            {
                errors.Add("Course location is required.");
            }

            if (request.CourseSlope > 155 || request.CourseSlope < 55)
            {
                errors.Add("Course slope must be between 55 and 155.");
            }

            return errors;
        }

        private void ValidateHolesHandicap(CreateCourseRequest request)
        {
            // Check for holes with the same handicap
            var duplicateHandicaps = request.CourseHoles
                .GroupBy(h => h.Handicap)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateHandicaps.Any())
            {
                var duplicateValues = string.Join(", ", duplicateHandicaps);
                throw new Exception($"Duplicate handicap values: {duplicateValues}");
            }

            // Check if any handicap is not within the number of holes
            var handicapsTooLow = request.CourseHoles
                .Where(h => h.Handicap < 1)
                .Select(h => h.Handicap)
                .Any();

            if (handicapsTooLow)
            {
                throw new Exception("Handicap must be larger than 0.");
            }

            var handicapsTooHigh = request.CourseHoles
                .Where(h => h.Handicap > request.NumHoles)
                .Any();

            if (handicapsTooHigh)
            {
                throw new Exception($"Handicap cannot be larger than {request.NumHoles}");
            } 
        }

        public async Task<List<Course>> GetAll()
        {
            return await _courseRepo.GetAll();
        }

        public async Task<Course> GetById(int courseId)
        {
            return await _courseRepo.GetById(courseId);
        }
    }
}
