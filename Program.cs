using Learn_Managment_System_Backend.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<DbConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Permite el uso de autenticacion en los controladores
// app.UseAuthorization();

//Mapeamos todos los controladores que tengamos disponibles
app.MapControllers();


var dbConnection = app.Services.GetRequiredService<DbConnection>();
dbConnection.PingDatabase();

app.Run();

