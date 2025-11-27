using Recipes.Api.Extensions;
using Recipes.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Configure Database (EF Core + SQL Server)
builder.Services.AddDatabaseServices(builder.Configuration);

// Add Business Services
builder.Services.AddApplicationServices();

// Add FluentValidation
builder.Services.AddValidationServices();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Map controllers
app.MapControllers();

app.Run();
