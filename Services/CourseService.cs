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

        public async Task<CourseModel> GetCourseById(string courseId)
        {
            /*Validamos que el formato del id es correcto, tomando el string de courseId
            creamos un nuevo objeto de tipo ObjectId con el cual creamos una estructura como esta "id:ObjectId(id)"*/
            if (!ObjectId.TryParse(courseId, out var objectId))
            {
                Console.WriteLine($"Formato de ID inválido: {courseId}");
                return null;
            }

            var filter = Builders<CourseModel>.Filter.Eq("_id", objectId);

            Console.WriteLine(filter);

            var query = await Collection.Find(filter).FirstOrDefaultAsync();

            Console.WriteLine("hola" + query + "parece que no esta");

            return query;
        }

        public async Task<CourseModel?> SearchCoursesByTitle(string title)
        {
            var filter = Builders<CourseModel>.Filter.Eq(c => c.Title, title);

            var query = await Collection.Find(filter).FirstOrDefaultAsync();

            return query;
        }
    }
}