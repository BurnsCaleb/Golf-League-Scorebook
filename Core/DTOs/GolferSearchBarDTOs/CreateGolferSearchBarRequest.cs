using Core.Models;

namespace Core.DTOs.GolferSearchBarDTOs
{
    public class CreateGolferSearchBarRequest
    {
        public required List<string> GolferNames { get; set;}
        public required League League { get; set; }
        public int? TeamId { get; set; }
    }
}
