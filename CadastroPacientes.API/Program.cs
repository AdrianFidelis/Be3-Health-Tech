using CadastroPacientes.Application.Services;
using CadastroPacientes.Domain.Interfaces;
using CadastroPacientes.Infrastructure.Database;
using CadastroPacientes.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura CORS para liberar o front-end em http://localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 2. Registra dependências da aplicação
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<IConvenioRepository, ConvenioRepository>();

// 3. Serviços do MVC/Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Pipeline da aplicação

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowAngularDev"); 

app.MapControllers();

app.Run();

public partial class Program { }
