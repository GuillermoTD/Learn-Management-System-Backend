using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Learn_Managment_System_Backend.Services
{
     public interface IUserService{
       Task<UserModel>CheckIfUserExists (LoginDTO credentials);
       Task<UserModel>CheckIfUserExists (SignupDTO credentials);
    }
}
