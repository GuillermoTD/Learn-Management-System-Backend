using System.Runtime.InteropServices.Marshalling;
using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.Services;
using Learn_Managment_System_Backend.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Learn_Managment_System_Backend.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SignupController : Controller{
        //Se declara una variable con el servicio de userservice
        private readonly IUserService _UserService;

        //Se declara una variable con la interfaz de configuracion para acceder al appsettings
        // private readonly IConfiguration _Configuration;

        //Constructor
        public SignupController(IUserService userservice){
            _UserService = userservice;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserModel>> Signup(SignupDTO request){
          
            var UserExist = await _UserService.CheckIfUserExists(request);
            
            if(UserExist == null){
                throw new Exception("invalid credentials");
            }
            
            Tools.CheckIfPasswordIsValid()



            

        }
    }
}