using System.Runtime.InteropServices.Marshalling;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Learn_Managment_System_Backend.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.IdentityModel.Tokens;


namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RefreshTokenController : Controller
    {

        private readonly IUserService _UserService;
        //Contructor
        public RefreshTokenController(UserService userService){
            this._UserService = userService;
        }


        [HttpPost("refresh")]
        public async Task<ActionResult>RefreshToken(){

            //Validamos si el refreshtoken es valido.
            if(!Request.Cookies.TryGetValue("refreshToken",out var refreshToken)){
                return Unauthorized(new {message = "no se encontró token autorizado"});
            }

            //Obtenemos el usuario a partir del refreshToken enviado
            UserModel User = await this._UserService.GenerateRefreshToken(refreshToken);
            
            //Validamos que dicho el refreshToken que el usuario tiene en base de datos no este expirado
            if(User == null || User.RefreshTokenExpiry < DateTime.UtcNow){
                return Unauthorized(new {message = "Token invalido o expirado"});
            }

            //Creamos un token y un refreshtoken nuevos
            string NewToken = _UserService.GenerateToken(User.Name, User.Email);
            string NewRefreshToken = _UserService.GenerateRefreshToken();

            //Creamos un token y refreshtoken nuevos
            User.Token = NewToken;
            User.RefreshToken = NewRefreshToken;
            User.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _UserService.UpdateUser();

            return Ok();
        }
    }
}