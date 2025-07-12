using IntegradorApi.Application.Interfaces;
using IntegradorApi.Application.Services;
using IntegradorApi.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string do MariaDB (ajuste se necess�rio)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularApp",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200") // <--- MUITO IMPORTANTE: A URL DO SEU FRONTEND ANGULAR
                                 .AllowAnyMethod() // Permite todos os m�todos HTTP (GET, POST, PUT, DELETE, etc.)
                                 .AllowAnyHeader(); // Permite todos os cabe�alhos
                                                    // .AllowCredentials(); // Use esta linha SE o seu frontend precisar enviar cookies ou credenciais (por exemplo, com autentica��o baseada em sess�o ou JWT com cookies)
                      });
});


builder.Services.AddScoped<IParametroService, ParametroService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection(); // <--- LINHA ADICIONADA/MOVIDA AQUI

// 2. Usar o middleware CORS ANTES de UseAuthorization() e UseEndpoints() / MapControllers()
app.UseCors("AllowAngularApp"); // <--- APLICA A POL�TICA CORS DEFINIDA


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
