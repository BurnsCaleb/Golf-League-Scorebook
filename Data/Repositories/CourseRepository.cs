using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
        }

        public async Task Delete(int courseId)
        {
            var courseToDelete = await GetById(courseId);
            if (courseToDelete != null)
            {
                _context.Courses.Remove(courseToDelete);
            }
        }

        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses
                .Include(c => c.Holes)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }

        public async Task<Course> GetById(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Holes)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<Course> GetByName(string name)
        {
            return await _context.Courses
                .Include(c => c.Holes)
                .FirstOrDefaultAsync(c => c.CourseName == name);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }
    }
}
