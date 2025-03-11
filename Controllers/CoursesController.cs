using System.Security.Claims;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        //Constructor
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCourses()
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Obtener el id del usaurio desde token JWT

                var courses = await _courseService.GetAllCourses(); //obtenemos los cursos del servicio de cursos

                if (string.IsNullOrEmpty(userId)) //Si es un string vacio implica que el usuario no existe
                {
                    Console.WriteLine("el usuario no ta logueao");
                    return Unauthorized("No se pudo obtener el ID del usuario.");
                }

                if (courses == null)
                {
                    Console.WriteLine("No se encontraron cursos.");
                    return NotFound("No se encontraron cursos.");
                }

                return Ok(courses);

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }


        }

        // [HttpGet]
        // [Authorize]
        // public async Task<IActionResult> SearchCourseByName(){
        // }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(string id)
        {
            Console.WriteLine(id);
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Obtener el id del usaurio desde token JWT
                

                if (string.IsNullOrEmpty(userId)) //Si es un string vacio implica que el usuario no existe
                {
                    Console.WriteLine("el usuario no ta logueao");
                    return Unauthorized("No se pudo obtener el ID del usuario.");
                }

                var course = await _courseService.GetCourseById(id); //obtenemos los cursos del servicio de cursos

                if (course == null) // Verificamos si el curso existe
                {

                    return NotFound(new { message = "No existe el curso buscado." });
                }

                return Ok(course);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "No existe el curso buscado", error = ex.Message });
            }

        }

        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> EnrollmentCourse(){}

        // [HttpPost]
        // [Authorize]
        // public async Task<IActionResult> PaymentCourse(){}

        // [HttpGet]
        // [Authorize]
        // public async Task<IActionResult> PaymentCourse(){}




    }
}