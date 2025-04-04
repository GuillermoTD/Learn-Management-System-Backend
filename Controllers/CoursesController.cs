using System.Net;
using System.Security.Claims;
using System.Web;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> GetCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Los numeros de pagina y su tama�o deben ser mayores a 0");
            }
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Obtener el id del usaurio desde token JWT

                var courses = await _courseService.GetAllCourses(pageNumber, pageSize); //obtenemos los cursos del servicio de cursos

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

        [HttpGet("search/{courseTitle}")]
        [Authorize]
        public async Task<ActionResult<List<CourseModel>>> SearchByTitle(string courseTitle, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Los numeros de pagina y su tamaño deben ser mayores a 0");
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Obtener el id del usaurio desde token JWT

                //Decodificamos el parametro enviado de parte del front con caracteres especiales
                var decodedTitle = WebUtility.UrlDecode(courseTitle);

                Console.WriteLine(decodedTitle);

                var Courses = await _courseService.SearchCoursesByTitle(decodedTitle, pageSize, pageNumber); //obtenemos los cursos del servicio de cursos


                if (string.IsNullOrEmpty(userId)) //Si es un string vacio implica que el usuario no existe
                {
                    Console.WriteLine("el usuario no ta logueao");
                    return Unauthorized("No se pudo obtener el ID del usuario.");
                }

                if (Courses == null)
                {
                    Console.WriteLine("No se encontraron cursos.");
                    return NotFound("No se encontraron cursos.");
                }
             
                return Ok(Courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Curso no encontrado",
                    error = ex.Message
                });
            }

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(string id)
        {
            Console.WriteLine(id);
            try
            {
                //Obtener el id del usaurio desde token JWT
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
        // public async Task<IActionResult> CreateReview(){}
    }
}