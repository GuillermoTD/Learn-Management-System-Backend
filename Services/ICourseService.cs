using Learn_Managment_System_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Learn_Managment_System_Backend.Services
{
    public interface ICourseService
    {
        public Task<List<CourseModel>> GetAllCourses(int pageNumber, int pageSize);
        public Task<ActionResult<CourseModel>> GetCourseById(string courseId);
        public Task<List<CourseModel>> SearchCoursesByTitle(string courseName, int pageNumber, int pageSize);
    }

}