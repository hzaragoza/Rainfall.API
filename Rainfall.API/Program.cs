using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rainfall.Common.Logger;
using Rainfall.Common.Middleware;
using Rainfall.Repository.Implementation;
using Rainfall.Repository.Interface;
using Rainfall.Service.Implementation.Rainfall;
using Rainfall.Service.Interface.Rainfall;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();
var ServerSection = builder.Configuration.GetSection("Server");
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Rainfall Api",
        Version = "1.0",
        Description = "An API which provides rainfall reading data",
        Contact = new OpenApiContact()
        {
            Name = "Sorted",
            Url = new Uri("https://www.sorted.com")
        }
    });

    setup.EnableAnnotations();

    setup.IncludeXmlComments($"{System.AppDomain.CurrentDomain.BaseDirectory}Rainfall.API.xml");

    setup.AddServer(new OpenApiServer()
    {
        Url = $"https://localhost:{ServerSection.GetSection("Port").Get<string>()}",
        Description = "Rainfall Api"
    });

    //setup.AddServer(new OpenApiServer()
    //{
    //    Url = "https://localhost:3000",
    //    Description = "Rainfall Api"
    //});
});

// register service layer
builder.Services.AddScoped<IRainfallService, RainfallService>();

// register repository layer
builder.Services.AddScoped<IRainfallRepository, RainfallRepository>();

// register logger
builder.Logging.AddProvider(new FileLoggerProvider(builder.Environment.ContentRootPath));

// register middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// register httpClient Factory
builder.Services.AddHttpClient("httpclient-rainfall", client =>
{
    client.BaseAddress = new Uri("https://environment.data.gov.uk");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "v1"));
}

app.UseHttpsRedirection();

//app.UseResponseCompression();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
