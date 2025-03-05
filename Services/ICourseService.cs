using Learn_Managment_System_Backend.Models;
using MongoDB.Driver;

namespace Learn_Managment_System_Backend.Services
{
    public interface ICourseService
    {
        public Task<List<CourseModel>> GetAllCourses();
        public Task<List<CourseModel>> GetCourseById(string courseId, string userId);
    }

}