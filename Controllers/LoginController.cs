using Learn_Managment_System_Backend.Config;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ZstdSharp;


namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class LoginController : Controller
    {
        //Se declara una variable con el servicio de userservice
        private readonly IUserService _UserService;

        //Se declara una variable con la interfaz de configuracion para acceder al appsettings
        private readonly IConfiguration _Configuration;


        //Constructor
        public LoginController(IUserService userService, IConfiguration configuration)
        {
            this._UserService = userService;
            this._Configuration = configuration;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO request)
        {
            try
            {
                Console.WriteLine("Se valida que existe el usuario");
                var User = await UserAuthenticationAsync(request);

                Console.WriteLine("Se genera el token");
                string Token = _UserService.GenerateToken(request.UserName, ObjectId.GenerateNewId().ToString());

                return Ok(User);
            }
            catch (System.Exception error)
            {
                Console.WriteLine("Usuario y/o Contraseña incorrecta", error.Message);
                return NotFound();
            }

        }


        private async Task<UserDTO> UserAuthenticationAsync(LoginDTO request)
        {
            try
            {
                // Verifica si el usuario existe
                UserModel UserExist = await _UserService.CheckIfUserExists(request);

                // Si el usuario no existe, retorna Unauthorized
                if (UserExist == null)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }

                // Verifica si la contraseña es válida
                bool IsValidPassword = Tools.CheckIfPasswordIsValid(request.Password, UserExist.Password);

                if (IsValidPassword == false)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }

                // Genera el token
                string Token = _UserService.GenerateToken(request.UserName, ObjectId.GenerateNewId().ToString());

                // Crea el DTO con la información del usuario
                UserDTO UserDataReturned = new UserDTO
                {
                    User = UserExist.User,
                    Name = UserExist.Name,
                    LastName = UserExist.LastName,
                    Email = UserExist.Email,
                    Registration_Date = UserExist.Registration_Date,
                    Token = Token, // El token generado se asigna aquí
                };

                // Devuelve la respuesta con el DTO
                return UserDataReturned;
            }
            catch (System.Exception error)
            {
                Console.WriteLine(error);
                throw new Exception("Error interno del servidor");
            }
        }

    }
}