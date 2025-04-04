using System.Runtime.InteropServices.Marshalling;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Learn_Managment_System_Backend.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SignupController : Controller
    {
        //Se declara una variable con el servicio de userservice
        private readonly IUserService _UserService;

        //Se declara una variable con la interfaz de configuracion para acceder al appsettings
        // private readonly IConfiguration _Configuration;

        //Constructor
        public SignupController(IUserService userservice)
        {
            _UserService = userservice;
        }
        

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Signup(SignupDTO request)
        {
            try
            {
                
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }
                var UserExist = await _UserService.CheckIfUserExists(request);

                if (UserExist)
                {
                   return BadRequest(new { error = "El usuario existe"});
                }

                UserModel NewUser = new UserModel
                {
                    Id = ObjectId.GenerateNewId().ToString(), // Generar un nuevo Id de MongoDB
                    Name = request.Name,
                    LastName = request.LastName,
                    Age = request.Age,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Registration_Date = DateTime.UtcNow,
                    Token = _UserService.GenerateToken(request.UserName, ObjectId.GenerateNewId().ToString()),
                    User = request.UserName,
                    Password = Tools.HashPassword(request.Password),
                    RefreshToken = _UserService.GenerateRefreshToken(),
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(7)
                };

                var UserCreated = await _UserService.CreateUser(NewUser);

                UserDTO UserDataReturned = new UserDTO{
                    User = UserCreated.User,
                    Email = UserCreated.Email,
                    Token = UserCreated.Token,
                    Registration_Date = UserCreated.Registration_Date,
                    Name = UserCreated.Name,
                    LastName = UserCreated.LastName,
                    RefreshToken = UserCreated.RefreshToken,
                    RefreshTokenExpiry = UserCreated.RefreshTokenExpiry
                };

                return Ok(UserDataReturned);
            }
            catch (System.Exception error)
            {
                return BadRequest(new { error = error.Message });
            }

        }
    }
}