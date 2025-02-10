using Learn_Managment_System_Backend.Config;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Windows.Markup;
using Learn_Managment_System_Backend.Services;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.DTO;

namespace Learn_Managment_System_Backend.Services
{
    public class UserService : IUserService
    {
         private readonly DbConnection DBConnect = new DbConnection();
        private readonly IMongoCollection<UserModel> Collection;

        private ILogger<UserService> log;

        public string Token { get; internal set; }
        public string User { get; internal set; }

        //Constructor
        public UserService(ILogger<UserService> log)
        {
            Collection = DBConnect.GetCollection<UserModel>("userapi");
            this.log = log;
        }

        public async Task<UserModel> CheckIfUserExists (LoginDTO login)
        {
            try
            {
                var filter = Builders<UserModel>.Filter.Eq(u => u.User, login.User);
                    // & Builders<UserModel>.Filter.Eq(u => u.Password, login.Password);

                //Aqui hacemos uso de la definicion filter y traemos el primer elemento que haga match
                return await Collection.FindAsync(filter).Result.FirstAsync();
            }
            catch (System.Exception error)
            {
                // log.LogError("Error:" + error.ToString());
                throw new Exception("Usuario incorrecto" + "\n" + error.Message);
            }
        }

        public Task<UserModel> CheckIfUserExists (SignupDTO request)
        {
            throw new NotImplementedException();
        }

        public string GenerateJwtToken(){
            
        }
    }
}