# Recipes API

A RESTful API for managing recipes, ingredients, tags, and units of measurement built with ASP.NET Core 8.0.

## Project Structure

```
recipes/
├── api/
│   ├── Controllers/          # API endpoints
│   ├── Models/              # Entity models with EF Core configurations
│   ├── Dtos/                # Data Transfer Objects
│   │   ├── Requests/        # Request DTOs
│   │   └── Responses/       # Response DTOs
│   ├── Services/            # Business logic
│   │   ├── Interfaces/      # Service contracts
│   │   └── Implementations/ # Service implementations
│   ├── Validators/          # FluentValidation validators
│   ├── Data/               # Database context
│   ├── Extensions/         # Service configuration extensions
│   ├── Middleware/         # Custom middleware
│   └── Migrations/         # EF Core migrations
└── README.md
```

## Technology Stack

- **.NET 8.0** - Web API framework
- **Entity Framework Core 8.0** - ORM with SQL Server
- **FluentValidation 11.9** - Request validation
- **Swagger/OpenAPI** - API documentation

## Features

- Recipe management (CRUD operations)
- Ingredient management
- Tag management
- Unit of measurement management
- Recipe-ingredient associations with quantities
- Recipe-tag associations
- Recipe time tracking (when recipes were made)
- Automatic timestamp management (CreatedAt/UpdatedAt)
- Global exception handling
- Request validation

## Database Schema

### Main Entities
- **Recipes** - Recipe information (name, description, instructions, prep/cook time, difficulty)
- **Ingredients** - Available ingredients
- **Tags** - Recipe categorization tags
- **UnitsOfMeasurement** - Measurement units (cups, tablespoons, etc.)

### Junction Tables
- **RecipeIngredients** - Links recipes to ingredients with quantities (composite PK)
- **RecipeTags** - Links recipes to tags (composite PK)
- **RecipeTimesMade** - Tracks when recipes were prepared (composite PK)

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express, or Full)
- Your preferred IDE (Visual Studio, VS Code, Rider)

### Installation

1. Clone the repository
```bash
git clone <repository-url>
cd recipes
```

2. Restore dependencies
```bash
dotnet restore
```

3. Update connection string in `api/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RecipesDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

4. Apply database migrations
```bash
cd api
dotnet ef database update
```

5. Run the application
```bash
dotnet run
```

6. Access Swagger UI
```
https://localhost:5001/swagger
```

## Common CLI Commands

### Build & Run

```bash
# Build the project
dotnet build

# Build in Release mode
dotnet build -c Release

# Run the application
dotnet run

# Run with watch (auto-reload on file changes)
dotnet watch run

# Clean build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore
```

### Entity Framework Migrations

```bash
# Navigate to the API project
cd api

# Create a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations to database
dotnet ef database update

# Revert to a specific migration
dotnet ef database update <MigrationName>

# Remove the last migration (if not applied)
dotnet ef migrations remove

# List all migrations
dotnet ef migrations list

# Generate SQL script from migrations
dotnet ef migrations script

# Drop the database
dotnet ef database drop

# View DbContext information
dotnet ef dbcontext info

# Scaffold DbContext from existing database (reverse engineer)
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

### Testing & Debugging

```bash
# Run with specific environment
dotnet run --environment Development
dotnet run --environment Production

# Run on specific port
dotnet run --urls "https://localhost:5001;http://localhost:5000"

# Build with specific configuration
dotnet build -c Debug
dotnet build -c Release

# Publish the application
dotnet publish -c Release -o ./publish

# View detailed build output
dotnet build --verbosity detailed
```

### Package Management

```bash
# Add a NuGet package
dotnet add package <PackageName>

# Add a specific version
dotnet add package <PackageName> --version 1.2.3

# Remove a package
dotnet remove package <PackageName>

# List installed packages
dotnet list package

# Update packages
dotnet restore
```

### Solution Management

```bash
# Add project to solution
dotnet sln add <project-path>

# Remove project from solution
dotnet sln remove <project-path>

# List projects in solution
dotnet sln list

# Create new solution
dotnet new sln -n <SolutionName>
```

### Database Connection String Examples

#### LocalDB (Windows)
```json
"Server=(localdb)\\mssqllocaldb;Database=RecipesDb;Trusted_Connection=true;"
```

#### SQL Server Express
```json
"Server=localhost\\SQLEXPRESS;Database=RecipesDb;Trusted_Connection=true;TrustServerCertificate=true;"
```

#### SQL Server with credentials
```json
"Server=localhost;Database=RecipesDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
```

#### Azure SQL Database
```json
"Server=tcp:yourserver.database.windows.net,1433;Database=RecipesDb;User ID=username;Password=password;Encrypt=true;"
```

## API Endpoints

### Recipes
- `GET /api/recipes` - Get all recipes
- `GET /api/recipes/{id}` - Get recipe by ID (with details)
- `POST /api/recipes` - Create new recipe
- `PUT /api/recipes/{id}` - Update recipe
- `DELETE /api/recipes/{id}` - Delete recipe
- `POST /api/recipes/{id}/ingredients` - Add ingredient to recipe
- `POST /api/recipes/{id}/tags/{tagId}` - Add tag to recipe
- `POST /api/recipes/{id}/times-made` - Record when recipe was made

### Ingredients
- `GET /api/ingredients` - Get all ingredients
- `GET /api/ingredients/{id}` - Get ingredient by ID
- `POST /api/ingredients` - Create new ingredient
- `DELETE /api/ingredients/{id}` - Delete ingredient

### Tags
- `GET /api/tags` - Get all tags
- `GET /api/tags/{id}` - Get tag by ID
- `POST /api/tags` - Create new tag
- `DELETE /api/tags/{id}` - Delete tag

### Units of Measurement
- `GET /api/unitsofmeasurement` - Get all units
- `GET /api/unitsofmeasurement/{id}` - Get unit by ID
- `POST /api/unitsofmeasurement` - Create new unit
- `DELETE /api/unitsofmeasurement/{id}` - Delete unit

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourConnectionString"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### appsettings.Development.json
Override settings for development environment.

## Architecture Notes

### Entity Configuration Pattern
Entity configurations are implemented as nested classes within their respective model files using `IEntityTypeConfiguration<T>`. This keeps related code together and improves discoverability.

Example:
```csharp
public class Recipe
{
    public Guid Id { get; set; }
    // ... properties

    public class Configuration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            // EF Core configuration
        }
    }
}
```

### Automatic Timestamps
The `RecipesDbContext` automatically sets `CreatedAt` and `UpdatedAt` timestamps on all entities during `SaveChangesAsync()`.

### Validation
FluentValidation is used for request validation with automatic integration via `AddValidatorsFromAssemblyContaining<Program>()`.

## Development Workflow

1. Make code changes
2. Create migration: `dotnet ef migrations add YourMigrationName`
3. Review generated migration in `Migrations/` folder
4. Apply migration: `dotnet ef database update`
5. Test changes via Swagger UI or your client application

## Troubleshooting

### Migration Issues
```bash
# If migration fails, try:
dotnet ef database drop    # Warning: deletes all data
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Build Issues
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

### Connection Issues
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure `TrustServerCertificate=true` for local development
- Check firewall settings

## Contributing

1. Create a feature branch
2. Make your changes
3. Add/update tests (when implemented)
4. Submit a pull request

## License

This is a personal project for learning and portfolio purposes.

## Future Enhancements

- [ ] User authentication and authorization
- [ ] Recipe image uploads
- [ ] Recipe sharing and ratings
- [ ] Shopping list generation
- [ ] Nutritional information
- [ ] Recipe search and filtering
- [ ] Unit tests
- [ ] Integration tests
- [ ] Containerization (Docker)
- [ ] CI/CD pipeline
