using System.Text;
using Learn_Managment_System_Backend.Config;
using Learn_Managment_System_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<DbConnection>();

//Agremos nuestros servicios
builder.Services.AddScoped<IUserService, UserService>();

//Declaramos el nombre para las configuraciones de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options=>{
    options.AddPolicy(name:MyAllowSpecificOrigins,
        policy => {
            policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials(); //se permiten cookies
        }
    );
});

// Configurar autenticación con JWT y cookies
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters{
         ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Permite el uso de autenticacion en los controladores
// app.UseAuthorization();


var dbConnection = app.Services.GetRequiredService<DbConnection>();
dbConnection.PingDatabase();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
//Mapeamos todos los controladores que tengamos disponibles
app.MapControllers();
app.Run();

