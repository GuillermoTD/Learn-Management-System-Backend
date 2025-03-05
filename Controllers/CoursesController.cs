using System.Security.Claims;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        //controlador
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //Obtener el id del usaurio desde token JWT

            var courses = _courseService.GetAllCourses(); //obtenemos los cursos del servicio de cursos

            if (string.IsNullOrEmpty(userId)) //Si es un string vacio implica que el usuario no existe
            {
                return Unauthorized("No se pudo obtener el ID del usuario.");
            }

            if (courses == null)
            {
                return NotFound("No se encontraron cursos.");
            }

            return Ok(courses);


        }
    }
}