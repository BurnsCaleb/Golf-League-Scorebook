using Core.DTOs.GolferSearchBarDTOs;

namespace Core.Interfaces.Service
{
    public interface IGolferSearchBarService
    {
        Task<CreateGolferSearchBarResult> PerformGolferCheck(CreateGolferSearchBarRequest request);
    }
}
