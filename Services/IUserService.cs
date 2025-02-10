using Learn_Managment_System_Backend.DTO;
using Learn_Managment_System_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Learn_Managment_System_Backend.Services
{
  public interface IUserService
  {
    Task<UserDTO> CheckIfUserExists(LoginDTO credentials);
    Task<bool> CheckIfUserExists(SignupDTO credentials);

    string GenerateToken(string username, string email);
    Task<UserModel> CreateUser(UserModel NewUser);
  }
}
