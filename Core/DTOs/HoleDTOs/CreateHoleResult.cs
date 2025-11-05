namespace Core.DTOs.HoleDTOs
{
    public class CreateHoleResult
    {
        public bool IsSuccess { get; set; } = false;
        public bool IsWarning { get; set; } = false;
        public bool KnownWarning { get; set; } = false;
        public string ErrorMessage { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();

        // Factory methods for easy creation
        public static CreateHoleResult Success()
        {
            return new CreateHoleResult
            {
                IsSuccess = true,
            };
        }

        public static CreateHoleResult Warning(string warning)
        {
            return new CreateHoleResult
            {
                IsWarning = true,
                ErrorMessage = warning,
                KnownWarning = true
            };
        }

        public static CreateHoleResult Failure(string error)
        {
            return new CreateHoleResult
            {
                ErrorMessage = error
            };
        }

        public static CreateHoleResult ValidationFailure(List<string> errors)
        {
            return new CreateHoleResult
            {
                ErrorMessage = "Validation failed",
                ValidationErrors = errors
            };
        }
    }
}
