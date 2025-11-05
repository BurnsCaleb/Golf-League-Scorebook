using Core.DTOs.CourseDTOs;
using Core.Interfaces.Repository;
using Core.Models;
using Core.Services;
using Moq;

namespace Test_Golf_League_Scorebook.Services
{
    public class CourseServiceTest
    {

        private readonly Mock<ICourseRepository> _mockCourseRepo;
        private readonly CourseService _courseService;

        public CourseServiceTest()
        {
            _mockCourseRepo = new Mock<ICourseRepository>();
            _courseService = new CourseService(_mockCourseRepo.Object);
        }

        #region CreateCourse Method Tests

        [Fact]
        public async Task CreateCourse_GoodValues_SaveSuccessful()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 3
                    }
                }
            };

            // Act
            var result = await _courseService.CreateCourse(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Course);
            Assert.Equal("Augusta National", result.Course.CourseName);
            Assert.Equal(3, result.Course.NumHoles);
            Assert.Equal(12, result.Course.CoursePar);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Once);
            _mockCourseRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task CreateCourse_NullHoles_ReturnsFailure()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = null
            };

            // Act
            var result = await _courseService.CreateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Could not find holes for this course.", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Never);
        }

        [Fact]
        public async Task CreateCourse_DuplicateHandicap_ReturnsFailure()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 1
                    }
                }
            };

            // Act
            var result = await _courseService.CreateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Duplicate handicap values: 1", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Never);
        }

        [Fact]
        public async Task CreateCourse_HighHandicap_ReturnsFailure()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 4
                    }
                }
            };

            // Act
            var result = await _courseService.CreateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Handicap cannot be larger than 3", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Never);
        }

        [Fact]
        public async Task CreateCourse_LowHandicap_ReturnsFailure()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 0
                    }
                }
            };

            // Act
            var result = await _courseService.CreateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Handicap must be larger than 0.", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Never);
        }

        #endregion

        #region InitialCheck Method Tests

        [Fact]
        public async Task InitialCheck_GoodValues_ReturnsSuccess()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 9
            };

            // Act
            var result = await _courseService.InitialCheck(request);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task InitialCheck_LowNumHoles_ReturnsWarning()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3
            };

            // Act
            var result = await _courseService.InitialCheck(request);

            // Assert
            Assert.True(result.IsWarning);
            Assert.Equal($"Are you sure {request.CourseName} has {request.NumHoles} holes?", result.ErrorMessage);
        }

        [Fact]
        public async Task InitialCheck_InvalidValues_ReturnsFailure()
        {
            // Arrange
            // Create Request
            var request = new CreateCourseRequest
            {
                CourseName = string.Empty,
                CourseLocation = string.Empty,
                CourseRating = 50,
                CourseSlope = 160,
                NumHoles = 3,
                CoursePar = 12,
            };

            // Act
            var result = await _courseService.InitialCheck(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(new List<string> { "Course name is required.", "Course location is required.", "Course slope must be between 55 and 155." }, result.ValidationErrors);
        }

        [Fact] 
        public async Task InitialCheck_DuplicateCourse_ReturnsFailure()
        {
            // Arrange
            var existingCourse = new Course
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                Holes = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 3
                    }
                }
            };

            var request = new CreateCourseRequest
            {
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 9
            };

            _mockCourseRepo.Setup(r => r.GetByName("Augusta National"))
                .ReturnsAsync(existingCourse);

            // Act
            var result = await _courseService.InitialCheck(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Course name: Augusta National is already being used.", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.Add(It.IsAny<Course>()), Times.Never);
            _mockCourseRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        #endregion

        #region UpdateCourse Method Tests

        [Fact]
        public async Task UpdateCourse_GoodValues_ReturnsSuccessful()
        {
            // Arrange
            var existingCourse = new Course
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                Holes = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 3
                    }
                }
            };

            var request = new CreateCourseRequest
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Athens Georgia",
                CourseRating = 55,
                CourseSlope = 130,
                NumHoles = 3,
                CoursePar = 11,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 3,
                        Distance = 110,
                        Handicap = 3
                    }
                }
            };

            var requestAsCourse = new Course
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Athens Georgia",
                CourseRating = 55,
                CourseSlope = 130,
                NumHoles = 3,
                CoursePar = 11,
                Holes = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 3,
                        Distance = 110,
                        Handicap = 3
                    }
                }
            };

            _mockCourseRepo.Setup(r => r.GetById(1))
                .ReturnsAsync(existingCourse);

            // Act
            var result = await _courseService.UpdateCourse(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Athens Georgia", result.Course.CourseLocation);
            Assert.Equal(11, result.Course.CoursePar);
            _mockCourseRepo.Verify(r => r.SaveChanges(), Times.Once());
        }

        [Fact]
        public async Task UpdateCourse_NoCourse_ReturnsFailure()
        {
            // Arrange
            var request = new CreateCourseRequest
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                CourseHoles = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 3
                    }
                }
            };

            _mockCourseRepo.Setup(r => r.GetById(1))
                .ReturnsAsync((Course)null);

            // Act 
            var result = await _courseService.UpdateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Augusta National does not exist.", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.SaveChanges(), Times.Never());
        }

        [Fact]
        public async Task UpdateCourse_NoHoles_ReturnsFailure()
        {
            // Arrange
            var existingCourse = new Course
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Augusta Georgia",
                CourseRating = 50,
                CourseSlope = 125,
                NumHoles = 3,
                CoursePar = 12,
                Holes = new List<Hole>
                {
                    new Hole {
                        HoleNum = 1,
                        Par = 4,
                        Distance = 300,
                        Handicap = 1
                    },
                    new Hole {
                        HoleNum = 2,
                        Par = 4,
                        Distance = 310,
                        Handicap = 2
                    },
                    new Hole {
                        HoleNum = 3,
                        Par = 4,
                        Distance = 320,
                        Handicap = 3
                    }
                }
            };

            var request = new CreateCourseRequest
            {
                CourseId = 1,
                CourseName = "Augusta National",
                CourseLocation = "Athens Georgia",
                CourseRating = 55,
                CourseSlope = 130,
                NumHoles = 3,
                CoursePar = 11,
            };

            _mockCourseRepo.Setup(r => r.GetById(1))
                .ReturnsAsync(existingCourse);

            // Act
            var result = await _courseService.UpdateCourse(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Could not find holes for this course.", result.ErrorMessage);
            _mockCourseRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        #endregion
    }
}
