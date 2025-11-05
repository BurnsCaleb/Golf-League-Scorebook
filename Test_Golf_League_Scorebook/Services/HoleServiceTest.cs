using Core.DTOs.HoleDTOs;
using Core.Interfaces.Service;
using Core.Services;

namespace Test_Golf_League_Scorebook.Services
{
    public class HoleServiceTest
    {
        private readonly IHoleService _holeService;

        public HoleServiceTest()
        {
            _holeService = new HoleService();
        }

        [Fact]
        public void CheckHole_GoodValues_ReturnsSuccessful()
        {
            // Arrange 
            var request = new CreateHoleRequest
            {
                HoleNum = 1,
                Par = 4,
                Distance = 350,
                Handicap = 1
            };

            // Act
            var result = _holeService.CheckHole(request);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void CheckHole_OddPar_ReturnsWarning()
        {
            // Arrange
            var request = new CreateHoleRequest
            {
                HoleNum = 1,
                Par = 6,
                Distance = 350,
                Handicap = 1
            };

            // Act
            var result = _holeService.CheckHole(request);

            // Assert
            Assert.True(result.IsWarning);
            Assert.Equal("Are you sure hole 1 is a par 6?", result.ErrorMessage);
        }

        [Fact]
        public void CheckHole_BadValues_ReturnsFailure()
        {
            // Arrange
            var request = new CreateHoleRequest
            {
                HoleNum = 1,
                Par = 0,
                Distance = 0,
                Handicap = 0
            };

            var expectedOutcome = new List<string>
            {
                "Hole par must be greater than 0.",
                "Hole distance must be greater than 0.",
                "Handicap must be greater than 0."
            };

            // Act
            var result = _holeService.CheckHole(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedOutcome, result.ValidationErrors);
        }
    }
}
