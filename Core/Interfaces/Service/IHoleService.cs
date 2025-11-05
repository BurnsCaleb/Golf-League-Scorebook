using Core.DTOs.HoleDTOs;

namespace Core.Interfaces.Service
{
    public interface IHoleService
    {
        CreateHoleResult CheckHole(CreateHoleRequest request);
    }
}
