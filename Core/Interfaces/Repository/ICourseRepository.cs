using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ICourseRepository
    {
        Task<Course> GetById(int courseId);
        Task<Course> GetByName(string name);
        Task<List<Course>> GetAll();
        void Add(Course course);
        void Update(Course course);
        Task Delete(int courseId);
        Task SaveChanges();
    }
}
