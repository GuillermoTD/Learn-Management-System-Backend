using Learn_Managment_System_Backend.Config;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Services;
using MongoDB.Driver;
using System.Threading.Tasks;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http.HttpResults;
using Learn_Managment_System_Backend.Controllers;

namespace Learn_Managment_System_Backend.Services
{
    public class CourseService : ICourseService
    {

        private readonly IMongoCollection<CourseModel> Collection;
        //mediante la interfaz IConfiguration de .net se puede acceder al appsettings
        private readonly IConfiguration _configuration;

        //Constructor
        public CourseService(IConfiguration configuration, DbConnection DBConnect)
        {
            Collection = DBConnect.GetCollection<CourseModel>("Courses");
            _configuration = configuration;
        }

        public async Task<List<CourseModel>> GetAllCourses()
        {
            /*Aqui se utiliza el objeto BsonDocument el cual es el que se encarga de hacer operaciones en los documentos de las colleciones.*/
            var courses = await Collection.Find(new BsonDocument()).ToListAsync();

            return courses;
        }


        public async Task<List<CourseModel>>GetCourseById(string courseId, string userId)
        {
            throw new NotImplementedException();
        }

    
    }
}