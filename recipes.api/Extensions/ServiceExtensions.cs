using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Recipes.Api.Data;
using Recipes.Api.Data.Seeding;
using Recipes.Api.Services.Implementations;
using Recipes.Api.Services.Interfaces;

namespace Recipes.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecipesDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register business services
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IIngredientService, IngredientService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>();

        return services;
    }

    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }

    public static IServiceCollection AddDevelopmentServices(
        this IServiceCollection services,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddScoped<DatabaseSeeder>();
            services.AddScoped<MasterDataSeeder>();
            services.AddScoped<RecipeSeeder>();
            services.AddScoped<UsageHistorySeeder>();
        }

        return services;
    }
}
