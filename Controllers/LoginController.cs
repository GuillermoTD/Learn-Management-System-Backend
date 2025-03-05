using Learn_Managment_System_Backend.Config;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Learn_Managment_System_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService _UserService;
        // private readonly IConfiguration _Configuration;

        public LoginController(IUserService userService, IConfiguration configuration)
        {
            this._UserService = userService;
            // this._Configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO request)
        {
            try
            {
                Console.WriteLine("Se valida que existe el usuario");
                var UserExist = await UserAuthenticationAsync(request);

                if (UserExist == null)
                {
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
                }

                Console.WriteLine("Se genera el token");
                return Ok(UserExist);
            }
            catch (Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
        }

        [HttpOptions]
        public IActionResult Preflight()
        {
            Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
            Response.Headers.Append("Access-Control-Allow-Methods", "POST, OPTIONS");
            Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization,Access-Control-Allow-Origin");
            Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            return Ok();
        }

        private async Task<UserDTO?> UserAuthenticationAsync(LoginDTO request)
        {
            try
            {
                UserModel UserExist = await _UserService.CheckIfUserExists(request);

                if (UserExist == null)
                {
                    return null;
                }

                bool IsValidPassword = Tools.CheckIfPasswordIsValid(request.Password, UserExist.Password);

                if (!IsValidPassword)
                {
                    return null;
                }

                string Token = _UserService.GenerateToken(request.UserName, ObjectId.GenerateNewId().ToString());
                string refreshToken = _UserService.GenerateRefreshToken();

                return new UserDTO
                {
                    User = UserExist.User,
                    Name = UserExist.Name,
                    LastName = UserExist.LastName,
                    Email = UserExist.Email,
                    Registration_Date = UserExist.Registration_Date,
                    Token = Token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
                };
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }
    }
}
