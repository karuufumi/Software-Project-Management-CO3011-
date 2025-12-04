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
        Console.WriteLine("🔄 Checking database...");
        
        // Check if database can be connected
        var canConnect = await context.Database.CanConnectAsync();
        
        if (canConnect)
        {
            Console.WriteLine("✅ Database connection successful!");
            
            // Get pending migrations
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"📋 Found {pendingMigrations.Count()} pending migration(s)");
                Console.WriteLine("🔄 Applying migrations...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migrations applied!");
            }
            else
            {
                Console.WriteLine("✅ Database is up to date (no pending migrations)");
            }
        }
        else
        {
            Console.WriteLine("⚠️ Cannot connect to database. Creating database...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Database created!");
        }
        
        Console.WriteLine("🌱 Seeding database...");
        await DbSeeder.SeedDatabase(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
        
        // If migration fails due to existing tables, try to continue anyway
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("⚠️ Tables already exist, skipping migration...");
            Console.WriteLine("🌱 Attempting to seed database...");
            
            try
            {
                await DbSeeder.SeedDatabase(context);
            }
            catch (Exception seedEx)
            {
                Console.WriteLine($"⚠️ Seeding error (this is OK if already seeded): {seedEx.Message}");
            }
        }
        else
        {
            Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
            throw;
        }
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
    },
    documentation = new
    {
        books = "/api/book",
        users = "/api/user",
        borrow = "/api/borrow",
        queue = "/api/queue",
        membership = "/api/membership",
        dashboard = "/api/dashboard"
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
Console.WriteLine("📍 Swagger UI: /swagger");
Console.WriteLine("📍 API Base URL: /api\n");

app.Run();