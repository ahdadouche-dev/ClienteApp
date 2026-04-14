using ClientesApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IClienteService, ClienteService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Habilita el middleware para generar el JSON de Swagger
    app.UseSwaggerUI(); //Monta la interfaz visual
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
