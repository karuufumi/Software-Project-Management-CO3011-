using backend.Data;
using backend.repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ ADD CORS POLICY
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    
    // Or for production, specify exact origins:
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",           // React dev
                "http://localhost:5173",           // Vite dev
                "https://your-frontend.vercel.app" // Production frontend
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// PostgreSQL configuration
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Get connection string - Railway provides DATABASE_URL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") // Railway
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") // Render
    ?? builder.Configuration.GetConnectionString("DefaultConnection"); // Local

// Railway connection string needs to be converted from postgres:// to PostgreSQL format
if (!string.IsNullOrWhiteSpace(connectionString) && connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
    
    Console.WriteLine("✅ Railway DATABASE_URL converted to Npgsql format");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is required. Set DATABASE_URL environment variable.");
}

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped(typeof(IUserRepository<>), typeof(UserRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IQueueRepository, QueueRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

var app = builder.Build();

// Apply migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        Console.WriteLine("🔄 Connecting to Railway PostgreSQL...");
        var canConnect = await context.Database.CanConnectAsync();
        
        if (!canConnect)
        {
            Console.WriteLine("❌ Cannot connect to database");
            throw new Exception("Database connection failed");
        }
        
        Console.WriteLine("✅ Database connected");
        Console.WriteLine("🔄 Applying migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("✅ Migrations applied");
        
        Console.WriteLine("🌱 Seeding database...");
        await DbSeeder.SeedDatabase(context);
        Console.WriteLine("✅ Database ready");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Database setup error: {ex.Message}");
        Console.WriteLine($"📋 Stack trace: {ex.StackTrace}");
        
        if (ex.Message.Contains("already exists"))
        {
            Console.WriteLine("ℹ️ Tables already exist, attempting to seed...");
            try { await DbSeeder.SeedDatabase(context); } catch { }
        }
    }
}

// ✅ USE CORS - MUST BE BEFORE UseAuthorization()
app.UseCors("AllowAll"); // Use "Production" in production environment

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

// Root endpoint - HTML Homepage
app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Library Management System API</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }
        .container {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            max-width: 900px;
            width: 100%;
            padding: 40px;
            animation: fadeIn 0.5s ease-in;
        }
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        h1 {
            color: #667eea;
            font-size: 2.5em;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .status {
            display: inline-block;
            background: #10b981;
            color: white;
            padding: 5px 15px;
            border-radius: 20px;
            font-size: 0.4em;
            font-weight: 600;
            text-transform: uppercase;
            animation: pulse 2s infinite;
        }
        @keyframes pulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.7; }
        }
        .subtitle {
            color: #6b7280;
            font-size: 1.1em;
            margin-bottom: 30px;
        }
        .cors-badge {
            display: inline-block;
            background: #10b981;
            color: white;
            padding: 3px 10px;
            border-radius: 5px;
            font-size: 0.85em;
            margin-left: 10px;
        }
        .btn {
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 30px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }
        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>
            📚 Library Management System
            <span class='status'>● Live</span>
        </h1>
        <p class='subtitle'>
            RESTful API for managing library operations
            <span class='cors-badge'>✓ CORS Enabled</span>
        </p>
        <a href='/swagger' class='btn'>🚀 Open API Documentation</a>
    </div>
</body>
</html>
", "text/html"));

// Health check endpoint
app.MapGet("/health", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        
        return Results.Json(new
        {
            status = canConnect ? "healthy" : "unhealthy",
            cors = "enabled",
            database = new
            {
                connected = canConnect,
                type = "PostgreSQL",
                provider = "Railway"
            },
            timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new
        {
            status = "unhealthy",
            error = ex.Message,
            timestamp = DateTime.UtcNow
        });
    }
});

Console.WriteLine("========================================");
Console.WriteLine("✅ Application started successfully!");
Console.WriteLine($"📍 Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"📍 Database: Railway PostgreSQL");
Console.WriteLine($"🌐 CORS: Enabled (AllowAll)");
Console.WriteLine($"📍 Swagger: /swagger");
Console.WriteLine($"📍 Health: /health");
Console.WriteLine("========================================");

app.Run();