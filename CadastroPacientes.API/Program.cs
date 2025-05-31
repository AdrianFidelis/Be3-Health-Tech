using CadastroPacientes.Application.Services;
using CadastroPacientes.Domain.Interfaces;
using CadastroPacientes.Infrastructure.Database;
using CadastroPacientes.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// registra DbConnectionFactory
builder.Services.AddSingleton<DbConnectionFactory>();

// registra o repositório Dapper
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<PacienteService>();


// adiciona controllers e swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
