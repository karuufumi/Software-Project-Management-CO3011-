using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable legacy timestamp behavior for PostgreSQL (IMPORTANT!)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add DbContext with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

var app = builder.Build();

// Automatically apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("🔄 Applying migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied!");
        
        Console.WriteLine("🌱 Seeding database...");
        await DbSeeder.SeedDatabase(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
        Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
        throw;
    }
}

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// Root route
app.MapGet("/", () => Results.Json(new
{
    status = "success",
    message = "🎉 Library Management System API is running!",
    version = "1.0.0",
    database = "PostgreSQL",
    timestamp = DateTime.UtcNow,
    endpoints = new
    {
        swagger = "/swagger",
        api = "/api",
        health = "/health"
    }
}));

// Health check
app.MapGet("/health", () => Results.Json(new
{
    status = "healthy",
    database = "PostgreSQL",
    timestamp = DateTime.UtcNow
}));

Console.WriteLine("\n🚀 Application is running!");
Console.WriteLine($"📍 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"📍 Database: PostgreSQL");

app.Run();