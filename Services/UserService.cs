using Learn_Managment_System_Backend.Config;
using Learn_Managment_System_Backend.Models;
using Learn_Managment_System_Backend.DTO;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;


namespace Learn_Managment_System_Backend.Services
{
    public class UserService : IUserService
    {
        // private readonly DbConnection DBConnect = new DbConnection();
        private readonly IMongoCollection<UserModel> Collection;

        private readonly IConfiguration _configuration;


        public required string Token { get; set; }
        public required string User { get; set; }

        //Constructor
        public UserService(IConfiguration configuration, DbConnection DBConnect)
        {
            Collection = DBConnect.GetCollection<UserModel>("Users");
            _configuration = configuration;
        }

        public async Task<UserModel> CheckIfUserExists(LoginDTO request)
        {
            try
            {
                var filter = Builders<UserModel>.Filter.Eq(u => u.User, request.UserName);
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

        public async Task<bool> CheckIfUserExists(SignupDTO request)
        {

            var filter = Builders<UserModel>.Filter.Eq(u => u.User, request.UserName);
            // & Builders<UserModel>.Filter.Eq(u => u.Password, login.Password);

            long UserExist = await Collection.CountDocumentsAsync(filter);

            //si encuentro elementos que correspondan con el username entonces digo que existe y devuelvo true
            if (UserExist > 0)
            {
                return true;
            }

            //Si no encuentro elementos entonces devuelvo false
            return false;

        }

        public string GenerateToken(string username, string userId)
        {
            try
            {
                /*Seteamos el appsettings para poder obtener la propiedad JWTSettings
           y de ahi sacar la configuracion de de JWT*/
                var JWTSettingsFromAppSettings = _configuration.GetSection("JwtSettings");

                /*Tomamos el secrekey de JWTSettings*/
                /*Como nota adicional, se utiliza el operador llamado null-forgiving que se representa asi !,
                 este permite decirle al compilador que estamos seguros de que el valos que estamos manejando no será nulo*/
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettingsFromAppSettings["SecretKey"]!));

                /*Se crean credenciales necesarias para firmar el token*/
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                /*Estos son campos que se utilizaran en la generacion del token, los mismos son tomados del appsettings*/
                var issuer = JWTSettingsFromAppSettings["Issuer"];
                var audience = JWTSettingsFromAppSettings["Audience"];

                /*Se crean los claims que se insertaran en el token para su posterior generacion*/
                var claims = new[]{
                new Claim(ClaimTypes.Name, username), //Nombre del usuario
                new Claim(ClaimTypes.NameIdentifier, userId), //
                new Claim("id", userId), //ID del usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //Identificador unico del token para evitar que se repita el token en diferentes usuarios
            };

                /*Se genera el token*/
                var token = new JwtSecurityToken(
                    issuer: issuer!.ToString(),
                    audience: audience!.ToString(),
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15), //la vigencia del token de acceso sera de 15 minutos
                    signingCredentials: credentials
                );
                //Se genera el token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (System.Exception error)
            {
                throw new($"Error al generar token \n{error.Message}");
            }

        }

        public async Task<UserModel> CreateUser(UserModel NewUser)
        {
            try
            {
                if (NewUser == null)
                {
                    throw new ArgumentNullException(nameof(NewUser), "El usuario no puede ser nulo.");
                }

                await Collection.InsertOneAsync(NewUser);

                return NewUser;

            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new Exception("Error: Ya existe un usuario con ese identificador único.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creando el usuario: {ex.Message}", ex);
            }
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        async public Task<UserModel> GetUserByRefreshToken(string refreshToken)
        {
            var Filtered_User_By_RefreshToken = Builders<UserModel>.Filter.Eq(u => u.RefreshToken, refreshToken);

            return await Collection.FindAsync(Filtered_User_By_RefreshToken).Result.FirstAsync();
        }

        async public Task<bool> UpdateUser(UserModel User)
        {
            //Filtramos al usuario en la base de datos
            var filter = Builders<UserModel>.Filter.Eq(u => u.Id, User.Id);

            var update = Builders<UserModel>.Update
                .Set(u => u.Token, User.Token)
                .Set(u => u.RefreshToken, User.RefreshToken);

            var userUpdated = await Collection.UpdateOneAsync(filter, update);
            return userUpdated.ModifiedCount > 0;
        }

        public Task<UserModel> GenerateRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}