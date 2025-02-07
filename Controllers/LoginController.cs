using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        // public async Task<ActionResult> Login(LoginDTO credentials)
        // {
        //     try
        //     {
        //         UserModel User;
        //         User = await UserAuthenticationAsync(credentials);

        //         User = GenerateTokenJWT(User);
        //     }
        //     catch (System.Exception error)
        //     {
        //         Console.WriteLine("Usuario y/o Contraseña incorrecta", error.Message);
        //         return NotFound();
        //     }

        //     return User;
        // }

        private UserModel GenerateTokenJWT(UserModel user)
        {
            throw new NotImplementedException();
        }

        private async Task<UserModel> UserAuthenticationAsync(LoginDTO credentials)
        {
            UserModel User = await _UserService.GetUser(credentials);




            return User;
        }
    }
}