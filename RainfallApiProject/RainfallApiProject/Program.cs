using Microsoft.OpenApi.Models;
using RainfallApiProject.Services;

var builder = WebApplication.CreateBuilder(args);
var hostingEnv = builder.Environment;

// Add services to the container.
builder.Services.AddTransient<RainfallService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("1.0", new OpenApiInfo
    {
        Title = "Rainfall Api",
        Version = "1.",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com"),
        },
        Description = "An API which provides rainfall reading data"
    });

    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/1.0/swagger.json", "Rainfall Api");

       
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
