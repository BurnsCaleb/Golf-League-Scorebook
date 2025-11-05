using Core.DTOs.CourseDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ICourseService
    {
        Task<CreateCourseResult> InitialCheck(CreateCourseRequest request);
        Task<CreateCourseResult> CreateCourse(CreateCourseRequest request);
        Task<CreateCourseResult> UpdateCourse(CreateCourseRequest request);
        Task<DeleteCourseResult> DeleteCourse(int courseId);
        Task<List<Course>> GetAll();
        Task<Course> GetById(int courseId);
    }
}
