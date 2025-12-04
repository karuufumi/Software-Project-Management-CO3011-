using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;

// FORCE console output to appear
Console.WriteLine("========================================");
Console.WriteLine("🚀 APPLICATION STARTING");
Console.WriteLine("========================================");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("✅ Builder created");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine("✅ Services registered");

// Enable legacy timestamp behavior for PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
Console.WriteLine("✅ PostgreSQL legacy timestamp behavior enabled");

// Debug ALL environment variables
Console.WriteLine("\n🔍 ENVIRONMENT VARIABLES:");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT = {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
Console.WriteLine($"DOTNET_ENVIRONMENT = {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}");

// Check connection string from all sources
var connFromConfig = builder.Configuration.GetConnectionString("DefaultConnection");
var connFromEnv1 = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
var connFromEnv2 = Environment.GetEnvironmentVariable("DATABASE_URL");

Console.WriteLine($"\n🔍 CONNECTION STRING SOURCES:");
Console.WriteLine($"1. Config (appsettings.json): {(string.IsNullOrEmpty(connFromConfig) ? "NULL/EMPTY" : "EXISTS (length: " + connFromConfig.Length + ")")}");
Console.WriteLine($"2. Env (ConnectionStrings__DefaultConnection): {(string.IsNullOrEmpty(connFromEnv1) ? "NULL/EMPTY" : "EXISTS (length: " + connFromEnv1.Length + ")")}");
Console.WriteLine($"3. Env (DATABASE_URL): {(string.IsNullOrEmpty(connFromEnv2) ? "NULL/EMPTY" : "EXISTS (length: " + connFromEnv2.Length + ")")}");

// Try all sources
string? connectionString = null;

if (!string.IsNullOrWhiteSpace(connFromEnv1))
{
    connectionString = connFromEnv1;
    Console.WriteLine("✅ Using ConnectionStrings__DefaultConnection");
}
else if (!string.IsNullOrWhiteSpace(connFromEnv2))
{
    connectionString = connFromEnv2;
    Console.WriteLine("✅ Using DATABASE_URL");
}
else if (!string.IsNullOrWhiteSpace(connFromConfig))
{
    connectionString = connFromConfig;
    Console.WriteLine("✅ Using appsettings.json");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("\n❌❌❌ FATAL ERROR ❌❌❌");
    Console.WriteLine("No connection string found!");
    Console.WriteLine("Please add environment variable in Render:");
    Console.WriteLine("Key: ConnectionStrings__DefaultConnection");
    Console.WriteLine("Value: postgresql://user:pass@host/db");
    Console.WriteLine("❌❌❌❌❌❌❌❌❌❌❌❌❌❌");
    
    // Don't throw, just use a dummy to see what happens
    connectionString = "Host=localhost;Database=dummy";
    Console.WriteLine("⚠️ Using dummy connection string to continue startup");
}
else
{
    Console.WriteLine($"✅ Connection string length: {connectionString.Length}");
    Console.WriteLine($"✅ Starts with: {connectionString.Substring(0, Math.Min(20, connectionString.Length))}...");
}

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine("✅ DbContext configured");

// Register repositories
builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

Console.WriteLine("✅ Repositories registered");

var app = builder.Build();

Console.WriteLine("✅ App built");

// Database setup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("\n🔄 Testing database connection...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (!canConnect)
        {
            Console.WriteLine("❌ Cannot connect to database");
        }
        else
        {
            Console.WriteLine("✅ Database connected!");
            
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var pendingList = pendingMigrations.ToList();
            
            if (pendingList.Any())
            {
                Console.WriteLine($"📋 Applying {pendingList.Count} migration(s)...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migrations applied");
            }
            else
            {
                Console.WriteLine("✅ No pending migrations");
            }
            
            Console.WriteLine("🌱 Seeding database...");
            await DbSeeder.SeedDatabase(context);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ DATABASE ERROR:");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Type: {ex.GetType().Name}");
        
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("⚠️ Tables exist, trying to seed...");
            try
            {
                await DbSeeder.SeedDatabase(context);
            }
            catch { }
        }
        else if (ex.Message.Contains("Format of the initialization string"))
        {
            Console.WriteLine("\n❌ CONNECTION STRING IS INVALID OR EMPTY!");
            Console.WriteLine("This means the environment variable is not set in Render.");
            Console.WriteLine("Go to Render Dashboard → Environment → Add Variable:");
            Console.WriteLine("  Key: ConnectionStrings__DefaultConnection");
            Console.WriteLine("  Value: your PostgreSQL URL");
        }
        else
        {
            throw;
        }
    }
}

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Routes
app.MapGet("/", () => Results.Json(new
{
    status = "running",
    message = "Library Management System API",
    timestamp = DateTime.UtcNow
}));

app.MapGet("/health", () => Results.Json(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow
}));

Console.WriteLine("\n✅✅✅ APPLICATION STARTED ✅✅✅\n");

app.Run();