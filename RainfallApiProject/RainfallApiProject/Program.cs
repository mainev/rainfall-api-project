using Microsoft.OpenApi.Models;
using RainfallApiProject.Services;

var builder = WebApplication.CreateBuilder(args);
var hostingEnv = builder.Environment;

// Services
builder.Services.AddScoped<RainfallService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("1.0", new OpenApiInfo
    {
        Title = "Rainfall Api",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com"),
        },
        Description = "An API which provides rainfall reading data"
    });

    c.AddServer(new OpenApiServer {
        Url = "http://localhost:3000",
        Description = "Operations relating to rainfall"
    });

    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{hostingEnv.ApplicationName}.xml");

    // This is added so all decimal data types will use the format=decimal in the OpenApi spec
    c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
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

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
