using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen();
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

    setup.AddServer(new OpenApiServer()
    {
        Url = "http://localhost:3000",
        Description = "Rainfall Api"
    });
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

app.UseAuthorization();

app.MapControllers();

app.Run();
