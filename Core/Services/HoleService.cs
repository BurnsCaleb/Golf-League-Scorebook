using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;

namespace Core.Services
{
    public class HoleService : IHoleService
    {
        public CreateHoleResult CheckHole(CreateHoleRequest request)
        {
            var validateErrors = ValidateHoleRequest(request);

            if (validateErrors.Any())
            {
                return CreateHoleResult.ValidationFailure(validateErrors);
            }

            if (!request.KnownWarning)
            {
                // Check par value
                var acceptablePar = new List<int>() { 3, 4, 5 };
                if (!acceptablePar.Contains(request.Par))
                {
                    return CreateHoleResult.Warning($"Are you sure hole {request.HoleNum} is a par {request.Par}?");
                }
            }

            return CreateHoleResult.Success();
        }

        private List<string> ValidateHoleRequest(CreateHoleRequest request)
        {
            var errors = new List<string>();

            if (request.Par < 1)
            {
                errors.Add("Hole par must be greater than 0.");
            }

            if (request.Distance < 1)
            {
                errors.Add("Hole distance must be greater than 0.");
            }

            if (request.Handicap < 1)
            {
                errors.Add("Handicap must be greater than 0.");
            }

            return errors;
        }
    }
}
